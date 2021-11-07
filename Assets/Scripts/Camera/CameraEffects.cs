using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] SpriteRenderer _canvasBlackSquare;
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    public void SetExitFade(float t)
    {
        _canvasBlackSquare.color = new Color(0, 0, 0, t);
    }
}
