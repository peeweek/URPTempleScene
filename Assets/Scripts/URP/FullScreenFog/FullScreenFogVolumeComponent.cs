using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Rendering/Full Screen Fog")]
[VolumeRequiresRendererFeatures(typeof(FullScreenFogRendererFeature))]
public class FullScreenFog : VolumeComponent, IPostProcessComponent
{
    public override string ToString()
    {
        return "Full Screen Fog";
    }

    public bool IsActive()
    {
        return Density.value > 0f;
    }

    public ColorParameter FogColor = new ColorParameter(new Color(0.1f,0.1f,0.12f,1.0f));
    public FloatParameter Density = new FloatParameter(3f);
    public FloatParameter FogKnee = new FloatParameter(3f);
    public FloatParameter HeightOffset = new FloatParameter(2f);
}
