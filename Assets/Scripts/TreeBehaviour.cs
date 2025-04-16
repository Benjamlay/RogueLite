using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    private Animator animator;

     [SerializeField] public float health;
     
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0)
        {
            animator.SetBool("Dead", true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("tree"))
        {
            DestroyImmediate(other.gameObject);
            DestroyImmediate(gameObject);
        }
    }
    
}
