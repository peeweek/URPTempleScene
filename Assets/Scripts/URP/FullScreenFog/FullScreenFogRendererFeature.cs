using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class FullScreenFogRendererFeature : ScriptableRendererFeature
{
    [SerializeField]
    Settings settings = new Settings();

    FullScreenFogRenderPass _fullScreenFogRenderPass;

    public override void Create()
    {
        if(_fullScreenFogRenderPass == null)
            _fullScreenFogRenderPass = new FullScreenFogRenderPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_fullScreenFogRenderPass);
    }

    /// <summary>
    /// Settings that will be displayed in inspector, need to be serializable, and instance field serialized.
    /// </summary>
    [System.Serializable]
    public class Settings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
        public Shader shader;
    }

    /// <summary>
    /// The actual rendering pass, does only a full screen rendering at a given point of rendering.
    /// Will read values from the volume stack, and set uniforms.
    /// </summary>
    class FullScreenFogRenderPass : ScriptableRenderPass
    {
        Settings settings;
        Material material;

        public FullScreenFogRenderPass(Settings settings)
        {
            this.settings = settings;
            this.renderPassEvent = settings.renderPassEvent;
            this.material = new Material(settings.shader);

        }

        /// <summary>
        /// Rendering Entrypoint. We will add commands for the rendergraph here.
        /// </summary>
        /// <param name="renderGraph"></param>
        /// <param name="frameData"></param>
        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            //base.RecordRenderGraph(renderGraph, frameData);

            // First, we poll the values from the active camera stack.
            // In order to do that, we poll from the VolumeManager using
            FullScreenFog fog = VolumeManager.instance.stack.GetComponent<FullScreenFog>();

            // then we set uniforms to the material
            material.SetColor("_FullScreenFogColor", fog.FogColor.value);
            material.SetFloat("_FullScreenFogDensity", fog.Density.value);
            material.SetFloat("_FullScreenFogKnee", fog.FogKnee.value);
            material.SetFloat("_FullScreenFogHeight", fog.HeightOffset.value);
            
            // We need to get a reference to the Color buffer, it's inside the UniversalResourceData 
            // structure, that we can get with frameData.Get<>()
            var resourceData = frameData.Get<UniversalResourceData>();
            var blitFogParameters = new RenderGraphUtils.BlitMaterialParameters(TextureHandle.nullHandle, resourceData.activeColorTexture, material, 0);
            renderGraph.AddBlitPass(blitFogParameters);         


        }
    }
}
