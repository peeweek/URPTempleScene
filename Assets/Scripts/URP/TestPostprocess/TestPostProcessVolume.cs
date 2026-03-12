using UnityEngine;
using UnityEngine.Rendering;

public class TestPostProcessVolume : VolumeComponent, IPostProcessComponent
{
    public ColorParameter fogColor = new ColorParameter(new Color(0.1f, 0.15f, 0.2f, 0.8f));
    public ClampedFloatParameter fogDistance = new ClampedFloatParameter(12f, 0.05f, 50f);

    public bool IsActive()
    {
        return fogColor.value.a > 0.0f;
    }
}
