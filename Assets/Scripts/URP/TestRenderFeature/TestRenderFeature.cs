using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class TestRenderFeature : ScriptableRendererFeature
{
    TestRenderPass _pass;

    public override void Create()
    {
        if ( _pass == null ) _pass = new TestRenderPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_pass);
    }

    class TestRenderPass : ScriptableRenderPass
    {
        Material material;
        public TestRenderPass()
        {
            this.material = new Material(Shader.Find("Hidden/FullScreenBlit"));
            this.renderPassEvent = RenderPassEvent.BeforeRenderingTransparents; // Desaturate Opaques Only
        }
                public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (this.material == null) this.material = new Material(Shader.Find("Hidden/FullScreenBlit"));

            var resourceData = frameData.Get<UniversalResourceData>();
            var blitParameters = new RenderGraphUtils.BlitMaterialParameters(resourceData.activeColorTexture, resourceData.activeColorTexture, material, 0);
            renderGraph.AddBlitPass(blitParameters);
        }
    }
}





