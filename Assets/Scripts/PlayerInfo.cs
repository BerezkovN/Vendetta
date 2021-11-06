using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private int _maxHelth = 3;

    private int _health;

    public UnityEvent<int> HealthChanged;

    public int MaxHelth => _maxHelth;

    public int Health
    {
        get => _health;
        protected set
        {
            if (_health != value)
            {
                _health = value;
                HealthChanged?.Invoke(value);
            }
        }
    }

    private void Start()
    {
        _health = _maxHelth;
    }

    public void ApplyDamage(int dmg)
    {
        if (dmg <= 0)
            throw new ArgumentOutOfRangeException(nameof(dmg));

        Health = Mathf.Max(Health - dmg, 0);
    }

    public void ApplyHeal(int heal)
    {
        if (heal <= 0)
            throw new ArgumentOutOfRangeException(nameof(heal));

        Health = Mathf.Min(Health + heal, MaxHelth);
    }
}
