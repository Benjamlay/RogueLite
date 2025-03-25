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
        
        Vector2 direction = (Player.position - transform.position).normalized;

        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        
        GameObject arrow = Instantiate(projectile, transform.position, rotation);
       
        arrow.GetComponent<Rigidbody2D>().linearVelocity = direction * 10;
        Destroy(arrow, 2f);
        
        yield return null;
    }
}