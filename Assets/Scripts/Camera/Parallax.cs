using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Parallax : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _parallaxFactor;

    private float _length;
    private float _startpos;


    private void Start()
    {
        _startpos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float temp = (_camera.transform.position.x * (1-_parallaxFactor));
        float dist = (_camera.transform.position.x * _parallaxFactor);

        Vector3 newPosition = new Vector3(_startpos + dist, transform.position.y, transform.position.z);

        transform.position = newPosition;

        if (temp > _startpos + (_length / 2)) 
            _startpos += _length;
        else if (temp < _startpos - (_length / 2))
            _startpos -= _length;
    }
}
