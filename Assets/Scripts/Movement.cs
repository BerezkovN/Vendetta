using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 4;

    [System.NonSerialized] public Vector3 directionVector = new Vector3(1, 0, 0);
    [System.NonSerialized] public bool isOnLadder = false;

    protected bool IsRunning
    {
        get => _animator.GetBool(nameof(IsRunning));
        set => _animator.SetBool(nameof(IsRunning), value);
    }

    protected bool IsStanding
    {
        get => _animator.GetBool(nameof(IsStanding));
        set => _animator.SetBool(nameof(IsStanding), value);
    }

    private Animator _animator;

    private float _lastMovementDirection;


    public enum States
    {
        Stand,
        Sit,
        Run,
        NotRun,
        Walk,
        Climb,
        NotClimb,
        Die,
        StealthAttack,
        Attack
    }

    public void CallState(States state)
    {
        if (state == States.Stand)
        {
            IsStanding = true;
        }
        else if (state == States.Sit)
        {
            IsStanding = false;
        }
        else if (state == States.Run)
        {
            IsRunning = true;
        }
        else if (state == States.NotRun)
        {
            IsRunning = false;
        }
        else if (state == States.Climb)
        {
            _animator.Play("Climb");
            _animator.speed = 1f;
        }
        else if (state == States.NotClimb)
        {
            _animator.speed = 0f;
        }
        else if (state == States.StealthAttack)
        {
            _animator.Play("StealthAttack");
        }
        else if (state == States.Die)
        {
            _animator.Play("Death");
        }
    }

    public void PutOnLadder(Vector3 point)
    {
        gameObject.transform.position = new Vector3(point.x, point.y, gameObject.transform.position.z);
        CallState(States.Climb);
    }

    public void PutOffLadder()
    {

    }

    public void Climb(float verticalDirection)
    {
        transform.Translate(directionVector.x * verticalDirection * _movementSpeed * Time.deltaTime, directionVector.y * verticalDirection * _movementSpeed * Time.deltaTime, 0);

        if (verticalDirection != 0)
            CallState(States.Climb);
        else
            CallState(States.NotClimb);
    }

    public void Move(float horizontalDirection)
    {
        if (_lastMovementDirection != horizontalDirection)
        {
            if (horizontalDirection != 0)
                transform.localScale = new Vector3(horizontalDirection, transform.localScale.y, transform.localScale.x);
        }

        _lastMovementDirection = horizontalDirection;

        horizontalDirection *= (IsStanding ? 1 : 0.5f);

        transform.Translate(directionVector.x * horizontalDirection * _movementSpeed * Time.deltaTime, directionVector.y * horizontalDirection * _movementSpeed * Time.deltaTime, 0);

        if (horizontalDirection != 0)
            CallState(States.Run);
        else
            CallState(States.NotRun);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
}
