using System.Collections;
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
    [SerializeField] private GameObject MagicalShield;
	[SerializeField] private BoxCollider2D PlayerCollider;
    private bool _attack;
    [HideInInspector] public bool _shieldUp;
    [SerializeField] private float _damageGiven = 10;
    
    [SerializeField] public PlayerMovement _playerMovement;

    private float _shieldCoolDown = 5f;
    private bool _canUseShield;
    
    private float ShakeCamDuration = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        HitPointColliderLeft.enabled = false;
        HitPointColliderUp.enabled = false;
        HitPointColliderDown.enabled = false;
        MagicalShield.SetActive(false);
        _canUseShield = true;
    }
    
    void Update()
    {
        if(_shieldUp)
        {
            MagicalShield.SetActive(true);
        }
        else
        {
            MagicalShield.SetActive(false);
        }
    }
    
    public void Attack(InputAction.CallbackContext context)
    {
        _attack = context.ReadValueAsButton();
        
        if (context.started)
        {
            AttackInput(_playerMovement.JoystickMovement);
            AttackInput(_playerMovement.moveDirection);
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

    // private void KeyBoardAttack()
    // {
    //     if(_playerMovement.moveDirection == Vector2.left)
    //     {
    //         _animator.SetBool("AttackLeft", true);
    //         HitPointColliderLeft.enabled = true;
    //     }
    //     else if(_playerMovement.moveDirection == Vector2.right)
    //     {
    //         _animator.SetBool("AttackLeft", true);
    //         HitPointColliderLeft.enabled = true;
    //     }
    //     else if (_playerMovement.moveDirection == Vector2.up)
    //     {
    //         _animator.SetBool("AttackUp", true);
    //         HitPointColliderUp.enabled = true;
    //     }
    //     else if (_playerMovement.moveDirection == Vector2.down)
    //     {
    //         _animator.SetBool("AttackDown", true);
    //         HitPointColliderDown.enabled = true;
    //     }
    // }

    // private void JoystickAttack()
    // {
    //     if (Mathf.Abs(_playerMovement.JoystickMovement.y) > Mathf.Abs(_playerMovement.JoystickMovement.x))
    //     {
    //         if (_playerMovement.JoystickMovement.y > 0)
    //         {
    //             _animator.SetBool("AttackUp", true);
    //             HitPointColliderUp.enabled = true;
    //         }
    //         else if (_playerMovement.JoystickMovement.y < 0)
    //         {
    //             _animator.SetBool("AttackDown", true);
    //             HitPointColliderDown.enabled = true;
    //         }
    //     }
    //     else
    //     {
    //         if (_playerMovement.JoystickMovement.x > 0)
    //         {
    //             _animator.SetBool("AttackLeft", true);
    //             HitPointColliderLeft.enabled = true;
    //         }
    //         else if (_playerMovement.JoystickMovement.x < 0)
    //         {
    //             _animator.SetBool("AttackLeft", true);
    //             HitPointColliderLeft.enabled = true;
    //         }
    //     }
    // }
    
    private void AttackInput(Vector2 input)
    {
        if (Mathf.Abs(input.y) > Mathf.Abs(input.x))
        {
            if (input.y > 0)
            {
                _animator.SetBool("AttackUp", true);
                HitPointColliderUp.enabled = true;
            }
            else if (input.y < 0)
            {
                _animator.SetBool("AttackDown", true);
                HitPointColliderDown.enabled = true;
            }
        }
        else
        {
            if (input.x > 0)
            {
                _animator.SetBool("AttackLeft", true);
                HitPointColliderLeft.enabled = true;
            }
            else if (input.x < 0)
            {
                _animator.SetBool("AttackLeft", true);
                HitPointColliderLeft.enabled = true;
            }
        }
    }
    public void ActivateShield(InputAction.CallbackContext context)
    {
        if(_canUseShield)
        {
            if (context.started)
            {
                _shieldUp = true;
                StartCoroutine("ShieldDuration");
            }
        }

        if (context.canceled)
        {
            _shieldUp = false;
            StartCoroutine("ShieldCooldown");
        }
    }

    public IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(3f);
        _shieldUp = false;
        _canUseShield = false;
        yield return new WaitForSeconds(3f);
        _canUseShield = true;
    }

    public IEnumerator ShieldCooldown()
    {
        _canUseShield = false;
        yield return new WaitForSeconds(1f);
        _canUseShield = true;
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
                CamShakeManager.Instance.Shake(2f, 2f, ShakeCamDuration);
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
                CamShakeManager.Instance.Shake(2f, 2f, ShakeCamDuration);
                treeBehaviour.health--;
                
            }
        }
    }
}
