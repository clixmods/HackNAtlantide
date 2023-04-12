using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/*
 * How to create custom effect for post process in URP https://www.febucci.com/2022/05/custom-post-processing-in-urp/
 * 
 */

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/VolumetricPostProcessComponent", typeof(UniversalRenderPipeline))]
public class VolumetricPostProcessComponent : VolumeComponent, IPostProcessComponent
{
    [Serializable]
    public sealed class TransformParameter : VolumeParameter<Transform>
    {
        public TransformParameter(Transform value, bool overrideState = false)
            : base(value, overrideState){}
    }
    // For example, an intensity parameter that goes from 0 to 1
    public ClampedIntParameter intensity = new ClampedIntParameter(value: 0, min: 0, max: 1, overrideState: true);
    // A color that is constant even when the weight changes
    public NoInterpColorParameter overlayColor = new NoInterpColorParameter(Color.white);
    // Other 'Parameter' variables you might have
    public ClampedFloatParameter stepSize = new ClampedFloatParameter(value: 0.1f, min: 0.01f, max: 20, overrideState: true);
    public FloatParameter densityMultiplier = new FloatParameter(value: 0.2f);
    public TextureParameter blueNoise = new TextureParameter(value: null);
    public FloatParameter blueNoiseOffset = new FloatParameter(value: 0f);
    public FloatParameter blueNoiseScale = new FloatParameter(value: 0f);
    public FloatParameter fogMultiplier = new FloatParameter(value: 1f);
    public FloatParameter extinction = new FloatParameter(value: 0.3f);
    public FloatParameter scattering = new FloatParameter(value: 1f);
    public FloatParameter skyboxExtinction  = new FloatParameter(value: 1f);
    public FloatParameter maxDistance  = new FloatParameter(value: 100f);
    public FloatParameter minDistance  = new FloatParameter(value: 100f);
    
    // Tells when our effect should be rendered
    public bool IsActive() => intensity.value > 0;
   
    // I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}