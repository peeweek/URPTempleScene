using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Rendering/Full Screen Fog")]
[VolumeRequiresRendererFeatures(typeof(FullScreenFogRendererFeature))]
public class FullScreenFog : VolumeComponent, IPostProcessComponent
{
    public bool IsActive()
    {
        return Density.value > 0f;
    }

    public ColorParameter FogColor = new ColorParameter(new Color(0.1f,0.1f,0.12f,1.0f));
    public FloatParameter Density = new ClampedFloatParameter(3f, 0.1f, 10f);
    public FloatParameter FogKnee = new ClampedFloatParameter(3f, 1f, 10f);
    public FloatParameter HeightOffset = new FloatParameter(2f);
}
