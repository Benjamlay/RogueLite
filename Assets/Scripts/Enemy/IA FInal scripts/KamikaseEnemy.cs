using System.Collections;
using UnityEngine;

public class KamikaseEnemy : MonoBehaviour
{
    [HideInInspector] public EnemyDetection _enemyDetection;
    [HideInInspector] public EnemyMotion _enemyMotion;
    private Transform Player;
    State _currentState = State.Idle;
    private Animator _animator;

    [SerializeField] private GameObject _explosion;
    private EnemyHealthPoints _enemyHealthPoints;
    private enum State { Idle, Chase, Explode, Dead }
    
    private float IdleInterval;
    private float ChaseInterval;
    private float ExplodeInterval;

    private Collider2D explosionCollider;
    
    [SerializeField] private float ChaseCoolDown = 0.2f;
    [SerializeField] private float ExplosionCoolDown = 10f;
    [SerializeField] private float idleCoolDown = 1f;

    private GameObject _newExplosion;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        _enemyDetection = GetComponent<EnemyDetection>();
        _enemyMotion = GetComponent<EnemyMotion>();
        _enemyHealthPoints = GetComponent<EnemyHealthPoints>();
        IdleInterval =Time.time + idleCoolDown;
        ChaseInterval = Time.time + ChaseCoolDown;
        ExplodeInterval = Time.time + ExplosionCoolDown;
        explosionCollider = GetComponentInChildren<CircleCollider2D>();
        explosionCollider.enabled = false;
        _animator = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Explode:
                Explode();
                break;
            case State.Dead:
                Dead();
                break;
        }
        CheckTransitions();
    }
    
    private void Dead()
    {
        _animator.SetBool("Explode", false);
        _animator.SetBool("Dead", true);
        _enemyMotion.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        StopAllCoroutines();
        explosionCollider.enabled = false;
    }
    
    private void Chase()
    {
        if(Time.time >= ChaseInterval)
        {
            _enemyMotion.SetDestination(Player.position);
            ChaseInterval = Time.time + ChaseCoolDown;
        }
    }

    private void Idle()
    {
        
    }
    private void Explode()
    {
        StartCoroutine("Exploding");
    }

    IEnumerator Exploding()
    {
        _animator.SetBool("Explode", true);
        yield return new WaitForSeconds(2);
        _enemyHealthPoints.TakeDamage(3f, transform.position);
        _animator.SetBool("Explode", false);
        _animator.SetBool("Boom", true);
        explosionCollider.enabled = true;
        yield return new WaitForSeconds(0.2f);
        explosionCollider.enabled = false;
        yield return new WaitForSeconds(0.2f);
    }
    
    void CheckTransitions()
    {
        if (_enemyDetection._detected)
        {
            _currentState = State.Chase;
            if (_enemyDetection._PlayerTooClose)
            {
                _currentState = State.Explode;
            }
        }
        else
        {
            _currentState = State.Idle;
        }

        if (_enemyHealthPoints._healthPoints <= 0)
        {
            _currentState = State.Dead;
        }
    }
}
