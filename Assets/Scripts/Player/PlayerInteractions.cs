using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private Interactable? _currentTarget = null;
    private PlayerInfo _playerInfo;

    private bool _pressed = false;

    public bool Dead { get => _playerInfo.Health <= 0; }
    public bool Alive { get => _playerInfo.Health > 0; }
    private void Start()
    {
        _playerInfo = GetComponent<PlayerInfo>();
    }
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
    }
}
