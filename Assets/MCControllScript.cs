using UnityEngine;

public class MCControllScript : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;

    public Transform playerObj;

    public Rigidbody rb;

    public float moveSpeed = 6f;

    public float rotationSpeed = 10f;

    public InputSystem_Actions input;

    public Vector2 moveInput;


    private void Awake()
    {
        input = new InputSystem_Actions();

    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }


    private void Update()
    {
        //Rotates the player object
        RotatePlayerObj();
    }

    private void FixedUpdate()
    {
         Vector3 moveDir = orientation.forward * moveInput.y + orientation.right * moveInput.x;
         rb.linearVelocity = moveDir.normalized * moveSpeed + new Vector3(0, rb.linearVelocity.y, 0);
        
    }   
     private void RotatePlayerObj()
    {
        Vector3 moveDir = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            playerObj.rotation = Quaternion.Slerp(playerObj.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }
}
