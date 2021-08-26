using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GraphicsSettings : MonoBehaviour
{
    public UniversalRenderPipelineAsset asset;
    private UniversalAdditionalCameraData camData;

    void Start()
    {
        camData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
    }

    public void SetAntialiasing_Off()
    {
        asset.msaaSampleCount = 1;
        //camData.antialiasing = AntialiasingMode.None;
    }

    public void SetAntialiasing_Low()
    {
        asset.msaaSampleCount = 2;
        //camData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
        //camData.antialiasingQuality = AntialiasingQuality.Low;
    }

    public void SetAntialiasing_Medium()
    {
        asset.msaaSampleCount = 4;
        //camData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
        //camData.antialiasingQuality = AntialiasingQuality.Medium;
    }

    public void SetAntialiasing_High()
    {
        asset.msaaSampleCount = 8;
        //camData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
        //camData.antialiasingQuality = AntialiasingQuality.High;
    }
}
