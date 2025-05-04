using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float health;
    [SerializeField] private Collider2D playerCollider;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator _animator;
    
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    
    [SerializeField] public float knockbackForce = 5f;
    [SerializeField] public float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;

    private bool IsHit;
    private float InvincibilityCoolDown = 1f;
    private float InvincibilityTimer;
    
    public bool gameOver;
    public event Action<PlayerHealth> OnDead;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        PlayerManager pManager = FindAnyObjectByType<PlayerManager>();
        pManager.AddPlayer(this);
        UpdateHearts();
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            gameOver = true;
        }
    }
    
    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }
    
    public void TakeDamage(float damage, Vector2 attackPosition)
    {
        if (!IsHit)
        {
            if (isKnockedBack) return;
            health -= damage;
            UpdateHearts();
            CamShakeManager.Instance.Shake(2f, 2f, 0.7f);
            StartCoroutine(FlashRed());
            StartCoroutine(Knockback(attackPosition));
            StartCoroutine(Invincibility());
        }
        if (health <= 0)
        {
            OnDead?.Invoke(this);
            _animator.SetBool("Dead", true);
            playerCollider.enabled = false;
            GetComponent<PlayerInput>().enabled = false;
        }
    }

    private IEnumerator Invincibility()
    {
        IsHit = true;
        yield return new WaitForSeconds(InvincibilityCoolDown);
        IsHit = false;
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
            TakeDamage(1, transform.position);
            Destroy(collision.gameObject);
        }
        

        if (collision.gameObject.CompareTag("Patroller"))
        {
            TakeDamage(1, transform.position);
        }
    }
}
