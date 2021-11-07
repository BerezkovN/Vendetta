using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;


public class CameraEffects : MonoBehaviour
{
    [SerializeField] private Light2D _light;
    [SerializeField] private Volume _postProcessing;

    private Bloom _bloom;
    private ColorAdjustments _colorAdjustments;
    private Vignette _vignette;

    private void Start()
    {
        if(_light == null || _postProcessing == null)
        {
            throw new System.ArgumentNullException($"neither {nameof(_light)} nor {nameof(_postProcessing)} shouldn't be null");
        }
        _postProcessing.profile.TryGet(out _bloom);
        _postProcessing.profile.TryGet(out _colorAdjustments);
        _postProcessing.profile.TryGet(out _vignette);
    }

    private void Update()
    {
        
    }
    public void SetExitFade(float t)
    {
        _colorAdjustments.postExposure.value = 5*Mathf.Log10(1+t) + 1/(1+t) - 1 - 4 * t  * t;
        _vignette.intensity.value = t;
    }
}
