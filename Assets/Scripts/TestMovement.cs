using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class TestMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 8;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private float _lastPos;

    private int _wallLayerMask;

    public bool IsRunning
    {
        get => _animator.GetBool(nameof(IsRunning));
        set => _animator.SetBool(nameof(IsRunning), value);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
    }

    private void Update()
    {
        float direction = Input.GetAxisRaw(Constants.HorizontalAxis);

        if (direction != 0)
        {
            Vector2 pos = transform.position;
            pos += new Vector2(direction * 0.05f, 0);
            Collider2D[] hits = Physics2D.OverlapBoxAll(pos, _collider.bounds.size, 0, _wallLayerMask);

            if (hits.Any())
            {
                Debug.Log($"Count: {hits.Length} - " + string.Join(", ", hits.Select(t => t.name)));
                direction = 0;
            }
        }

        if (_lastPos != direction)
        {
            if (direction != 0)
                transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);

            IsRunning = direction != 0;
        }

        _lastPos = direction;

        _rigidbody.velocity = new Vector2(direction * _movementSpeed, _rigidbody.velocity.y);
    }

    private void FixedUpdate()
    {
        
    }
}
