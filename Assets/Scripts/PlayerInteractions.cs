using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private Interactable? _currentTarget = null;

    private bool _pressed = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (_currentTarget != null)
        {
            if (Input.GetButtonDown(_currentTarget.Key.KeyName))
            {
                _pressed = true;
                _currentTarget.OnClick();
            }
            else if (_pressed && !Input.GetButton(_currentTarget.Key.KeyName))
            { 
                _pressed = false;
                _currentTarget.OnClickExit();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Interactable>(out Interactable obj) && 
            !obj.IsHidden())
        {
            if (_currentTarget != null)
            {
                _pressed = false;
                _currentTarget.TriggerExit(collision);
            }
            _currentTarget = obj;
            _currentTarget.TriggerEnter(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Interactable>(out Interactable obj) && 
            _currentTarget != null && obj == _currentTarget)
        {
            _pressed = false;
            _currentTarget.TriggerExit(collision);
            _currentTarget = null;
        }
    }

}
