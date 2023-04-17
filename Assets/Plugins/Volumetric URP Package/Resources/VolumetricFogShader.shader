Shader "Hidden/Custom/VolumetricFogShader"
{
	Properties 
	{
	    _MainTex ("Main Texture", 2D) = "white" {}
	 
	   
	}

    HLSLINCLUDE
    //#include "Lighting.cginc"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LightCookie/LightCookie.hlsl"

    // Universal Pipeline keywords
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE 
#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
#pragma multi_compile_fragment _ _SHADOWS_SOFT
#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
#pragma multi_compile_fragment _ _LIGHT_LAYERS
#pragma multi_compile_fragment _ _LIGHT_COOKIES
#pragma multi_compile _ _FORWARD_PLUS
#pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
    TEXTURE2D(_CameraDepthTexture);
    SAMPLER(sampler_CameraDepthTexture);
    
    float _BlueNoiseOffset;
    float _BlueNoiseScale;
    TEXTURE2D(_BlueNoise);
    SAMPLER(sampler_BlueNoise);
    
    TEXTURE2D(_BlueNoiseDirectional);
    SAMPLER(sampler_BlueNoiseDirectional);

    TEXTURE2D(_MainTex);
    SAMPLER(sampler_MainTex);

    float2 _MainTex_TexelSize;
    TEXTURE2D_X(_VolumetricRenderTexture);
    SAMPLER(sampler_VolumetricRenderTexture);

    
    
    // Variables
    float _Intensity;
    float _FogMultiplier;
    float _Extinction;
    float _Scattering;
    float _Range;
    float _SkyboxExtinction;
    
    float4 _Color;
    float4x4 _CameraToWorldMatrix;
    float3 _VolFog_BottomCorner;
    float3 _VolFog_TopCorner;
    half _StepSize;
    half _DensityMultiplier;

    float4 _LightSplitsNear;
    float4 _LightSplitsFar;
    float4x4 unity_WorldToShadow[4];

    half _AdditionalLightsVolumetricIntensity[8];
    half _MainLightVolumetricIntensity;

    half4 _AdditionalLightsVolumetricTint[8];
    half4 _MainLightVolumetricTint;

    half _AdditionalLightsVolumetricBlueNoiseScale[8];
    half _MainLightVolumetricBlueNoiseScale;

    half _AdditionalLightsVolumetricBlueNoiseOffset[8];
    half _MainLightVolumetricBlueNoiseOffset;

    half _AdditionalLightsVolumetricAttenuationOuter[8];
    half _AdditionalLightsVolumetricAttenuationInner[8];
    
    float2 rayBoxDist(float3 boundsMinPosition, float3 boundsMaxPosition, float3 rayOrigin, float3 inverseRayDirection)
    {
        float3 rayDir0 = (boundsMinPosition - rayOrigin) * inverseRayDirection;
        float3 rayDir1 = (boundsMaxPosition - rayOrigin) * inverseRayDirection;
        float3 rayMin = min(rayDir0, rayDir1);
        float3 rayMax = max(rayDir0, rayDir1);
        // Distance
        float distanceA = max(max(rayMin.x, rayMin.y), rayMin.z);
        float distanceB = min(rayMax.x, min(rayMax.y, rayMax.z));

        float distanceToBox = max(0, distanceA);
        float distanceInsideBox = max(0, distanceB - distanceToBox);
        return float2(distanceToBox, distanceInsideBox);
    }

    float4 getCascadeWeights(float depth)
    {
        float4 zNear = float4(depth >= _LightSplitsNear);
        float4 zFar = float4(depth < _LightSplitsFar);
        float4 weights = zNear * zFar;
        return weights;
    }
    real4 SampleAdditionalLightsCookieAtlasTextureClix(float2 uv)
    {
        return SAMPLE_TEXTURE2D_LOD(_AdditionalLightsCookieAtlasTexture, sampler_AdditionalLightsCookieAtlasTexture, uv,0);
    }
    real3 SampleAdditionalLightCookieClix(int perObjectLightIndex, float3 samplePositionWS)
    {
        if(!IsLightCookieEnabled(perObjectLightIndex))
            return real3(1,1,1);

        int lightType     = GetLightCookieLightType(perObjectLightIndex);
        int isSpot        = lightType == URP_LIGHT_TYPE_SPOT;
        int isDirectional = lightType == URP_LIGHT_TYPE_DIRECTIONAL;

        float4x4 worldToLight = GetLightCookieWorldToLightMatrix(perObjectLightIndex);
        float4 uvRect = GetLightCookieAtlasUVRect(perObjectLightIndex);

        float2 uv;
        if(isSpot)
        {
            uv = ComputeLightCookieUVSpot(worldToLight, samplePositionWS, uvRect);
        }
        else if(isDirectional)
        {
            uv = ComputeLightCookieUVDirectional(worldToLight, samplePositionWS, uvRect, URP_TEXTURE_WRAP_MODE_REPEAT);
        }
        else
        {
            uv = ComputeLightCookieUVPoint(worldToLight, samplePositionWS, uvRect);
        }

        real4 color = SampleAdditionalLightsCookieAtlasTextureClix(uv);

        return IsAdditionalLightsCookieAtlasTextureRGBFormat() ? color.rgb
                : IsAdditionalLightsCookieAtlasTextureAlphaFormat() ? color.aaa
                : color.rrr;
    }

    Light GetAdditionalLightPostProcess(uint i, float3 positionWS)
    {
        return GetAdditionalPerObjectLight(i, positionWS);
    }
    half ShadowAtten(float3 worldPosition)
    {
        return MainLightRealtimeShadow(TransformWorldToShadowCoord(worldPosition));
    }
half4 _LightOcclusionProbInfo;
    half LocalShadowMap(float3 worldPos)
    {
        int lightAmount = _AdditionalLightsCount;
        half product = 0;

        for(int i = 0; i < lightAmount; i++)
        {
            Light light = GetAdditionalLightPostProcess(i, worldPos);
            half shadow = AdditionalLightShadow(i, worldPos, light.direction,1,_LightOcclusionProbInfo) ;
            if(shadow > 0)
            { 
                product += shadow ;
            }
        }
        return product;
    }
    float getShadowTerm(float distanceTravelled, float3 rayOrigin)
    {
        float4 weights = getCascadeWeights(distanceTravelled);
            
        float3 shadowCoord0 = mul(unity_WorldToShadow[0], float4(rayOrigin,1)).xyz;
        float3 shadowCoord1 = mul(unity_WorldToShadow[1], float4(rayOrigin,1)).xyz;
        float3 shadowCoord2 = mul(unity_WorldToShadow[2], float4(rayOrigin,1)).xyz;
        float3 shadowCoord3 = mul(unity_WorldToShadow[3], float4(rayOrigin,1)).xyz;
        float4 shadowCoord = float4(shadowCoord0 * weights[0]+shadowCoord1 * weights[1]+shadowCoord2 * weights[2]+shadowCoord3 * weights[3],1);
        Light mainLight = GetMainLight();
     
        float shadow_term = ShadowAtten(rayOrigin);
        shadow_term = shadow_term+ LocalShadowMap(rayOrigin);

        shadow_term = shadow_term > shadowCoord.z;
        return shadow_term;
    }
    Light GetMainLightClix(float4 shadowCoord, float3 positionWS, half4 shadowMask)
    {
        Light light = GetMainLight(shadowCoord);
        light.shadowAttenuation = MainLightShadow(shadowCoord, positionWS, shadowMask, _MainLightOcclusionProbes);

        // #if defined(_LIGHT_COOKIES)
        //     real3 cookieColor = SampleMainLightCookie(positionWS);
        //     light.color *= cookieColor;
        // #endif

        return light;
    }
    real3 SampleMainLightCookieClix(float3 samplePositionWS)
    {
        if(!IsMainLightCookieEnabled())
            return real3(1,1,1);

        float2 uv = ComputeLightCookieUVDirectional(_MainLightWorldToLight, samplePositionWS, float4(1, 1, 0, 0), URP_TEXTURE_WRAP_MODE_NONE);
        real4 color = SAMPLE_TEXTURE2D_LOD(_MainLightCookieTexture, sampler_MainLightCookieTexture, uv,0);

        return IsMainLightCookieTextureRGBFormat() ? color.rgb
                 : IsMainLightCookieTextureAlphaFormat() ? color.aaa
                 : color.rrr;
    }
    float3 GetColor( float3 rayOrigin)
    {
        Light mainLight = GetMainLightClix(TransformWorldToShadowCoord(rayOrigin), rayOrigin, 0);
        int lightAmount = _AdditionalLightsCount;
        float3 product;
        float3 mainProduct = mainLight.color * _MainLightVolumetricTint * _MainLightVolumetricIntensity * SampleMainLightCookieClix(rayOrigin)*mainLight.shadowAttenuation ;
        //mainProduct *= (mainLight.distanceAttenuation); dont work in build
          return mainProduct;
        float4 uvRect = float4(1,1,1,1);
        for(int i = 0; i < lightAmount; i++)
        {
            Light light = GetAdditionalLightPostProcess(i, rayOrigin);
            float4x4 worldToLight = GetLightCookieWorldToLightMatrix(i);
            // Translate and rotate 'positionWS' into the light space.
            float4 positionLS  = mul(worldToLight, float4(rayOrigin, 1));
            float3 sampleDirLS = normalize(positionLS.xyz / positionLS.w);
            // Project direction to Octahederal quad UV.
            float2 positionUV = saturate(PackNormalOctQuadEncode(sampleDirLS) * 0.5 + 0.5);
            // Remap to atlas texture
            float2 positionAtlasUV = uvRect.xy * float2(positionUV) + uvRect.zw;
            float2 uv = positionAtlasUV;
            float3 oof = SAMPLE_TEXTURE2D_LOD(_BlueNoiseDirectional, sampler_BlueNoiseDirectional, uv,0);
            float distanceAttenuation = light.distanceAttenuation;
             if(light.distanceAttenuation <= _AdditionalLightsVolumetricAttenuationInner[i])
            {
                 continue;
                 //distanceAttenuation = _AdditionalLightsVolumetricAttenuationInner[i] - light.distanceAttenuation;
            }
            if(light.distanceAttenuation >= _AdditionalLightsVolumetricAttenuationOuter[i])
            {
                continue;
                //distanceAttenuation = _AdditionalLightsVolumetricAttenuationInner[i] + light.distanceAttenuation;
            }
            if( length(light.color.rgb)  > 0 )
            {
                product.rgb += light.color * _AdditionalLightsVolumetricTint[i] * distanceAttenuation * _AdditionalLightsVolumetricIntensity[i] * SampleAdditionalLightCookieClix(i,rayOrigin)
                 ;
            }
           
        }
        if(length(product.rgb) > 0)
            return mainProduct;

        return mainProduct ;
    }
    
    struct v2f
    {
        float2 uv : TEXCOORD0;
        float4 vertex : SV_POSITION;
        float3 viewDirection : TEXCOORD1;
    };

    v2f vertVolumetric(uint vertexID : SV_VertexID)
    {
    
        // On calcule les vertex position
        float x = (vertexID != 1) ? -1 : 3;
        float y = (vertexID == 2) ? -3 : 1;
        float3 vertexPosition = float3(x, y, 1.0);
        float far = _ProjectionParams.z;

        v2f output;
        output.vertex = float4(vertexPosition.x, -vertexPosition.y, 1, 1);
        output.uv = (vertexPosition.xy + 1) / 2;
        
         //  if (_ProjectionParams.x < 0.0)
         // {
         //     output.uv.y = 1.0 - output.uv.y;
         // }
        float3 viewDirectionWithCamera = mul(unity_CameraInvProjection, vertexPosition.xyzz * far).xyz;
        output.viewDirection = mul(_CameraToWorldMatrix, float4(viewDirectionWithCamera, 0)).xyz;
           // On non-GL when AA is used, the main texture and scene depth texture
            // will come out in different vertical orientations.
            // So flip sampling of the texture when that is the case (main texture
            // texel size will have negative Y).

       
        return output;
    }
     v2f vertProject(uint vertexID : SV_VertexID)
    {
    
        // On calcule les vertex position
        float x = (vertexID != 1) ? -1 : 3;
        float y = (vertexID == 2) ? -3 : 1;
        float3 vertexPosition = float3(x, y, 1.0);
        float far = _ProjectionParams.z;

        v2f output;
        output.vertex = float4(vertexPosition.x, -vertexPosition.y, 1, 1);
        output.uv = (vertexPosition.xy + 1) / 2;
        
         if (_ProjectionParams.x < 0.0)
        {
            output.uv.y = 1.0 - output.uv.y;
        }
        float3 viewDirectionWithCamera = mul(unity_CameraInvProjection, vertexPosition.xyzz * far).xyz;
        output.viewDirection = mul(_CameraToWorldMatrix, float4(viewDirectionWithCamera, 0)).xyz;
           // On non-GL when AA is used, the main texture and scene depth texture
            // will come out in different vertical orientations.
            // So flip sampling of the texture when that is the case (main texture
            // texel size will have negative Y).

       
        return output;
    }
    float random (float2 uv)
    {
        return frac(sin(dot(uv,float2(12.9898,78.233)))*43758.5453123);}
 
    float2 squareUV(float2 uv)
    {
        return float2((uv.x * _ScreenParams.x)/1000 , (uv.y * _ScreenParams.y)/1000);
    }
    float GetNoiseLight(float3 xyz)
    {
        int lightAmount = _AdditionalLightsCount;
        float3 product ;
        float3 mainProduct = SAMPLE_TEXTURE2D(_BlueNoiseDirectional, sampler_BlueNoiseDirectional,(xyz*_MainLightVolumetricBlueNoiseScale) - _Time.xy*_MainLightVolumetricBlueNoiseOffset);
        product = mainProduct;
        for(int i = 0; i < lightAmount; i++)
        {
            product += SAMPLE_TEXTURE3D(_BlueNoiseDirectional, sampler_BlueNoiseDirectional,(xyz*_AdditionalLightsVolumetricBlueNoiseScale[i]) + _Time.y* _AdditionalLightsVolumetricBlueNoiseOffset[i]);;
        }
        if(length(product.rgb) > 0)
            return mainProduct;
        
        return mainProduct;
    }
    
    float4 frag(v2f input) : SV_Target
    {
        //UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        
        float3 rayOrigin = _WorldSpaceCameraPos;
        float3 viewLength = length(input.viewDirection);
        float3 rayDirection  = input.viewDirection / viewLength;
        float2 rayBoxInfo = rayBoxDist(_VolFog_BottomCorner, _VolFog_TopCorner, rayOrigin, 1/rayDirection );
        float distanceToBox = rayBoxInfo.x;
        float distanceInsideBox = rayBoxInfo.y;
        float nonLinearDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,  sampler_CameraDepthTexture, input.uv);
        float depth = Linear01Depth(nonLinearDepth,_ZBufferParams) * viewLength;
        float3 forward = mul((float3x3)unity_CameraToWorld, float3(0,0,1));
        float projectedDepth = LinearEyeDepth(depth,_ZBufferParams) / dot(forward, rayDirection);
        float distanceLimit = min(depth - distanceToBox , distanceInsideBox);
        float3 entryPoint = rayOrigin + rayDirection * distanceToBox;
        float distanceTravelled = SAMPLE_TEXTURE2D(_BlueNoise, sampler_BlueNoise,(squareUV(input.uv* _BlueNoiseScale   ) + random(_Time.x)* _BlueNoiseOffset ) ) ;
        distanceTravelled *= _StepSize;
        
        float transmittance = 0;
        float4 colortoadd = float4(0,0,0,0);
        
        float extinction = length(_WorldSpaceCameraPos - entryPoint) * _Extinction ;
        float4 colorCloud = GetNoiseLight(   mul( float3( rayDirection.x,rayDirection.y, rayDirection.z) ,length(forward) )  )  + distanceTravelled ;
        while(distanceTravelled < distanceLimit )
        {
            rayOrigin = (entryPoint + rayDirection * (distanceTravelled ) ) ;
            half density = getShadowTerm(distanceTravelled, rayOrigin) * (_DensityMultiplier)    ;
            if(density > 0)
            {
                transmittance += exp(-density ) ;
                half scattering = _Scattering  * _StepSize  * density;
				extinction += _Extinction * _StepSize * density ;
				half4 light =  scattering * exp(-extinction)   ;
				
                colortoadd.rgb += (GetColor(rayOrigin) )  * light  *exp(distanceTravelled/distanceLimit)  ;
                
                if(transmittance < 0.01)
                {
                    break;
                }
            }
            distanceTravelled += _StepSize  ;
        }
         colortoadd.rgb *= colorCloud;
         //colorCloud.rgb += GetNoiseLight( rayDirection );
       
        if (Linear01Depth(nonLinearDepth,_ZBufferParams) > 0.999999)
        {
            colortoadd.rgb *= _SkyboxExtinction;
        }
        half4 color = float4(0,0,0,0); // from fragComposite
        
               
        color.rgb +=  (colortoadd.rgb   * _FogMultiplier) ;
       
        return color;
    }
    float4 _TempRTB_TexelSize;
    half _BlurStrength;
    half _BlurQuality;
    half _BlurDirection;
    half _BlurRadius;
    
             half4 blurVertical(float2 uv) : SV_TARGET
                {
                    half4 color = half4(0,0,0,0);
                    float pi = 6.28;// Blur calculations
                    float Radius = _BlurRadius/_ScreenParams.xy;
                           
                    for( float d=0.0; d<pi; d+=pi/_BlurDirection)
                    {
                        for(float i=1.0/_BlurQuality; i<=1.0; i+=1.0/_BlurQuality)
                        {
			                color += SAMPLE_TEXTURE2D_X( _VolumetricRenderTexture,sampler_VolumetricRenderTexture, uv+float2(cos(d),sin(d))*Radius*i );		
                        }
                    }
                    // Output to screen
                    return color /= _BlurQuality * _BlurDirection - 15.0;;
                }

                 float4 fragCompositeVol(v2f input) : SV_Target
                {
                    
                    float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                  //  float4 vol = SAMPLE_TEXTURE2D_X(_VolumetricRenderTexture, sampler_VolumetricRenderTexture, input.uv);
                    //float4 colora = float4(1,1,1,1);
                    #if UNITY_UV_STARTS_AT_TOP
                        input.uv.y = 1.0 - input.uv.y;
                        #endif
                         
                        if (_ProjectionParams.y < 0.0)
                        {
                            input.uv.y = 1.0 - input.uv.y;
                        }
                    return color + blurVertical(input.uv);
                }

   
   
    ENDHLSL
    
    SubShader
    {
       Cull Off ZWrite Off ZTest Always
       Pass
        {
            HLSLPROGRAM
             
                #pragma vertex vertVolumetric
                #pragma fragment frag
            ENDHLSL        
        }  

           Pass
        {
            HLSLPROGRAM
                #pragma vertex vertProject
                #pragma fragment fragCompositeVol
            ENDHLSL        
        }
    }
}