using UnityEngine;

public class ArcherEnemy : MonoBehaviour
{
    [HideInInspector] public EnemyDetection _enemyDetection;
    [HideInInspector] public EnemyMotion _enemyMotion;
    private Transform Player;
    private Vector2 randomOffset;
    [SerializeField] public float  patrolRadius = 2f;
    State _currentState = State.Patrol;

    [SerializeField] private GameObject projectile;
    
    private float ShootInterval;
    [SerializeField] private float shootCoolDown = 3f;
    public enum State { Patrol, Flee, Shoot }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        _enemyDetection = GetComponent<EnemyDetection>();
        _enemyMotion = GetComponent<EnemyMotion>();
        ShootInterval = Time.time + shootCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case State.Patrol:
                Patrol();
                Debug.Log("Patrol");
                break;
            case State.Flee:
                Flee();
                Debug.Log("Flee");
                break;
            case State.Shoot:
                Shoot();
                Debug.Log("Shooting");
                break;
        }

        CheckTransitions();
    }

    private void Patrol()
    {
        randomOffset = Random.insideUnitCircle * patrolRadius;
        _enemyMotion.SetDestination(randomOffset.normalized);
    }

    private void Flee()
    {
        Vector2 destination = (transform.position - Player.position).normalized;
        _enemyMotion.SetDestination(destination * 10);
    }

    private void Shoot()
    {
        if(Time.time >= ShootInterval)
        {
            Vector2 direction = (Player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject arrow = Instantiate(projectile, transform.position, rotation);
            arrow.GetComponent<Rigidbody2D>().linearVelocity = direction * 5;
            Destroy(arrow, 2f);
            ShootInterval = Time.time + shootCoolDown;
        }
    }
    
    void CheckTransitions()
    {
        if (_enemyDetection._detected)
        {
            _currentState = State.Shoot;
            if (_enemyDetection._PlayerTooClose)
            {
                _currentState = State.Flee;
            }
        }
        
        else
        {
            _currentState = State.Patrol;
        }
    }
}
