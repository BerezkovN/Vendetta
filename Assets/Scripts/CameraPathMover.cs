using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraPathMover : MonoBehaviour
{
    [SerializeField] private CameraView _cameraView;
    [SerializeField] private float _beginDelay;
    [SerializeField] private TransformDelay[] _points;

    private Coroutine _currentAction;

    public UnityEvent Finished;

    public void TryActivate()
    {
        if (_currentAction == null)
            _currentAction = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        if (_beginDelay != 0)
            yield return new WaitForSeconds(_beginDelay);

        foreach (var pt in _points)
        {
            _cameraView.Target = pt.Transform;
            yield return new WaitForSeconds(pt.Delay);
        }

        Finished?.Invoke();
    }
}

[Serializable]
public struct TransformDelay
{
    [SerializeField] public float Delay;
    [SerializeField] public Transform Transform;

    public override string ToString()
    {
        return $"{Transform} with {Delay}sec delay.";
    }
}
