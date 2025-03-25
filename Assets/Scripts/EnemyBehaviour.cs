using System;
using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    private EnemyMotion _enemyMotion;
    private Transform Player;
    private Vector2 targetPosition;
    public float  patrolRadius = 2f;
    
    public float moveSpeed = 3f;
    public float arrivalThreshold = 0.1f;

    private void Awake()
    {
        _enemyMotion = GetComponent<EnemyMotion>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        
        //InvokeRepeating("Chase", 0, 0.5f);
        InvokeRepeating("Patrol", 0f, 2f);
    }

    
    void FixedUpdate()
    {
        
    }

    void Chase()
    {
        _enemyMotion.SetDestination(Player.position);
    }

    void Patrol()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        targetPosition = (Vector2)transform.position + (Vector2)transform.right + randomOffset;
        //Debug.Log("Nouveau point de patrouille : " + targetPosition);
        
        _enemyMotion.SetDestination(randomOffset);
    }

    void Flee()
    {
        Vector2 Destination = (transform.position - Player.position).normalized;
        _enemyMotion.SetDestination(Destination * 10);
    }
}