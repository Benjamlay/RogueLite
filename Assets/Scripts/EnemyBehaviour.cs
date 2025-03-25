using System;
using System.Collections;
using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    
    private Transform Player;
    private Vector2 targetPosition;
    [SerializeField] public float  patrolRadius = 2f;
    [SerializeField] public float arrivalThreshold = 0.1f;
    public EnemyDetection _enemyDetection;
    public EnemyMotion _enemyMotion;
    
    

    private void Awake()
    {
        _enemyDetection = GetComponent<EnemyDetection>();
        _enemyMotion = GetComponent<EnemyMotion>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }
    void Start()
    {
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
        targetPosition = (Vector2)transform.position + (Vector2)transform.right + randomOffset;
        
        _enemyMotion.SetDestination(randomOffset);
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
        Debug.Log("arrow instantiated");
       Instantiate(projectile, (Vector2)transform.position, Quaternion.identity);
        // Rigidbody2D _rb = bullet.GetComponent<Rigidbody2D>();
        // _rb.AddForce((_enemyDetection.player.position - transform.position) * speed, ForceMode2D.Impulse);
        yield return null;
    }
}