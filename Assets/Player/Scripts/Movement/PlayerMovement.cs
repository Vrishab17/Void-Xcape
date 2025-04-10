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
    public float cameraCrouchOffset = -0.4f;
    public float crouchTransitionSpeed = 6f;

    [Header("First-Person Arms Settings")]
    public Transform armsTransform;
    public Vector3 armsCrouchOffsetIdle = new Vector3(0, -0.4f, 0f);
    public Vector3 armsCrouchOffsetWalk = new Vector3(0, -0.3f, 0.1f);
    public Vector3 armsCrouchScale = new Vector3(0.9f, 0.9f, 0.9f);

    [Header("Crouch Animation Delay (only on uncrouch)")]
    public float crouchAnimDelay = 0.2f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;
    private Animator animator;

    private bool isCrouching = false;
    private Vector3 originalCameraLocalPos;
    private Vector3 originalArmsLocalPos;
    private Vector3 originalArmsLocalScale;

    private bool animatorIsCrouching = false;
    private float crouchAnimTimer = 0f;

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
        bool crouchHeld = Input.GetKey(KeyCode.LeftControl);

        bool hasMovementInput = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;

        // Crouch state (instant logic)
        if (crouchHeld && !isCrouching)
        {
            isCrouching = true;
        }
        else if (!crouchHeld && isCrouching)
        {
            isCrouching = false;
        }

        // Speed logic
        float speed = isRunning ? runSpeed : walkSpeed;
        if (isCrouching) speed = crouchSpeed;

        // Animator logic
        if (animator != null)
        {
            animator.SetBool("isWalking", hasMovementInput);
            animator.SetBool("isRunning", isRunning && hasMovementInput && !isCrouching);

            // Only delay uncrouch animation
            if (isCrouching && !animatorIsCrouching)
            {
                animator.SetBool("isCrouching", true);
                animatorIsCrouching = true;
                crouchAnimTimer = 0f;
            }
            else if (!isCrouching && animatorIsCrouching)
            {
                if (crouchAnimTimer <= 0f)
                {
                    crouchAnimTimer = crouchAnimDelay;
                }
                else
                {
                    crouchAnimTimer -= Time.deltaTime;
                    if (crouchAnimTimer <= 0f)
                    {
                        animator.SetBool("isCrouching", false);
                        animatorIsCrouching = false;
                    }
                }
            }

            animator.SetLayerWeight(1, animatorIsCrouching ? 1f : 0f);
        }

        // Direction calculation
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

        // Gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Apply movement
        controller.Move(moveDirection * Time.deltaTime);

        // Smooth crouch transition
        float currentHeight = controller.height;
        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        controller.height = Mathf.Lerp(currentHeight, targetHeight, crouchTransitionSpeed * Time.deltaTime);
        float heightDiff = controller.height - currentHeight;
        controller.center += new Vector3(0, heightDiff / 2f, 0);

        // Smooth camera crouch movement
        Vector3 targetCameraPos = originalCameraLocalPos + new Vector3(0, isCrouching ? cameraCrouchOffset : 0f, 0);
        playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, targetCameraPos, crouchTransitionSpeed * Time.deltaTime);

        // Arms crouch movement
        if (armsTransform != null)
        {
            Vector3 offset = isCrouching
                ? (hasMovementInput ? armsCrouchOffsetWalk : armsCrouchOffsetIdle)
                : Vector3.zero;

            Vector3 targetArmsPos = originalArmsLocalPos + offset;
            armsTransform.localPosition = Vector3.Lerp(armsTransform.localPosition, targetArmsPos, crouchTransitionSpeed * Time.deltaTime);

            Vector3 targetScale = isCrouching ? armsCrouchScale : originalArmsLocalScale;
            armsTransform.localScale = Vector3.Lerp(armsTransform.localScale, targetScale, crouchTransitionSpeed * Time.deltaTime);
        }

        // Mouse look
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
