using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Fire :
    Interactable
{
    private SpriteRenderer _spriteRenderer;
    private Light2D _light;
    private void FixedUpdate()
    {
        if (_light.enabled)
        {
            var t = Time.realtimeSinceStartup;
            var sint = Mathf.Sin(t);
            var sintcost = sint * Mathf.Cos(8 * t);
            _light.intensity = 0.75f + (sintcost * sintcost * sintcost + 2.0f * sint) / 12;
        }
    }
    protected override void InitializeInternal()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _light = GetComponent<Light2D>();
        _light.enabled = false;
        _spriteRenderer.enabled = false;
    }
    protected override void OnClickInternal(PlayerInfo playerInfo)
    {
        HideInteractionKey();
        _light.enabled = true;
        _spriteRenderer.enabled = true;
    }

}