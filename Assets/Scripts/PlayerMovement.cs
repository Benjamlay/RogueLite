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
    
    public bool lookingUp;
    public bool lookingLeft;
    public bool lookingDown;
    private Animator _animator;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        lookingUp = false;
        lookingLeft = false;
    }

    
    
    void Update()
    {
        Move();
    }

    public void MoveUp(InputAction.CallbackContext context)
    {
        _MoveUp = context.ReadValueAsButton();
        lookingUp = true;
        lookingLeft = false;
        lookingDown = false;
    }
    public void MoveDown(InputAction.CallbackContext context)
    {
        _MoveDown = context.ReadValueAsButton();
        lookingDown = true;
        lookingLeft = false;
        lookingUp = false;
    }
    public void MoveLeft(InputAction.CallbackContext context)
    {
        _MoveLeft = context.ReadValueAsButton();
        lookingLeft = true;
        lookingUp = false;
        lookingDown = false;
    }
    public void MoveRight(InputAction.CallbackContext context)
    {
        _MoveRight = context.ReadValueAsButton();
        lookingLeft = true;
        lookingUp = false;
        lookingDown = false;
        
    }


    private void Move()
    {
        if(_MoveUp)
        {
            _rb.AddForce(Vector2.up * (_speed * Time.deltaTime), ForceMode2D.Impulse);
        }
        if (_MoveDown)
        {
            _rb.AddForce(Vector2.down * (_speed * Time.deltaTime), ForceMode2D.Impulse);
        }
        if (_MoveLeft)
        {
            _rb.AddForce(Vector2.left * (_speed * Time.deltaTime), ForceMode2D.Impulse);
        }
        if (_MoveRight)
        {
            _rb.AddForce(Vector2.right * (_speed * Time.deltaTime), ForceMode2D.Impulse);
        }

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
}
