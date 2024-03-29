using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Enemy : Interactable
{
    // Amount of time the enemy will search for the player
    [SerializeField] private float _reachDistance = 1;
    [SerializeField] private float _verticalReachDistance = 8;
    [SerializeField] private int _alertRange = 8;
    [SerializeField] private int _fightStart = 6;
    [SerializeField] private int _enemyCommunicationRadius = 6;
    [SerializeField] private int _baseHealth = 100;
    [SerializeField] private int _baseSpeed = 5;
    [SerializeField] private int _fleeSpeed = 7;
    [SerializeField] private int _damage = 30;
    [SerializeField] private GameObject _pointLeft;
    [SerializeField] private GameObject _pointRight;
    [SerializeField] private Light2D _light;
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _alertColor;
    [SerializeField] private Color _fightColor;


    // Internal state of the object, if the player was spotted then we should 
    private bool _fighting;
    private bool _fleeing;
    private bool _alerted;
    private bool _moving = true;
    // random value, if false then the enemy will try to flee away
    private bool _brave = false;
    private float _health;
    // false for left, true for right
    private bool _lastMovementDirection;
    // position & time
    private KeyValuePair<Vector3, float> _alertFocus = new KeyValuePair<Vector3, float>(Vector3.zero,0);

    // used for animations; true while animation is in charge right now
    private float _stunEnd;

    // Direction in which the enemy is looking to
    // true for right, false for left

    public bool Dead { get; private set; }
    public bool Alive { get => !Dead; }
    public float Health { get => _health; private set { _health = value; if (_health <= 0) { Dead = true; OnDeath(); } } }
    public float Speed { get { return _fleeing ? _fleeSpeed : _baseSpeed; } }
    public bool Alerted { get => _alerted; set { _alerted = value; UpdateColor(); } }
    public bool Fighting { get => _fighting; set { _fighting = value; UpdateColor(); } }

    private GameObject Target { get; set; }
    private Movement Animations { get; set; }
    // only set when player was seen
    private PlayerInteractions Player { get; set; }

    private void Start()
    {
        Health = _baseHealth;
        Alerted = false;
        if (_pointLeft == null || _pointRight == null || _light == null)
        {
            throw new System.ArgumentNullException("Neither pointLeft nor pointRight nor _light should be null");
        }
        if(Random.Range(0, 100) < 40)
        {
            _brave = true;
        }
        Animations = GetComponent<Movement>();
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
        else if (Fighting)
        {
            Fight();
        } 
        else
        {
            Patrol();
        }
    }
    private void FixedUpdate()
    {
        if(Dead)
        {
            return;
        }
        foreach(Collider2D collider in Physics2D.OverlapCircleAll(transform.position, _enemyCommunicationRadius))
        {
            if (collider.gameObject.TryGetComponent(out Enemy mate))
            {
                Communicate(mate);
            }
            if ((collider.gameObject.transform.position.x > gameObject.transform.position.x &&
                !_lastMovementDirection) ||
                (collider.gameObject.transform.position.x < gameObject.transform.position.x &&
                _lastMovementDirection))
            {
                continue; // The object is not within the area of the enemy view 
            }
            else if (collider.gameObject.TryGetComponent(out PlayerInteractions player))
            {
                ProceedPlayer(player, Vector2.Distance(player.transform.position, gameObject.transform.position));
            }
        }

    }

    protected override void OnClickInternal(PlayerInfo playerInfo)
    {
        if (!_fighting)
        {
            Health = -100;
            playerInfo.GetComponent<PlayerInteractions>().StealthAttack();
        }
        else
        {
            Health -= playerInfo.Damage;
            playerInfo.GetComponent<PlayerInteractions>().Attack();
        }
    }

    protected override void InitializeInternal()
    {
        _light.color = _baseColor;
    }


    private void UpdateColor()
    {
        if(Fighting)
        {
            _light.color = _fightColor;
        }
        else if (_alerted)
        {
            _light.color = _alertColor;
        }
        else
        {
            _light.color = _baseColor;
        }
    }

    private void OnDeath()
    {
        Dead = true;
        HideInteractionKey();
        _light.color = Color.clear;
        _light.enabled = false;
        Animations.CallState(Movement.States.Die);
    }
    private void Fight()
    {
        if(Player.Dead)
        {
            _fighting = false;
            _stunEnd = 0;
            _timeStandingEnd = 0;
            _alertFocusBuf = new KeyValuePair<Vector3, float>(Vector3.zero, 0);
            return;
        }
        if (_stunEnd < Time.realtimeSinceStartup)
        {
            if (Mathf.Abs(Player.transform.position.x - transform.position.x) > _reachDistance)
            {
                Target = Player.gameObject;
                _moving = true;
            }
            else
            {
                if (Mathf.Abs(Player.transform.position.x - transform.position.x) < _reachDistance &&
                    Mathf.Abs(Player.transform.position.y - transform.position.y) < _verticalReachDistance)
                {
                    Animations.CallState(Movement.States.Attack);
                    Player.TakeDamage(_damage);
                    _moving = false;
                    _stunEnd = Time.realtimeSinceStartup + 1.0f;
                } else
                {
                    _moving = false;
                    _stunEnd = Time.realtimeSinceStartup + 0.125f;
                }
            }
        }
        if (_moving)
        {
            Move();
        }
        else
        {
            Animations.CallState(Movement.States.NotRun);
        }
    }
    private void Flee()
    {
        if (Player.Alive)
        {
            Animations.CallState(Movement.States.Run);
        }
        else
        {
            Animations.CallState(Movement.States.Walk);
        }
        Move();
    }

    private KeyValuePair<Vector3, float> _alertFocusBuf = new KeyValuePair<Vector3, float>(Vector3.zero, 0);
    private float _timeStandingEnd = 0;
    
    private void Patrol()
    {
        if (_alerted && _alertFocusBuf.Key != _alertFocus.Key)
        {
            _pointLeft.transform.position = new Vector3(-10 + Random.Range(-2.5f, 7.5f), 0, 0) + _alertFocus.Key;
            _pointRight.transform.position = new Vector3(10 + Random.Range(-7.5f, 2.5f), 0, 0) + _alertFocus.Key;

            _alertFocusBuf = _alertFocus;
            _timeStandingEnd = Time.realtimeSinceStartup;
        }
        GameObject? target = null;
        if (_pointLeft.transform.position.x > transform.position.x)
        {
            target = _pointRight;
        } 
        else if (_pointRight.transform.position.x < transform.position.x)
        {
            target = _pointLeft;
        }
        else if(Target != _pointLeft && Target != _pointRight)
        {
            if (Random.Range(0, 2) < 1)
            {
                target = _pointLeft;
            }
            else
            {
                target = _pointRight;
            }
        }
        if (Target == null)
        {
            Target = target;
        } 
        else if (target != null)
        {
            if (_moving && Mathf.Abs(Target.transform.position.x - gameObject.transform.position.x) < 1.0f)
            {
                _timeStandingEnd = Time.realtimeSinceStartup + Random.Range(3,6);
                _moving = false;
            }
            else if (_timeStandingEnd < Time.realtimeSinceStartup)
            {
                Target = target;
            }
        }
        if (_timeStandingEnd < Time.realtimeSinceStartup)
        {
            _moving = true;
        }

        if (_moving)
        {
            Move();
        }
        else
        {
            Animations.CallState(Movement.States.NotRun);
        }

    }
    private void Move()
    {
        bool direction = Target.transform.position.x > transform.position.x;
        if(direction != _lastMovementDirection)
        {
            _lastMovementDirection = direction;
            // mirror the key
            CreatedKey.transform.localScale = new Vector3(direction ? -1 : 1, transform.localScale.y, transform.localScale.z);
        }
        Animations.SetMovementSpeed(Speed);
        Animations.Move(direction ? 1 : -1);
    }

    // communicate with the teammate within communication range
    private void Communicate(Enemy teammate)
    {
        if(teammate.Dead)
        {
            _alertFocus = new KeyValuePair<Vector3, float>(teammate.transform.position, Time.realtimeSinceStartup);
            Alerted = true;
        } 
        else if (Alerted)
        {
            // update alert focus only if the time of last alert is newer
            if (teammate._alertFocus.Value < _alertFocus.Value)
            {
                teammate._alertFocus = _alertFocus;
            }
            teammate.Alerted = true;
        }

    }
    // if player was seen by enemy
    private void ProceedPlayer(PlayerInteractions player, float dist)
    {
        if(player.Alive)
        {

            if (dist < _fightStart)
            {
                Fighting = true;
                Player = player;
            }
            else if (dist < _alertRange) { 
                _alertFocus = new KeyValuePair<Vector3, float>(player.transform.position, Time.realtimeSinceStartup);
                Alerted = true;
            }
        }
    }
}
