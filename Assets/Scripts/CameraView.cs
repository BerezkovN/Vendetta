using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraView : MonoBehaviour
{
    [SerializeField] private float _dampTime = 0.15f;
    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;
    // from 0% to 50%, (50% means that border line should have size 50% from screen)
    [SerializeField, Range(0, 0.5f)] private float _borderSize = 0.05f;
    [SerializeField] private Transform _target;

    private Camera _camera;

    private Vector3 velocity = Vector3.zero;

    public Transform Target
    {
        get => _target;
        set => _target = value;
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        float edgeOffsetX = 0;
        float edgeOffsetY = 0;

        float borderX = _borderSize * Screen.width;
        float borderY = _borderSize * Screen.height;
        
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x <= borderX || mousePos.x >= Screen.width - borderX)
            edgeOffsetX = Screen.width / 2 > mousePos.x ? -1 : 1;
        if (mousePos.y <= borderY || mousePos.y >= Screen.height - borderY)
            edgeOffsetY = Screen.height / 2 > mousePos.y ? -1 : 1;

        if (_target)
        {
            Vector3 point = _camera.WorldToViewportPoint(_target.position);
            Vector3 delta = _target.position - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            destination.x += _offsetX;
            destination.y += _offsetY;

            destination.x += edgeOffsetX;
            destination.y += edgeOffsetY;

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, _dampTime);
        }

    }
}
