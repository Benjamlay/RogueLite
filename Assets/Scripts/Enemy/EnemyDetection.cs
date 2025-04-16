using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
   [SerializeField] public float detectionRadius = 5f; 
   [SerializeField] public float TooCloseRadius = 2f;
   [SerializeField] public LayerMask playerLayer; 
   
   [SerializeField] public Transform player;
   
   public bool _detected;
   public bool _PlayerTooClose;
   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDetection();
        PlayerTooClose();
    }

    private void PlayerDetection()
    {
        Collider2D detected = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (detected != null)
        {
            player = detected.transform;
            _detected = true;
        }
        else
        {
            player = null;
            _detected = false;
        }
        
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawWireSphere(transform.position, TooCloseRadius);
    }

    private void PlayerTooClose()
    {
        Collider2D detected = Physics2D.OverlapCircle(transform.position, TooCloseRadius, playerLayer);

        if (detected != null)
        {
            _PlayerTooClose = true;
        }
        else
        {
            _PlayerTooClose = false;
        }
        
    }
}
