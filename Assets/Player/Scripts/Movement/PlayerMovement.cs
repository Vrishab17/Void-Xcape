using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject playerBodyModel;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float crouchSpeed = 2f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    [Header("Mouse Look")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float cameraCrouchOffset = -0.5f;
    public float crouchTransitionSpeed = 6f;

    [Header("First-Person Arms Settings")]
    public Transform armsTransform;

    public Vector3 armsOffsetIdle = new Vector3(0f, 0f, 0f);
    public Vector3 armsOffsetWalk = new Vector3(0f, -0.1f, 0.05f);
    public Vector3 armsOffsetRun = new Vector3(0f, -0.15f, 0.1f);
    public Vector3 armsOffsetCrouchIdle = new Vector3(0f, -0.4f, 0f);
    public Vector3 armsOffsetCrouchWalk = new Vector3(0f, -0.3f, 0.1f);
    public Vector3 armsScaleCrouch = new Vector3(0.9f, 0.9f, 0.9f);

    private Vector3 originalCameraLocalPos;
    private Vector3 originalArmsLocalPos;
    private Vector3 originalArmsLocalScale;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;

    private Animator animator;
    private bool isCrouching = false;

    // Layer blending
    private float crouchLayerBlendSpeed = 8f; // how fast to blend
    private float crouchLayerTargetWeight = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalCameraLocalPos = playerCamera.transform.localPosition;

        if (armsTransform != null)
        {
            originalArmsLocalPos = armsTransform.localPosition;
            originalArmsLocalScale = armsTransform.localScale;
        }

        if (playerBodyModel != null)
        {
            animator = playerBodyModel.GetComponent<Animator>();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Input
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isJumping = Input.GetButton("Jump");
        isCrouching = Input.GetKey(KeyCode.LeftControl); // Hold to crouch

        bool hasMovementInput = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;

        // Determine speed
        float speed = isRunning ? runSpeed : walkSpeed;
        if (isCrouching) speed = crouchSpeed;

        // Animator states
        if (animator != null)
        {
            animator.SetBool("isWalking", hasMovementInput);
            animator.SetBool("isRunning", isRunning && hasMovementInput && !isCrouching);
            animator.SetBool("isCrouching", isCrouching);

            // Smooth crouch layer blending
            crouchLayerTargetWeight = isCrouching ? 1f : 0f;
            float currentWeight = animator.GetLayerWeight(1);
            float newWeight = Mathf.Lerp(currentWeight, crouchLayerTargetWeight, Time.deltaTime * crouchLayerBlendSpeed);
            animator.SetLayerWeight(1, newWeight);
        }

        // Movement direction
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 flatMovement = (forward * moveZ + right * moveX) * speed;

        float y = moveDirection.y;

        if (controller.isGrounded)
        {
            moveDirection = flatMovement;

            if (isJumping && !isCrouching)
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

        // Smooth crouch height + center
        float currentHeight = controller.height;
        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        controller.height = Mathf.Lerp(currentHeight, targetHeight, crouchTransitionSpeed * Time.deltaTime);
        float heightDiff = controller.height - currentHeight;
        controller.center += new Vector3(0f, heightDiff / 2f, 0f);

        // Smooth camera movement
        // Sync camera transition with crouch layer blend
        if (animator != null)
        {
            float crouchWeight = animator.GetLayerWeight(1); // Direct sync
            Vector3 syncedCameraOffset = Vector3.Lerp(Vector3.zero, new Vector3(0, cameraCrouchOffset, 0), crouchWeight);
            playerCamera.transform.localPosition = originalCameraLocalPos + syncedCameraOffset;
        }
        // Arms position and scale
        if (armsTransform != null)
        {
            Vector3 targetOffset = armsOffsetIdle;

            if (isCrouching)
            {
                targetOffset = hasMovementInput ? armsOffsetCrouchWalk : armsOffsetCrouchIdle;
            }
            else if (isRunning && hasMovementInput)
            {
                targetOffset = armsOffsetRun;
            }
            else if (hasMovementInput)
            {
                targetOffset = armsOffsetWalk;
            }

            Vector3 targetArmsPos = originalArmsLocalPos + targetOffset;
            armsTransform.localPosition = Vector3.Lerp(armsTransform.localPosition, targetArmsPos, crouchTransitionSpeed * Time.deltaTime);

            Vector3 targetScale = isCrouching ? armsScaleCrouch : originalArmsLocalScale;
            armsTransform.localScale = Vector3.Lerp(armsTransform.localScale, targetScale, crouchTransitionSpeed * Time.deltaTime);
        }

        // Mouse look
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
