using System.Collections;
using UnityEngine;

public class EnemyHealthPoints : MonoBehaviour
{
    [SerializeField] private float _healthPoints;
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    [SerializeField] public float knockbackForce = 5f;
    [SerializeField] public float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void TakeDamage(float damage, Vector2 attackPosition)
    {
        if (isKnockedBack) return;
        
        _healthPoints -= damage;
        
        StartCoroutine(FlashRed());
        StartCoroutine(Knockback(attackPosition));
        
        if (_healthPoints <= 0)
        {
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
}
