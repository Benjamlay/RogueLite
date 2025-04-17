using UnityEngine;

public class ArcherEnemy : MonoBehaviour
{
    [HideInInspector] public EnemyDetection _enemyDetection;
    [HideInInspector] public EnemyMotion _enemyMotion;
    private Transform Player;
    private Vector2 randomOffset;
    [SerializeField] public float  patrolRadius = 2f;
    State _currentState = State.Patrol;

    [SerializeField] private GameObject projectile;

    private EnemyHealthPoints _enemyHealthPoints;
    
    private float ShootInterval;
    private float PatrolInterval;
    private float FleeInterval;
    
    [SerializeField] private float shootCoolDown = 3f;
    [SerializeField] private float PatrolCoolDown = 2.5f;
    [SerializeField] private float FleeCoolDown = 1f;
    
    private Animator _animator;

    private enum State { Patrol, Flee, Shoot, Dead }
    
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        _enemyDetection = GetComponent<EnemyDetection>();
        _enemyMotion = GetComponent<EnemyMotion>();
        ShootInterval = Time.time + shootCoolDown;
        PatrolInterval = Time.time + PatrolCoolDown;
        FleeInterval = Time.time + FleeCoolDown;
        _animator = GetComponentInChildren<Animator>();
        _enemyHealthPoints = GetComponent<EnemyHealthPoints>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Flee:
                Flee();
                break;
            case State.Shoot:
                Shoot();
                break;
            case State.Dead:
                Dead();
                break;
        }
        CheckTransitions();
    }

    private void Patrol()
    {
        if(Time.time >= PatrolInterval)
        {
            _animator.SetBool("Running", false);
            randomOffset = Random.insideUnitCircle * patrolRadius;
            _enemyMotion.SetDestination(randomOffset);
            PatrolInterval = Time.time + PatrolCoolDown;
        }
    }

    private void Dead()
    {
        _animator.SetBool("Running", false);
        _animator.SetBool("ShootLeft", false);
        _animator.SetBool("Dead", true);
        _enemyMotion.rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Flee()
    {
        if(Time.time >= FleeInterval)
        {
            Vector2 destination = (transform.position - Player.position).normalized;
            _enemyMotion.SetDestination(destination * 10);
            _animator.SetBool("Running", true);
            FleeInterval = Time.time + FleeCoolDown;
        }
    }

    private void Shoot()
    {
        if(Time.time >= ShootInterval)
        {
            _animator.SetBool("ShootLeft", true);
            Vector2 direction = (Player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject arrow = Instantiate(projectile, transform.position, rotation);
            arrow.GetComponent<Rigidbody2D>().linearVelocity = direction * 5;
            Destroy(arrow, 2f);
            ShootInterval = Time.time + shootCoolDown;
        }
    }
    
    void CheckTransitions()
    {
        if (_enemyDetection._detected)
        {
            _currentState = State.Shoot;
            if (_enemyDetection._PlayerTooClose)
            {
                _currentState = State.Flee;
            }
        }
        else
        {
            _currentState = State.Patrol;
        }

        if (_enemyHealthPoints._healthPoints <= 0)
        {
            _currentState = State.Dead;
        }
    }
}
