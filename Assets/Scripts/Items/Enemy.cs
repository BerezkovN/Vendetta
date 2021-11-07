using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Interactable
{
    // Amount of time the enemy will search for the player
    [SerializeField] private int _attentionSpan = 30;
    [SerializeField] private int _spotRadius = 4;
    [SerializeField] private int _enemyCommunicationRadius = 6;
    [SerializeField] private int _baseHealth = 100;
    [SerializeField] private int _baseSpeed = 5;
    [SerializeField] private int _fleeSpeed = 5;
    [SerializeField] private int _damage = 30;
    [SerializeField] GameObject PointLeft;
    [SerializeField] GameObject PointRight;


    // Internal state of the object, if the player was spotted then we should 
    private bool _alerted;
    private bool _fighting;
    private bool _fleeing;
    private bool _moving = true;
    // random value, if false then the enemy will try to flee away
    private bool _brave = false;
    private float _health;
    private float _currentSpeed;
    // false for left, true for right
    private bool _lastMovementDirection;
    // Direction in which the enemy is looking to
    // true for right, false for left

    public bool Dead { get; private set; }
    public float Health { get => _health; private set { _health = value; if (_health <= 0) { Dead = true; } } }
    public float Speed { get { return _fleeing ? _fleeSpeed : _baseSpeed; } }
    private GameObject Target { get; set; }

    private void Start()
    {
        Health = _baseHealth;
        if(PointLeft == null || PointRight == null)
        {
            throw new System.ArgumentNullException("Neither pointLeft nor pointRight should be null");
        }
        if(Random.Range(0, 100) < 40)
        {
            _brave = true;
        }
        if(Random.Range(0,2)  < 1)
        {
            Target = PointLeft;
        } else
        {
            Target = PointRight;
        }
    }
    private void Update()
    {
        if(Dead)
        {
            return;
        }
        if (!_brave && _health < 30)
        {
            _fleeing = true;
        }
        if(_fleeing)
        {
            Flee();
        } 
        else if (_fighting)
        {
            Fight();
        } 
        else
        {
            Patrol();
        }
        if (_moving || _fleeing)
        {
            Move();
        }
    }
    private void FixedUpdate()
    {
        foreach(Collider2D collider in Physics2D.OverlapCircleAll(transform.position, _enemyCommunicationRadius))
        {
            if((collider.gameObject.transform.position.x > gameObject.transform.position.x && 
                Target.transform.position.x > gameObject.transform.position.x) ||
                (collider.gameObject.transform.position.x < gameObject.transform.position.x && 
                Target.transform.position.x < gameObject.transform.position.x))
            {
                continue; // The object is not within the area of the enemy view 
            }
            if (collider.gameObject.TryGetComponent(out Enemy mate))
            {
                Communicate(mate);
            } 
            else if (collider.gameObject.TryGetComponent(out PlayerInteractions player) && 
                Vector2.Distance(player.transform.position, gameObject.transform.position) < _spotRadius)
            {
                ProceedPlayer(player);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_alerted && collision.gameObject.TryGetComponent<Enemy>(out Enemy mate))
        {
            mate._alerted = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        
    }

    protected override void OnClickInternal(PlayerInfo playerInfo)
    {
        Health -= playerInfo.Damage;
    }

    protected override void InitializeInternal()
    {

    }


    private void Death()
    {
        Dead = true;
        HideInteractionKey();
        // TODO
        // death animation
    }
    private void Fight()
    {
        // TODO
        // animation of fighting the player.
        // fighting logic
    }
    private void Flee()
    {
        // flee logic
    }
    private void Patrol()
    {
        if (PointLeft.transform.position.x > transform.position.x)
        {
            Target = PointRight;
        } 
        else if (PointRight.transform.position.x < transform.position.x)
        {
            Target = PointLeft;
        }
    }
    private void Move()
    {
        bool direction = Target.transform.position.x > transform.position.x;
        if(direction != _lastMovementDirection)
        {
            _lastMovementDirection = direction;
            transform.localScale = new Vector3(direction ? -1 : 1, transform.localScale.y, transform.localScale.x);
            // mirror the key
            CreatedKey.transform.localScale = new Vector3(direction ? 1 : -1, transform.localScale.y, transform.localScale.x);
        }
        Vector3 vec = direction ? Vector2.right : Vector2.left;
        vec = vec * Speed * Time.deltaTime;
        transform.Translate(vec);
        // TODO
        // move || run animation ?
    }


    private void Communicate(Enemy teammate)
    {
        if(_alerted || teammate.Dead)
        {
            _alerted = teammate._alerted = true;
        }

    }
    // if player was seen by enemy
    private void ProceedPlayer(PlayerInteractions player)
    {
        if(player.Alive)
        {
            Target = player.gameObject;
            _fighting = true;
        }
    }
}
