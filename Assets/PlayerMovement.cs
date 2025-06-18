using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions controls;
    private Vector2 moveInput;
    private Vector2 jumpDirectionInput;
    [Header("Movement Variables")]
    public float moveSpeed = 5f;

    public float acceleration = 50f;

    public float maxSpeed = 12f;

    public float currentSpeed = 0f;

    private int lastDirection = 0;


    [Header("Jump variables")]

    public float jumpForce = 10f;
    public float doubleJumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    

    private bool isJumping;
    private bool isGrounded;
    private bool canDoubleJump;
    private Rigidbody2D rb;

    void Awake()
    {
        controls = new InputSystem_Actions();
        // Movimento lateral
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Salto
        controls.Player.Jump.performed += ctx => Jump();
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void Update()
    {
        CheckGrounded();

        // Se tocaste no chão, podes saltar de novo
        if (isGrounded)
        {
            canDoubleJump = true;
        }

        // Atualiza a direção que o jogador está a pressionar (WASD)
        jumpDirectionInput = controls.Player.Move.ReadValue<Vector2>().normalized;
            Debug.Log("JumpDirectionInput: " + jumpDirectionInput);
    }

    void FixedUpdate()
    {
        // Se estiver a andar para um lado
        if (moveInput.x != 0)
        {
            // Atualiza a direção
            if (Mathf.Sign(moveInput.x) != lastDirection)
            {
                currentSpeed = 0f; // Reinicia se mudar de direção
                lastDirection = (int)Mathf.Sign(moveInput.x);
            }

            // Acelera até ao máximo
            currentSpeed += acceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

            rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocity.y);
        }
        else
        {
            // Parou de se mexer, reinicia a velocidade
            currentSpeed = 0f;
            lastDirection = 0;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

       
    }

    // Só modifica a variável
    void Jump()
    {
         if (isGrounded)
        {
            // Salto normal
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;

            Vector2 jumpDir = jumpDirectionInput;
            
               // Garante que tem uma componente vertical forte para o salto
            if (Mathf.Abs(jumpDir.y) < 0.1f)
            {
                jumpDir.y = 1f;
            }

        jumpDir = jumpDir.normalized;

            rb.linearVelocity = jumpDir.normalized * doubleJumpForce;
        }
    }


    
      void CheckGrounded()
    {
        // Checa se o jogador está a tocar no chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
