using System;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    
    [SerializeField] public float _speed;
    
    private EnemyDetection _enemyDetection;
    
    [SerializeField] private GameObject player;


    // Update is called once per frame
    
    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = ((Vector2)player.transform.position - (Vector2)transform.position) * _speed;
        Destroy(gameObject, 2f);
    }
    void Update()
    {
        
    }
}
