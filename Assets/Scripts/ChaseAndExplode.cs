using UnityEngine;

public class ChaseAndExplode : MonoBehaviour
{
    private EnemyBehaviour _enemyBehaviour;
    private bool _explosionSoon;
    
    private Vector2 lastPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    private void Awake()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
        _explosionSoon = false;
    }
    
    
    void Start()
    {
        InvokeRepeating("updateState", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_explosionSoon)
        {
            _enemyBehaviour._enemyMotion.rb.position = lastPosition;
        }
    }
    
    private void updateState()
    {
        if(!_explosionSoon)
        {
            if (_enemyBehaviour._enemyDetection._detected)
            {
                StartCoroutine(_enemyBehaviour.Chase());
            }
            else if (_enemyBehaviour._enemyDetection._detected == false)
            {
                StartCoroutine(_enemyBehaviour.Arrival());
            }
        }
        else if (_explosionSoon)
        {
            StartCoroutine(_enemyBehaviour.Explode());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            lastPosition = _enemyBehaviour.transform.position;
            _explosionSoon = true;
        }
    }
    
    
}
