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
        Destroy(this, 2f);
    }
    void Update()
    {
        
    }
}
