using UnityEngine;

public class ChaserEnemy : MonoBehaviour
{
    
    [HideInInspector] public EnemyDetection _enemyDetection;
    [HideInInspector] public EnemyMotion _enemyMotion;
    
    [SerializeField] public float  patrolRadius = 2f;
    State _currentState = State.Patrol;
    private Transform Player;
    public enum State { Patrol, Chase, Attack, Dead }

    private Vector2 randomOffset;
   [SerializeField] private float attackRate;
    private float nextAttack;
    [SerializeField] private float PatrolCoolDown = 3f;
    private float nextPatrolRecalculateTime;

    private float ChaseInterval;
    [SerializeField] private float ChaseCoolDown = 1.5f;
    
    private Animator _animator;
    private EnemyHealthPoints _enemyHealthPoints;
    
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        _enemyDetection = GetComponent<EnemyDetection>();
        _enemyMotion = GetComponent<EnemyMotion>();
        nextPatrolRecalculateTime = Time.time + PatrolCoolDown;
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
            case State.Chase:
                Chase();
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
        // _animator.SetBool("Running", false);
        // _animator.SetBool("ShootLeft", false);
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
         /*Not implemented yet*/
    }

    void CheckTransitions()
    {
        if (_enemyDetection._detected)
        {
            _currentState = State.Chase;
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
