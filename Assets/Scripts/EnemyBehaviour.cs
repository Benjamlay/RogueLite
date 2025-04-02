using System;
using System.Collections;
using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    
    private Transform Player;
    [SerializeField] GameObject _explosionAnimator;
    //[SerializeField] GameObject _firepoint;
    private Vector2 _targetPosition;
    [SerializeField] public float  patrolRadius = 2f;
    [SerializeField] public float arrivalThreshold = 0.1f;
    [HideInInspector] public EnemyDetection _enemyDetection;
    [HideInInspector] public EnemyMotion _enemyMotion;
    private SpriteRenderer _spriteRenderer;
    
    private Animator _animator;
    
    [HideInInspector] public bool StartCoroutine;

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
    
    public IEnumerator Chase()
    {
        StartCoroutine = true;
        yield return new WaitForSeconds(0.7f);
        _enemyMotion.SetDestination(Player.position);
        StartCoroutine = false;
    }
    
    public IEnumerator Patrol()
    {
        StartCoroutine = true;
        yield return new WaitForSeconds(3);
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        
        _enemyMotion.SetDestination(randomOffset);
        StartCoroutine = false;
    }
    
    

    public IEnumerator Arrival()
    {
        _enemyMotion.SetDestination(new Vector2(0, 0));
        yield return null;
    }

    public IEnumerator Flee()
    {
        StartCoroutine = true;
        yield return new WaitForSeconds(0.5f);
        Vector2 Destination = (transform.position - Player.position).normalized;
        _enemyMotion.SetDestination(Destination * 10);
        
        StartCoroutine = false;
    }

    public IEnumerator Shoot(GameObject projectile)
    {
        StartCoroutine = true;
        yield return new WaitForSeconds(1.5f);
        Vector2 direction = (Player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject arrow = Instantiate(projectile, transform.position, rotation);
        arrow.GetComponent<Rigidbody2D>().linearVelocity = direction * 5;
        Destroy(arrow, 2f);
        StartCoroutine = false;
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
} 