using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraView : MonoBehaviour
{
    [SerializeField] private float _delay = 0.15f;
    public Transform _target2;
    [SerializeField] private float _shakePower = 0.16f;
    [SerializeField] private float _verticalShakePower = 0.16f;
    [SerializeField] private float _verticalShakeSpeed = 8f;
    private Vector3 velocity2 = Vector3.zero;

    private Vector3 _originalPos;
    private float _shakeTimeoutTimestamp;
    private float _smoothTimeoutTimestamp;



    [SerializeField] private float _dampTime = 0.3f;
    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;
    // from 0% to 50%, (50% means that border line should have size 50% from screen)
    [SerializeField, Range(0, 0.5f)] private float _borderSize = 0.05f;
    [SerializeField] private bool _canExpandBorders = false;
    [SerializeField] private Transform _target;

    private Camera _camera;

    private Vector3 velocity = Vector3.zero;

    public Transform Target
    {
        get => _target;
        set => _target = value;
    }

    public bool CanExpandBorders
    {
        get => _canExpandBorders;
        set => _canExpandBorders = value;
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }
    private void Awaker()
    {
        _originalPos = transform.position;
    }

    private void LateUpdate()
    {
        float edgeOffsetX = 0;
        float edgeOffsetY = 0;
        Vector3 buf = transform.position;
        if (_canExpandBorders)
        {
            float borderX = _borderSize * Screen.width;
            float borderY = _borderSize * Screen.height;

            Vector3 mousePos = Input.mousePosition;

            if (mousePos.x <= borderX || mousePos.x >= Screen.width - borderX)
                edgeOffsetX = Screen.width / 2 > mousePos.x ? -1 : 1;
            if (mousePos.y <= borderY || mousePos.y >= Screen.height - borderY)
                edgeOffsetY = Screen.height / 2 > mousePos.y ? -1 : 1;
        }

        if (_target2)
        {
            Vector3 point = _camera.WorldToViewportPoint(_target.position);
            Vector3 delta = _target.position - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = _originalPos + delta;
            if (Time.time <= _shakeTimeoutTimestamp)
            {
                _originalPos = Vector3.SmoothDamp(_originalPos, destination, ref velocity2, _delay);
                Vector3 shakeDestination = _originalPos + Random.insideUnitSphere * _shakePower;
                shakeDestination.z = _originalPos.z;

                transform.position = shakeDestination;
            }
            else if (Time.time <= _smoothTimeoutTimestamp)
            {
                _originalPos = Vector3.SmoothDamp(_originalPos, destination, ref velocity2, _delay);
                Vector3 shakeDestination = _originalPos + new Vector3(0, Mathf.Sin(Time.realtimeSinceStartup * _verticalShakeSpeed)) * _verticalShakePower;
                shakeDestination.z = _originalPos.z;

                transform.position = shakeDestination;
            }
        }
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
        transform.position = new Vector3(transform.position.x, transform.position.y, buf.z);
    }
    /// <summary>
    /// Shakes camera during specified duration.
    /// </summary>
    /// <param name="duration">Duration in seconds.</param>
    public void ShakeCamera(float duration)
    {
        _shakeTimeoutTimestamp = Time.time + duration;
    }

    /// <summary>
    /// Shakes camera during specified duration with specified power.
    /// </summary>
    /// <param name="duration">Duration in seconds.</param>
    /// <param name="shakeAmount">Shaked power.</param>
    public void ShakeCameraRoughly(float duration, float shakePower)
    {
        _shakePower = shakePower;
        _shakeTimeoutTimestamp = Time.time + duration;
    }

    public void ShakeCameraSmoothly(float duration)
    {
        _smoothTimeoutTimestamp = Time.time + duration;
    }
#if DEBUG
    [ContextMenu("Shake For 5sec")]
    private void TestShake()
    {
        if (Application.isPlaying)
            ShakeCamera(5);
    }
#endif
}
