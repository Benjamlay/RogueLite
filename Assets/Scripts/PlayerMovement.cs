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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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
    
}
