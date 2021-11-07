using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Firefly : MonoBehaviour
{
    [SerializeField] float Velocity;
    [SerializeField] float MaxDistance;
    [SerializeField] float DistanceDiff;

    public float BaseRadius { get; set; } = 0.25f;
    private float deltaTime
    {

        get
        {
            return (Time.realtimeSinceStartup - _timeBegin) * Velocity;
        }
    }


    private Light2D _light;
    private float _timeBegin;
    private List<Vector3> _bezierCurvePoints;
    private List<float> _bezierLUT;
    private const int LUTSize = 100;
    private float _seed;

    private void Start()
    {
        _seed = Random.Range(0.0f, 0.25f);
        _bezierLUT = new List<float>(new float[LUTSize]);
        _light = gameObject.GetComponent<Light2D>();
        _timeBegin = Time.realtimeSinceStartup + _seed;

        _bezierCurvePoints = new List<Vector3>();
        _bezierCurvePoints.Add(new Vector3(0, 0, 0));
        _bezierCurvePoints.Add(new Vector3(0, 0, 0));
        _bezierCurvePoints.Add(new Vector3(0, 0, 0));
        _bezierCurvePoints.Add(new Vector3(0, 0, 0));
        GenerateBezierCurve();
    }

    private void GenerateBezierCurve()
    {
        _bezierCurvePoints[0] = gameObject.transform.localPosition;
        float angle1 = Random.Range(0.0f, 1000.0f);
        float buf1 = Random.Range(Random.Range(0, MaxDistance),3 * MaxDistance);
        _bezierCurvePoints[1] = new Vector3(Mathf.Cos(angle1) * buf1, Mathf.Sin(angle1) * buf1);
        float angle2 = Random.Range(0.0f, 1000.0f);
        float buf2 = Random.Range(Random.Range(0, MaxDistance), 3 * MaxDistance);
        _bezierCurvePoints[2] = new Vector3(Mathf.Cos(angle2) * buf2, Mathf.Sin(angle2) * buf2);
        float angle = Random.Range(0.0f, 1000.0f);
        float dist = MaxDistance - Random.Range(0, DistanceDiff);
        _bezierCurvePoints[3] = new Vector3(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist);

        Vector3 buf = CalculatePosition(0);
        _bezierLUT[0] = 0;
        for (int i = 1; i < LUTSize; i++)
        {
            Vector3 secondBuf = BezierCurve((float)i / LUTSize);
            _bezierLUT[i] = _bezierLUT[i - 1] + (secondBuf - buf).magnitude;
            buf = secondBuf;
        }
    }

    private Vector3 BezierCurve(float t)
    {
        return new Vector3(
            _bezierCurvePoints[0].x * Mathf.Pow((1 - t), 3) +
            _bezierCurvePoints[1].x * 3 * t * Mathf.Pow((1 - t), 2) +
            _bezierCurvePoints[2].x * 3 * Mathf.Pow(t, 2) * (1 - t) +
            _bezierCurvePoints[3].x * Mathf.Pow(t, 3),

            _bezierCurvePoints[0].y * Mathf.Pow((1 - t), 3) +
            _bezierCurvePoints[1].y * 3 * t * Mathf.Pow((1 - t), 2) +
            _bezierCurvePoints[2].y * 3 * Mathf.Pow(t, 2) * (1 - t) +
            _bezierCurvePoints[3].y * Mathf.Pow(t, 3),

            0
        );
    }

    private Vector3 CalculatePosition(float t)
    {
        t = Mathf.Clamp(t, 0, 1);
        float buf = t * _bezierLUT[LUTSize - 1];
        for (int i = 0; i < LUTSize - 1; i++)
        {
            if (_bezierLUT[i] < buf && buf < _bezierLUT[i + 1])
            {
                float dist = _bezierLUT[i + 1] - _bezierLUT[i];
                t = (float)i / LUTSize + Mathf.Lerp(0, 1, (buf - _bezierLUT[i])/dist) / 100;
                break;
            }
        }
        return BezierCurve(t);
    }

    private void Update()
    {
        if (deltaTime > 1)
        {
            GenerateBezierCurve();
            _timeBegin = Time.realtimeSinceStartup;
        }
        gameObject.transform.localPosition = CalculatePosition(deltaTime);
        _light.pointLightOuterRadius = BaseRadius + Mathf.Sin(_seed + Time.realtimeSinceStartup / 5) / 10;
    }
}