using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Firefly : MonoBehaviour
{
    [SerializeField] float Velocity;
    [SerializeField] float MaxDistance;
    [SerializeField] float DistanceDiff;

    public float BaseRadius { get; set; } = 0.15f;
    private float deltaTime
    {

        get
        {
            return (_seed + Time.realtimeSinceStartup - _timeBegin) * Velocity;
        }
    }


    private Light2D _light;
    private float _timeBegin;
    private List<Vector3> _bezierCurvePoints;
    private List<float> _bezierLUT;
    private float _seed;
    private const int LUTSize = 100;

    private void Start()
    {
        _seed = Random.Range(0, 10);
        _bezierLUT = new List<float>(new float[LUTSize]);
        _light = gameObject.GetComponent<Light2D>();

        _bezierCurvePoints = new List<Vector3>();
        _bezierCurvePoints.Add(new Vector3(0, 0, 0));
        _bezierCurvePoints.Add(new Vector3(0, 0, 0));
        _bezierCurvePoints.Add(new Vector3(0, 0, 0));
        _bezierCurvePoints.Add(new Vector3(0, 0, 0));
        GenerateBezierCurve();
    }

    private void GenerateBezierCurve()
    {
        float angle = Random.Range(0, 100);
        float dist = MaxDistance - Random.Range(0, DistanceDiff);
        _bezierCurvePoints[0] = gameObject.transform.position;
        float buf1 = Random.Range(0.25f, MaxDistance);
        _bezierCurvePoints[1] = new Vector3(Mathf.Cos(angle) * buf1, Mathf.Sin(angle) * buf1);
        float buf2 = Random.Range(0.25f, MaxDistance);
        _bezierCurvePoints[2] = new Vector3(Mathf.Cos(angle) * buf2, Mathf.Sin(angle) * buf2);
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
        float buf = _bezierLUT[LUTSize - 1];
        for (int i = 0; i < LUTSize - 1; i++)
        {
            if (_bezierLUT[i] < buf && buf < _bezierLUT[i + 1])
            {
                float dist = _bezierLUT[i + 1] - _bezierLUT[i];
                t = (float)i / LUTSize + Mathf.Lerp(0, dist, buf - _bezierLUT[i]);
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
            _timeBegin = _seed + Time.realtimeSinceStartup;
        }
        gameObject.transform.position = CalculatePosition(deltaTime);
        _light.pointLightOuterRadius = BaseRadius + Mathf.Sin(_seed + Time.realtimeSinceStartup / 5) / 10;
    }
}