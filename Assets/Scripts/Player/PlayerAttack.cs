using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    [SerializeField] private Collider2D HitPointColliderLeft;
    [SerializeField] private Collider2D HitPointColliderUp;
    [SerializeField] private Collider2D HitPointColliderDown;
    private bool _attack;
    [SerializeField] private float _damageGiven = 10;
    
    [SerializeField] public PlayerMovement _playerMovement;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        HitPointColliderLeft.enabled = false;
        HitPointColliderUp.enabled = false;
        HitPointColliderDown.enabled = false;
    }

    
    // Update is called once per frame
    void Update()
    {
       
    }
    
    public void Attack(InputAction.CallbackContext context)
    {
        _attack = context.ReadValueAsButton();
        
        if (context.started)
        {
            if(_playerMovement.lookingLeft)
            {
                _animator.SetBool("AttackLeft", true);
                HitPointColliderLeft.enabled = true;
            }
            else if (_playerMovement.lookingUp)
            {
                _animator.SetBool("AttackUp", true);
                HitPointColliderUp.enabled = true;
            }
            else if (_playerMovement.lookingDown)
            {
                _animator.SetBool("AttackDown", true);
                HitPointColliderDown.enabled = true;
            }
        }
        if (context.canceled)
        {
            _animator.SetBool("AttackLeft", false);
            _animator.SetBool("AttackUp", false);
            _animator.SetBool("AttackDown", false);
            HitPointColliderLeft.enabled = false;
            HitPointColliderUp.enabled = false;
            HitPointColliderDown.enabled = false;
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Patroller"))
        {
            Debug.Log("hit an enemy");
            EnemyHealthPoints enemyHP = other.GetComponent<EnemyHealthPoints>();
            if (enemyHP != null)
            {
                enemyHP.TakeDamage(_damageGiven, transform.position);
                CamShakeManager.Instance.Shake(1f, 1f, 0.5f);
            }
        }

        if (other.CompareTag("tree"))
        {
            Debug.Log("hit a tree");
            Animator animator = other.GetComponent<Animator>();
            TreeBehaviour treeBehaviour = other.GetComponent<TreeBehaviour>();
            if(!animator.GetBool("Dead"))
            {
                animator.Play("Hit");
                CamShakeManager.Instance.Shake(1f, 1f, 0.5f);
                treeBehaviour.health--;
                
            }
        }
    }
}
