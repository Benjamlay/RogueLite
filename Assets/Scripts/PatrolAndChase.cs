using UnityEngine;

public class PatrolAndChase : MonoBehaviour
{
    private EnemyBehaviour _enemyBehaviour;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool _attack;

    private void Awake()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
    }
    
    void Start()
    {
        InvokeRepeating("updateState", 0f, 1f);
    }


    private void updateState()
    {
            if (_enemyBehaviour._enemyDetection._detected)
            {
                StartCoroutine(_enemyBehaviour.Chase());
            }
            else if (_enemyBehaviour._enemyDetection._detected == false)
            {
                StartCoroutine(_enemyBehaviour.Patrol());
            }
    }
    
}
