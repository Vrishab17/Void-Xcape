using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject playerBodyModel1; // <- drag your full-body model here in the inspector
    public GameObject playerBodyModel2;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    [Header("Mouse Look")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    private Animator animator1;
    private Animator animator2;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Get animator from the full-body model (not this GameObject)
        if (playerBodyModel1 != null)
        {
            animator1 = playerBodyModel1.GetComponent<Animator>();
        }
        if (playerBodyModel1 != null)
        {
            animator2 = playerBodyModel2.GetComponent<Animator>();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get input
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isJumping = Input.GetButton("Jump");

        // Check for movement input
        bool hasMovementInput = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;

        // Update animator states
        if (animator1 != null)
        {
            animator1.SetBool("isWalking", hasMovementInput);

        }
        if (animator2 != null)
        {
            animator2.SetBool("isWalking", hasMovementInput);

        }

        // Calculate movement
        float speed = isRunning ? runSpeed : walkSpeed;
        animator1.SetBool("isRunning", isRunning);
        animator2.SetBool("isRunning", isRunning);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 flatMovement = (forward * moveZ + right * moveX) * speed;

        // Preserve vertical velocity
        float y = moveDirection.y;

        // Apply jump
        if (controller.isGrounded)
        {
            moveDirection = flatMovement;

            if (isJumping)
                moveDirection.y = jumpForce;
        }
        else
        {
            moveDirection = flatMovement;
            moveDirection.y = y;
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move character
        controller.Move(moveDirection * Time.deltaTime);

        // Mouse Look
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
