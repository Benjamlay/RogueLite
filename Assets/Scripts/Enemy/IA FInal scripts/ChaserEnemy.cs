using System.Collections;
using UnityEngine;

public class ChaserEnemy : MonoBehaviour
{
    
    [HideInInspector] public EnemyDetection _enemyDetection;
    [HideInInspector] public EnemyMotion _enemyMotion;
    
    [SerializeField] public float  patrolRadius = 2f;
    State _currentState = State.Patrol;
    private Transform Player;
    private Rigidbody2D _rb;
    public enum State { Patrol, Chase, Attack, Dead }

    private Vector2 randomOffset;
   [SerializeField] private float attackRate;
    private float nextAttack;
    [SerializeField] private float PatrolCoolDown = 3f;
    private float nextPatrolRecalculateTime;

    private float ChaseInterval;
    [SerializeField] private float ChaseCoolDown = 1.5f;

    private float AttackInterval;
    [SerializeField] private float AttackCoolDown = 2f;
    
    private Animator _animator;
    private EnemyHealthPoints _enemyHealthPoints;

    [SerializeField] private CircleCollider2D ColUp;
    [SerializeField] private CircleCollider2D ColDown;
    [SerializeField] private CircleCollider2D ColLeft;

    [SerializeField] private CircleCollider2D HitPoint;
    
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        _rb = GetComponent<Rigidbody2D>();
        _enemyDetection = GetComponent<EnemyDetection>();
        _enemyMotion = GetComponent<EnemyMotion>();
        nextPatrolRecalculateTime = Time.time + PatrolCoolDown;
        AttackInterval = Time.time + AttackCoolDown;
        _animator = GetComponentInChildren<Animator>();
        _enemyHealthPoints = GetComponent<EnemyHealthPoints>();
        _currentState = State.Patrol;
        ColUp.enabled = false;
        ColDown.enabled = false;
        ColLeft.enabled = false;
        HitPoint.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Dead:
                Dead();
                break;
        }
        CheckTransitions();
    }

    

    private void Patrol()
    {
        if(Time.time >= nextPatrolRecalculateTime)
        {
            randomOffset = Random.insideUnitCircle * patrolRadius;
            _enemyMotion.SetDestination(randomOffset);
            nextPatrolRecalculateTime = Time.time + PatrolCoolDown;
        }
    }
    
    private void Dead()
    {
        _animator.SetBool("Dead", true);
        _enemyMotion.rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    
    private void Chase()
    {
        if(Time.time >= ChaseInterval)
        {
            _enemyMotion.SetDestination(Player.position);
            ChaseInterval = Time.time + ChaseCoolDown;
        }
    }

    private void Attack()
    {
        if(Time.time >= AttackInterval)
        {
            // if (_rb.linearVelocity.x > 0.1)
            // {
            //     transform.localScale = new Vector3(1, 1, 1);
            //     ColLeft.enabled = true;
            //     ColDown.enabled = false;
            //     ColUp.enabled = false;
            // }
            //
            // if (_rb.linearVelocity.x > -0.1)
            // {
            //     transform.localScale = new Vector3(-1, 1, 1);
            //     ColLeft.enabled = true;
            //     ColDown.enabled = false;
            //     ColUp.enabled = false;
            // }
            //
            // if (_rb.linearVelocity.y < -0.1)
            // {
            //     ColDown.enabled = true;
            //     ColUp.enabled = false;
            //     ColLeft.enabled = false;
            // }
            //
            // if (_rb.linearVelocity.y < 0.1)
            // {
            //     ColUp.enabled = true;
            //     ColDown.enabled = false;
            //     ColLeft.enabled = false;
            // }
            StartCoroutine("HitPointEnable");
            AttackInterval = Time.time + AttackCoolDown;
            
        }
    }

    IEnumerator HitPointEnable()
    {
        _animator.SetTrigger("New Trigger");
        HitPoint.enabled = true;
        yield return new WaitForSeconds(0.2f);
        HitPoint.enabled = false;
    }
    void CheckTransitions()
    {
        if (_enemyDetection._detected)
        {
            _currentState = State.Chase;
            
            if (_enemyDetection._PlayerTooClose)
            {
                _currentState = State.Attack;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(1, transform.position);
        }
    }
}
