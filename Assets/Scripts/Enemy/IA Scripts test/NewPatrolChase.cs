using System.Collections;
using UnityEngine;

public class NewPatrolChase : MonoBehaviour
{
    // public EnemyMotion _enemyMotion;
    // public EnemyDetection _EnemyDetection;
    
    private EnemyBehaviour _enemyBehaviour;
    
    
    private bool chasing;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        PatrolOrChase();
        if (!_enemyBehaviour.StartCoroutine)
        {
            StartCoroutine(chasing ? _enemyBehaviour.Chase() : _enemyBehaviour.Patrol());
        }
    }

    private void PatrolOrChase()
    {
        if (_enemyBehaviour._enemyDetection._detected)
        {
            chasing = true;
            Debug.Log("Chasing");
        }
        else if (_enemyBehaviour._enemyDetection._detected == false)
        {
            chasing = false;
            Debug.Log("Patroling");
        }
    }
}
