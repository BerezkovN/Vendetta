using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private CameraEffects _cameraEffects;
 
    private Interactable? _currentTarget = null;
    private PlayerInfo _playerInfo;

    private bool _pressed = false;

    public bool Dead { get => _playerInfo.Health <= 0; }
    public bool Alive { get => _playerInfo.Health > 0; }
    private void Start()
    {
        _playerInfo = GetComponent<PlayerInfo>();
        if(_cameraEffects == null)
        {
            throw new System.ArgumentNullException("_cameraEffects cannot be null!");
        }
    }
    float _escPressedEnd = 0;
    void Update()
    {
        if (_currentTarget != null)
        {
            if (Input.GetButtonDown(_currentTarget.Key.KeyName))
            {
                _pressed = true;
                _currentTarget.OnClick(_playerInfo);
            }
            else if (_pressed && !Input.GetButton(_currentTarget.Key.KeyName))
            { 
                _pressed = false;
                _currentTarget.OnClickExit(_playerInfo);
            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            _escPressedEnd = Time.realtimeSinceStartup + 3;
        } 
        else if(Input.GetButton("Cancel"))
        {
            _cameraEffects.SetExitFade(1 - (_escPressedEnd - Time.realtimeSinceStartup) / 3);
            if(_escPressedEnd < Time.realtimeSinceStartup)
            {
                Application.Quit();
            }
        } else
        {
            _escPressedEnd = 0;
            _cameraEffects.SetExitFade(0);
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

    public void TakeDamage(int damage)
    {
        _playerInfo.ApplyDamage(damage);
        _cameraEffects.OnHit();
    }
}
