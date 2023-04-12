using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class VolumetricScriptableRendererFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class PassSettings
    {
        public Material material;
        // Where/when the render pass should be injected during the rendering process.
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        // Used for any potential down-sampling we will do in the pass.
        [Range(1,16)] public int downsample = 16;
        [Range(1, 64)] public int blurQuality = 5;
        [Range(1, 100)] public int blurRadius = 37;
        [Range(1, 64)] public int blurDirection = 16;
        // additional properties ...
    }
    VolumetricRenderPass pass;
    public PassSettings passSettings = new();
    public override void Create()
    {
        pass = new VolumetricRenderPass("passSettings");
        pass.settings = passSettings;
        pass.renderPassEvent = passSettings.renderPassEvent;
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var cameraColorTargetIdent = renderer.cameraColorTarget;
        pass.Setup(cameraColorTargetIdent);
        renderer.EnqueuePass(pass);
    }
}

