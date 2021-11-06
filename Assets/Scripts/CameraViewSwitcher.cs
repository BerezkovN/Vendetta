using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewSwitcher : MonoBehaviour
{
    [SerializeField] private CameraView _cameraView;

    private Transform _currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == _cameraView.Target)
        {
            _currentTarget = _cameraView.Target;
            _cameraView.Target = this.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform == _currentTarget)
        {
            _cameraView.Target = _currentTarget;
            _currentTarget = null;
        }
    }
}
