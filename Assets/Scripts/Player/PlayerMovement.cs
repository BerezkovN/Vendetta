using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 4;

    [System.NonSerialized] public Vector3 directionVector = new Vector3(1, 0, 0);

    private Animator _animator;

    private float _lastMovementDirection;

    protected bool IsRunning
    {
        get => _animator.GetBool(nameof(IsRunning));
        set => _animator.SetBool(nameof(IsRunning), value);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float direction = Input.GetAxisRaw(Constants.HorizontalAxis);

        if (_lastMovementDirection != direction)
        {
            if (direction != 0)
                transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.x);

            IsRunning = direction != 0;
        }

        _lastMovementDirection = direction;



        transform.Translate(directionVector.x * direction * _movementSpeed * Time.deltaTime, directionVector.y * direction * _movementSpeed * Time.deltaTime, 0);
    }
}
