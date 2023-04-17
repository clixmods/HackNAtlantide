using System;
using System.Collections.Generic;
using Post_Process_Custom;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class VolumetricRenderPass : ScriptableRenderPass
{

    #region Properties and Properties Shaders
    
    // Volumetric Post Process Setting
    private readonly int _propertyID_Intensity = Shader.PropertyToID("_Intensity");
    private readonly int _propertyID_Color = Shader.PropertyToID("_Color");
    private readonly int _propertyID_StepSize = Shader.PropertyToID("_StepSize");
    private readonly int _propertyID_DensityMultiplier = Shader.PropertyToID("_DensityMultiplier");
    private readonly int _propertyID_BlueNoiseOffset = Shader.PropertyToID("_BlueNoiseOffset");
    private readonly int _propertyID_BlueNoiseScale = Shader.PropertyToID("_BlueNoiseScale");
    private readonly int _propertyID_BlueNoise = Shader.PropertyToID("_BlueNoise");
    private readonly int _propertyID_FogMultiplier = Shader.PropertyToID("_FogMultiplier");
    private readonly int _propertyID_Extinction = Shader.PropertyToID("_Extinction");
    private readonly int _propertyID_Scattering = Shader.PropertyToID("_Scattering");
    private readonly int _propertyID_SkyboxExtinction = Shader.PropertyToID("_SkyboxExtinction");
    // Additionnal Light Intensity
    private readonly int shaderIDVolumetricBoost = Shader.PropertyToID("_AdditionalLightsVolumetricIntensity");
    private float[] m_additionalLightsVolumetricIntensities;
    // Main Light Intensity
    private readonly int shaderIDMainVolumetricBoost = Shader.PropertyToID("_MainLightVolumetricIntensity");
    // Obsololete
    private readonly int _propertyID_VolFog_BottomCorner = Shader.PropertyToID("_VolFog_BottomCorner");
    private readonly int _propertyID_VolFog_TopCorner = Shader.PropertyToID("_VolFog_TopCorner");
    // Additionnal Light Volumetric Tint
    private Vector4[] m_additionalLightsVolumetricTint;
    private readonly int shaderIDVolumetricTint = Shader.PropertyToID("_AdditionalLightsVolumetricTint");
    // Main Light Volumetric Tin
    private readonly int shaderIDMainVolumetricTint = Shader.PropertyToID("_MainLightVolumetricTint");
    // Blue Noise 
    private Texture[] m_additionalLightsVolumetricBlueNoiseTexture;
    private readonly int shaderIDVolumetricBlueNoiseTexture = Shader.PropertyToID("_AdditionalLightsVolumetricBlueNoiseTexture");
    private readonly int shaderIDMainVolumetricBlueNoiseTexture = Shader.PropertyToID("_MainLightVolumetricBlueNoiseTexture");
    // Blue Noise Cloud Main Light
    private readonly int _propertyID_BlueNoiseCloud = Shader.PropertyToID("_BlueNoiseDirectional");
    // Blue Noise offset
    private float[] m_additionalLightsVolumetricBlueNoiseOffset;
    private readonly int shaderIDVolumetricBlueNoiseOffset = Shader.PropertyToID("_AdditionalLightsVolumetricBlueNoiseOffset");
    private readonly int shaderIDMainVolumetricBlueNoiseOffset = Shader.PropertyToID("_MainLightVolumetricBlueNoiseOffset");
    // Blue Noise scale
    private float[] m_additionalLightsVolumetricBlueNoiseScale;
    private readonly int shaderIDVolumetricBlueNoiseScale= Shader.PropertyToID("_AdditionalLightsVolumetricBlueNoiseScale");
    private readonly int shaderIDMainVolumetricBlueNoiseScale = Shader.PropertyToID("_MainLightVolumetricBlueNoiseScale");
    // Additionnal Light properties
    private Texture[] m_additionalLightsCookies;
    
    private readonly int _propertyID_CameraMatrix = Shader.PropertyToID("_CameraToWorldMatrix");
    
    private readonly int shaderIDCookie = Shader.PropertyToID("_AdditionalLightsCookies");
    // Blur Setting
    private readonly int _propertyID_BlurStrength = Shader.PropertyToID("_BlurStrength");
    private readonly int _propertyID_BlurQuality = Shader.PropertyToID("_BlurQuality");
    private readonly int _propertyID_BlurDirection = Shader.PropertyToID("_BlurDirection");
    private readonly int _propertyID_BlurRadius = Shader.PropertyToID("_BlurRadius");
    // Additionnal Light Attenuation
    private readonly int _propertyID_AttenuationInner = Shader.PropertyToID("_AdditionalLightsVolumetricAttenuationInner");
    private readonly int _propertyID_AttenuationOuter = Shader.PropertyToID("_AdditionalLightsVolumetricAttenuationOuter");
    private float[] m_additionalLightsVolumetricAttenuationInner;
    private float[] m_additionalLightsVolumetricAttenuationOuter;
    #endregion
    public VolumetricScriptableRendererFeature.PassSettings settings;
    private RenderTargetIdentifier source;
    RenderTargetHandle tempTexture;

    private string profilerTag;
    public void Setup(RenderTargetIdentifier source)
    {
        this.source = source;
    }
    public VolumetricRenderPass(string profilerTag)
    {
        this.profilerTag = profilerTag;
    }
    RenderTargetHandle temptexture3;
    private readonly int _propertyID_vol = Shader.PropertyToID("_VolumetricRenderTexture");
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData   renderingData)
    {
        settings.material.SetInt(_propertyID_BlurQuality, settings.blurQuality);
        settings.material.SetInt(_propertyID_BlurDirection, settings.blurDirection);
        settings.material.SetInt(_propertyID_BlurRadius, settings.blurRadius);
        var original = renderingData.cameraData.cameraTargetDescriptor;
        if (Camera.current != null) //This is necessary so it uses the proper resolution in the scene window
        {
            renderingData.cameraData.cameraTargetDescriptor.width = (int)Camera.current.pixelRect.width / settings.downsample;
            renderingData.cameraData.cameraTargetDescriptor.height = (int)Camera.current.pixelRect.height / settings.downsample;
            original.width = (int)Camera.current.pixelRect.width;
            original.height = (int)Camera.current.pixelRect.height;
        }
        else //regular game window
        {
            renderingData.cameraData.cameraTargetDescriptor.width /= settings.downsample;
            renderingData.cameraData.cameraTargetDescriptor.height /= settings.downsample;
        }
        renderingData.cameraData.cameraTargetDescriptor.colorFormat = RenderTextureFormat.Default;
        temptexture3.id = 1;
        cmd.GetTemporaryRT(tempTexture.id, renderingData.cameraData.cameraTargetDescriptor);
        cmd.GetTemporaryRT(temptexture3.id, original);
    }

    
    // The actual execution of the pass. This is where custom rendering occurs.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.isSceneViewCamera)
            return; 
        
        CommandBuffer cmd = CommandBufferPool.Get();
 
        var stack = VolumeManager.instance.stack;
        var customEffect = stack.GetComponent<VolumetricPostProcessComponent>();
        //it is very important that if something fails our code still calls 
        //CommandBufferPool.Release(cmd) or we will have a HUGE memory leak
       // try
        //{
        if (customEffect.IsActive())
        {
            var materials = VolumetricPostProcessingMaterials.Instance;
            //here we set out material properties
            SetupMaterialVariables(renderingData, cmd, customEffect);
            //never use a Blit from source to source, as it only works with MSAA
            // enabled and the scene view doesnt have MSAA,
            // so the scene view will be pure black
            Blit(cmd, source, tempTexture.Identifier(),settings.material, 0);
            cmd.SetGlobalTexture(_propertyID_vol, tempTexture.Identifier());
            Blit(cmd, source, temptexture3.Identifier(), settings.material, 1);
            Blit(cmd, temptexture3.Identifier(), source);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    private void SetupMaterialVariables(RenderingData renderingData, CommandBuffer cmd,
        VolumetricPostProcessComponent customPostProcess)
    {
       
        // Get lightData
        var lightData = renderingData.lightData;
        var lights = lightData.visibleLights;
        // Create arrays
        m_additionalLightsVolumetricIntensities = new float[UniversalRenderPipeline.maxVisibleAdditionalLights];
        m_additionalLightsVolumetricTint = new Vector4[UniversalRenderPipeline.maxVisibleAdditionalLights];
        m_additionalLightsVolumetricBlueNoiseTexture = new Texture[UniversalRenderPipeline.maxVisibleAdditionalLights];
        m_additionalLightsVolumetricBlueNoiseScale = new float[UniversalRenderPipeline.maxVisibleAdditionalLights];
        m_additionalLightsVolumetricBlueNoiseOffset = new float[UniversalRenderPipeline.maxVisibleAdditionalLights];
        m_additionalLightsVolumetricAttenuationInner = new float[UniversalRenderPipeline.maxVisibleAdditionalLights];
        m_additionalLightsVolumetricAttenuationOuter = new float[UniversalRenderPipeline.maxVisibleAdditionalLights];
        // Apply values to array
        for (int i = 0, lightIter = 0;
             i < lights.Length && lightIter < UniversalRenderPipeline.maxVisibleAdditionalLights;
             i++)
        {
            VisibleLight light = lights[i];
            if (lightData.mainLightIndex != i)
            {
                if (VolumetricLight.AdditionnalVolumetricLights.TryGetValue(light.light, out var value))
                {
                    m_additionalLightsVolumetricIntensities[lightIter] = value.VolumetricBoost;
                    m_additionalLightsVolumetricTint[lightIter] = value.TintColor;
                    m_additionalLightsVolumetricBlueNoiseTexture[lightIter] = value.BlueNoise;
                    m_additionalLightsVolumetricBlueNoiseScale[lightIter] = value.BlueNoiseScale;
                    m_additionalLightsVolumetricBlueNoiseOffset[lightIter] = value.BlueNoiseOffset;
                    m_additionalLightsVolumetricAttenuationOuter[lightIter] = value.AttenuationOuter;
                    m_additionalLightsVolumetricAttenuationInner[lightIter] = value.AttenuationInner;
                }

                lightIter++;
            }
            else
            {
                if (VolumetricLight.MainVolumetricLight != null)
                {
                    cmd.SetGlobalFloat(shaderIDMainVolumetricBoost, VolumetricLight.MainVolumetricLight.VolumetricBoost);
                    cmd.SetGlobalVector(shaderIDMainVolumetricTint, VolumetricLight.MainVolumetricLight.TintColor);
                    cmd.SetGlobalFloat(shaderIDMainVolumetricBlueNoiseScale,
                        VolumetricLight.MainVolumetricLight.BlueNoiseScale);
                    cmd.SetGlobalFloat(shaderIDMainVolumetricBlueNoiseOffset,
                        VolumetricLight.MainVolumetricLight.BlueNoiseOffset);
                    cmd.SetGlobalTexture(_propertyID_BlueNoiseCloud, VolumetricLight.MainVolumetricLight.BlueNoise);
                }
            }
        }
        var materials = VolumetricPostProcessingMaterials.Instance;
        // Send array to Shader 
        cmd.SetGlobalFloatArray(shaderIDVolumetricBoost, m_additionalLightsVolumetricIntensities);
        cmd.SetGlobalVectorArray(shaderIDVolumetricTint, m_additionalLightsVolumetricTint);
        cmd.SetGlobalFloatArray(_propertyID_AttenuationOuter, m_additionalLightsVolumetricAttenuationOuter);
        cmd.SetGlobalFloatArray(_propertyID_AttenuationInner, m_additionalLightsVolumetricAttenuationInner);
        settings.material.SetInt(_propertyID_BlurQuality, settings.blurQuality);
        settings.material.SetInt(_propertyID_BlurDirection, settings.blurDirection);
        settings.material.SetInt(_propertyID_BlurRadius, settings.blurRadius);
        settings.material.SetFloat(_propertyID_Intensity, customPostProcess.intensity.value);
        settings.material.SetColor(_propertyID_Color, customPostProcess.overlayColor.value);
        settings.material.SetFloat(_propertyID_StepSize, customPostProcess.stepSize.value);
        settings.material.SetFloat(_propertyID_DensityMultiplier, customPostProcess.densityMultiplier.value);
        settings.material.SetFloat(_propertyID_BlueNoiseOffset, customPostProcess.blueNoiseOffset.value);
        settings.material.SetFloat(_propertyID_BlueNoiseScale, customPostProcess.blueNoiseScale.value);
        settings.material.SetFloat(_propertyID_FogMultiplier, customPostProcess.fogMultiplier.value);
        settings.material.SetFloat(_propertyID_Extinction, customPostProcess.extinction.value);
        settings.material.SetFloat(_propertyID_Scattering, customPostProcess.scattering.value);
        settings.material.SetFloat(_propertyID_SkyboxExtinction, customPostProcess.skyboxExtinction.value);
        Transform cameraTransform = renderingData.cameraData.camera.transform;
        settings.material.SetVector(_propertyID_VolFog_BottomCorner,
            cameraTransform.position - Vector3.one * customPostProcess.minDistance.value);
        settings.material.SetVector(_propertyID_VolFog_TopCorner,
            cameraTransform.position + Vector3.one * customPostProcess.maxDistance.value);

        if (customPostProcess.blueNoise.value != null)
        {
            settings.material.SetTexture(_propertyID_BlueNoise, customPostProcess.blueNoise.value);
        }
        settings.material.SetMatrix(_propertyID_CameraMatrix, renderingData.cameraData.camera.cameraToWorldMatrix);
         
    }
    // Called when the camera has finished rendering.
    // Here we release/cleanup any allocated resources that were created by this pass.
    // Gets called for all cameras i na camera stack.
    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if (cmd == null) throw new ArgumentNullException("cmd");
        
        // Since we created a temporary render texture in OnCameraSetup, we need to release the memory here to avoid a leak.
        cmd.ReleaseTemporaryRT(_propertyID_vol);
    }
    
}