using System;
using UnityEngine;
using Pathfinding;
public class EnemyMotion : MonoBehaviour
{

    public Vector2 target;
    public float speed = 200;
    public float nextWaypointDistance = 3;

    //public Transform enemyGFX;
    
    Path path;
    int currentWaypoint = 0;
    //bool reachedEndOfPath = false;
    
    [SerializeField] Seeker seeker;
    [HideInInspector] public Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        
    }

    

    public void SetDestination(Vector2 destination)
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, destination, OnPathComplete);
        }
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target, OnPathComplete);
        }
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Update()
    {
        SpeedControl();
    }
    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            //reachedEndOfPath = true;
            return;
        }
        else
        {
            //reachedEndOfPath = false;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * (speed * Time.deltaTime);
        
        rb.AddForce(force * 10);
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
        // if (force.x >= 0.1f)
        // {
        //     enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        // }else if (force.x <= -0.1f)
        // {
        //     enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        // }
        
        
    }

    private void SpeedControl()
    {
        if (rb.linearVelocity.magnitude > speed)
        {
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity.normalized, speed);
        }
    }
}
