using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    [SerializeField] private Collider2D HitPointCollider;
    
    private bool _attack;
    [SerializeField] private float _damageGiven = 10;
    private float attackDuration = 0.2f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        HitPointCollider.enabled = false;
    }

    
    // Update is called once per frame
    void Update()
    {
        // if (_attack)
        // {
        //     Debug.Log("attacking");
        //     Hit();
        // }
    }
    
    public void Attack(InputAction.CallbackContext context)
    {
        _attack = context.ReadValueAsButton();
        if (context.started)
        {
            _animator.SetBool("Attack", true);
            HitPointCollider.enabled = true;
        }
        if (context.canceled)
        {
            _animator.SetBool("Attack", false);
            HitPointCollider.enabled = false;
        }
    }

    // private void Hit()
    // {
    //     HitPointCollider.enabled = true;
    //     Invoke(nameof(HitPointCollider), attackDuration);
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("hit an enemy");
            EnemyHealthPoints enemyHP = other.GetComponent<EnemyHealthPoints>();
            if (enemyHP != null)
            {
                enemyHP.TakeDamage(_damageGiven, transform.position);
            }
        }
    }
}
