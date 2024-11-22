using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private MainInputAction _playerInput;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;

    private Vector2 _moveInput;
    private bool _isJumping = false;

    private void Awake()
    {
        _moveInput = Vector2.zero;
        _playerInput = new MainInputAction();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _playerInput.Enable();

        _playerInput.Player.Move.performed += OnMovePerformed;
        _playerInput.Player.Move.canceled += OnMoveCanceled;
        _playerInput.Player.Attack.performed += OnAttackPerformed;
        _playerInput.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        _playerInput.Player.Move.performed -= OnMovePerformed;
        _playerInput.Player.Move.canceled -= OnMoveCanceled;
        _playerInput.Player.Attack.performed -= OnAttackPerformed;
        _playerInput.Player.Jump.performed -= OnJumpPerformed;

        _playerInput.Disable();
    }

    private void FixedUpdate()
    {
        // TODO 점프중엔 이동 막기
        Vector2 velocity = new Vector2(_moveInput.x * _moveSpeed, _rb.velocity.y);
        _rb.velocity = velocity;

        if (_isJumping)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);            
            _isJumping = false;
        }

        if (_moveInput.x > 0)
            _spriteRenderer.flipX = true;
        else if (_moveInput.x < 0)
            _spriteRenderer.flipX = false;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>().normalized;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        // 공격 로직
        Debug.Log("Attack!");
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        // 땅인지 확인
        _isJumping = true;
        Debug.Log("JUMP!");
    }
}
