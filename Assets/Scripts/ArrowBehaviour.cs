using System;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    private EnemyDetection _enemyDetection;
    
    [SerializeField] private GameObject player;


    // Update is called once per frame
    
    void Start()
    {
        Destroy(this, 2f);
    }
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
