using System;
using System.Collections;
using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    
    private Transform Player;
    [SerializeField] private PlayerHealth PlayerHealth;

    [SerializeField] GameObject _explosionAnimator;
    [SerializeField] GameObject _firepoint;
    private Vector2 _targetPosition;
    [SerializeField] public float  patrolRadius = 2f;
    [SerializeField] public float arrivalThreshold = 0.1f;
    public EnemyDetection _enemyDetection;
    public EnemyMotion _enemyMotion;
    private SpriteRenderer _spriteRenderer;
    
    private Animator _animator;

    [SerializeField] private Collider2D _BombCollider;

    private void Awake()
    {
        _enemyDetection = GetComponent<EnemyDetection>();
        _enemyMotion = GetComponent<EnemyMotion>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
    }
    void Start()
    {
        
        _BombCollider.enabled = false;
        _animator = GetComponentInChildren<Animator>();
        _animator.SetBool("Explode", false);
    }
    void FixedUpdate()
    {
    }
    void Update()
    {
    }
    
    public IEnumerator Chase()
    {
        _enemyMotion.SetDestination(Player.position);
        yield return null;
    }

    public IEnumerator Patrol()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        //_targetPosition = (Vector2)transform.position + (Vector2)transform.right + randomOffset;
        
        _enemyMotion.SetDestination(randomOffset);
        yield return null;
    }

    public IEnumerator Arrival()
    {
        _enemyMotion.SetDestination(new Vector2(0, 0));
        yield return null;
    }

    public IEnumerator Flee()
    {
        Vector2 Destination = (transform.position - Player.position).normalized;
        _enemyMotion.SetDestination(Destination * 10);
        yield return null;
    }

    public IEnumerator Shoot(GameObject projectile)
    {
        Vector2 direction = (Player.position - transform.position).normalized;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        GameObject arrow = Instantiate(projectile, transform.position, rotation);
        arrow.GetComponent<Rigidbody2D>().linearVelocity = direction * 10;
        Destroy(arrow, 2f);
        yield return null;
    }

    public IEnumerator Explode()
    {
        _animator.SetBool("Explode", true);
        Invoke("Bomb", 0.2f);
        Destroy(gameObject, 4);
        yield return null;
    }

    private void Bomb()
    {
        _BombCollider.enabled = true;
        _spriteRenderer.enabled = false;
        GameObject explosion = Instantiate(_explosionAnimator, transform.position, transform.rotation);
        Destroy(explosion, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
           //PlayerHealth.TakeDamage(5, transform.position);
        }
        
    }
} 