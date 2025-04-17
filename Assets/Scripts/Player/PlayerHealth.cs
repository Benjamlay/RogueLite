using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float health = 5f;
    [SerializeField] private Collider2D playerCollider;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    [SerializeField] public float knockbackForce = 5f;
    [SerializeField] public float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;
    
    public event Action<PlayerHealth> OnDead;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        PlayerManager pManager = FindAnyObjectByType<PlayerManager>();
        pManager.AddPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            //afficher game over
        }
        //Debug.Log(health);
        
    }
    public void TakeDamage(float damage, Vector2 attackPosition)
    {
        if (isKnockedBack) return;
        
        health -= damage;
        
        StartCoroutine(FlashRed());
        StartCoroutine(Knockback(attackPosition));
        
        if (health <= 0)
        {
            OnDead?.Invoke(this);
            Destroy(gameObject);
        }
    }
    

    private IEnumerator Knockback(Vector2 attackPosition)
    {
        isKnockedBack = true;
        Vector2 knockbackDirection = (transform.position - (Vector3)attackPosition).normalized;
        rb.linearVelocity = knockbackDirection * knockbackForce;

        yield return new WaitForSeconds(knockbackDuration);
    
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }
    
    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            TakeDamage(5, transform.position);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Bomb"))
        {
            TakeDamage(5, transform.position);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Patroller"))
        {
            TakeDamage(5, transform.position);
        }
    }
}
