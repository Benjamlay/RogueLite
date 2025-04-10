using UnityEngine;

public class ChaserEnemy : MonoBehaviour
{
    
    [HideInInspector] public EnemyDetection _enemyDetection;
    [HideInInspector] public EnemyMotion _enemyMotion;
    
    [SerializeField] public float  patrolRadius = 2f;
    State _currentState = State.Patrol;
    private Transform Player;
    public enum State { Patrol, Chase, Attack }

    private Vector2 randomOffset;
   [SerializeField] private float attackRate;
    private float nextAttack;
    private float PatrolCoolDown = 3f;
    private float nextPatrolRecalculateTime;
    private float ChaseCoolDown = 1f;
    
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        _enemyDetection = GetComponent<EnemyDetection>();
        _enemyMotion = GetComponent<EnemyMotion>();
        nextPatrolRecalculateTime = Time.time + PatrolCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case State.Patrol:
                Patrol();
               // Debug.Log("Patrol");
                break;
            case State.Chase:
                Chase();
               // Debug.Log("Chase");
                break;
        }
        CheckTransitions();
    }

    

    private void Patrol()
    {
        if(Time.time >= nextPatrolRecalculateTime)
        {
            randomOffset = Random.insideUnitCircle * patrolRadius;
            nextPatrolRecalculateTime = Time.time + PatrolCoolDown;
        }
        
        _enemyMotion.SetDestination(randomOffset.normalized);
    }

    
    private void Chase()
    {
        _enemyMotion.SetDestination(Player.position);
    }

    private void Attack()
    {
         /*Not implemented yet*/
    }

    void CheckTransitions()
    {
        if (_enemyDetection._detected)
        {
            _currentState = State.Chase;
        }
        else
        {
            _currentState = State.Patrol;
        }
    }
}
