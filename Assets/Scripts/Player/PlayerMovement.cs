using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    private Rigidbody2D _rb;

    private bool _MoveUp;
    private bool _MoveDown;
    private bool _MoveLeft;
    private bool _MoveRight;
    
    // public bool lookingUp;
    // public bool lookingLeft;
    // public bool lookingDown;
    public Vector2 JoystickMovement;
    private Animator _animator;
    
    public Vector2 moveDirection = Vector2.zero;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        // lookingUp = false;
        // lookingLeft = false;
        // lookingDown = false;
    }

    
    
    void Update()
    {
        Move();
    }

    public void MoveUp(InputAction.CallbackContext context)
    {
        _MoveUp = context.ReadValueAsButton();
    }
    public void MoveDown(InputAction.CallbackContext context)
    {
        _MoveDown = context.ReadValueAsButton();
    }
    public void MoveLeft(InputAction.CallbackContext context)
    {
        _MoveLeft = context.ReadValueAsButton();
    }
    public void MoveRight(InputAction.CallbackContext context)
    {
        _MoveRight = context.ReadValueAsButton();
    }
    
    private void Move()
    {
        moveDirection = Vector2.zero;
        
        if(_MoveUp)
        {
            moveDirection += Vector2.up;
        }
        if (_MoveDown)
        {
            moveDirection += Vector2.down;
        }
        if (_MoveLeft)
        {
            moveDirection += Vector2.left;
        }
        if (_MoveRight)
        {
            moveDirection += Vector2.right;
        }

        if (moveDirection != Vector2.zero)
        {
            moveDirection = moveDirection.normalized;
            _rb.AddForce(moveDirection * (_speed * Time.deltaTime), ForceMode2D.Impulse);
        }
        
        _rb.AddForce(JoystickMovement * (_speed * Time.deltaTime) , ForceMode2D.Impulse);
        
        if (_rb.linearVelocity.x > 0.1)
        {
            _animator.SetBool("Running", true);
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        else if (_rb.linearVelocity.x < -0.1)
        {
            _animator.SetBool("Running", true);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (_rb.linearVelocity.y > 0.1)
        {
            _animator.SetBool("Running", true);
        }
        else if (_rb.linearVelocity.y < -0.1)
        {
            _animator.SetBool("Running", true);
        }
        
        else {_animator.SetBool("Running", false);}
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void ControllerMove(InputAction.CallbackContext context)
    {
        JoystickMovement = context.ReadValue<Vector2>();
    }
}
