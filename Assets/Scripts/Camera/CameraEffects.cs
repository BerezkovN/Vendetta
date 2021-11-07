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
    private readonly Queue<KeyValuePair<Color, float>> colorQueue = new Queue<KeyValuePair<Color, float>>();
    private Color _baseLightColor;
    private Color? _currentColor = null;
    private float _colorEnd;
    private CameraView _view;
    private Color? CurrentColor 
    { 
        get => _currentColor; 
        set 
        { 
            _currentColor = value;  
            if (value == null) { _bloom.tint.value = _baseLightColor; _bloom.threshold.value = 1; } 
            else { _bloom.tint.value = value.Value; _bloom.threshold.value = 0.5f; } 
        } 
    }
    private void Start()
    {
        if(_light == null || _postProcessing == null)
        {
            throw new System.ArgumentNullException($"neither {nameof(_light)} nor {nameof(_postProcessing)} shouldn't be null");
        }
        _postProcessing.profile.TryGet(out _bloom);
        _postProcessing.profile.TryGet(out _colorAdjustments);
        _postProcessing.profile.TryGet(out _vignette);
        _baseLightColor = _bloom.tint.value;
        _view = GetComponent<CameraView>();
    }

    private void Update()
    {
        if(_colorEnd < Time.realtimeSinceStartup)
        {
            _colorEnd = 0;
            if(colorQueue.Count > 0)
            {
                _colorEnd = Time.realtimeSinceStartup;
                KeyValuePair<Color, float> pair = colorQueue.Dequeue();
                _vignette.intensity.value = 0.5f;
                CurrentColor = pair.Key;
                _colorEnd = pair.Value;
            } 
            else if(CurrentColor != null)
            {
                CurrentColor = null;
                _vignette.intensity.value = 0;
            }
        }
    }
    public void SetExitFade(float t)
    {
        _colorAdjustments.postExposure.value = 5 * Mathf.Log10(1 + t) + 1 / (1 + t) - 1 - 4 * t * t;
        _vignette.intensity.value = t;
    }
    public void OnTakingDamage()
    {
        colorQueue.Enqueue(new KeyValuePair<Color, float>(Color.red, Time.realtimeSinceStartup + 0.125f));
        _view.ShakeCameraRoughly(0.1f, 2);
    }
    public void OnSilentKill()
    {

    }
    public void OnHit()
    {

    }
}
