using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class TestPostProcessRenderFeature : ScriptableRendererFeature
{
    [System.Serializable] 
    class Settings 
    {
        public RenderPassEvent PassEvent = RenderPassEvent.BeforeRenderingTransparents;
        public Shader Shader;
    }


    [SerializeField]
    Settings settings;

    RenderPass _renderPass;

    public override void Create()
    {
        _renderPass = new RenderPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
       renderer.EnqueuePass(this._renderPass);
    }

    class RenderPass : ScriptableRenderPass
    {
        Material _material;
        Settings _settings;

        public RenderPass(Settings settings)
        {
            this._settings = settings;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            // Lazy : If we have a null material (or shader changed), we re-create.
            if (this._material == null || this._material.shader != _settings.Shader)
            {
                if (this._settings.Shader == null)
                    return;

                this._material = new Material(_settings.Shader);
            }

            // Injection Point, can be changed across time.
            this.renderPassEvent = _settings.PassEvent;

            // Query the volume manager to filter values for the TestPostProcessVolume
            var ppv = VolumeManager.instance.stack.GetComponent<TestPostProcessVolume>();
            _material.SetColor("_FogColor", ppv.fogColor.value);
            _material.SetFloat("_FogDistance", ppv.fogDistance.value);


            // Prepare for a FullScreen Blit
            var resourceData = frameData.Get<UniversalResourceData>();
            var blitMaterialParameters = new RenderGraphUtils.BlitMaterialParameters(
                TextureHandle.nullHandle,
                resourceData.activeColorTexture,
                this._material, 0
                );
            renderGraph.AddBlitPass(blitMaterialParameters);
        }
    }

}
