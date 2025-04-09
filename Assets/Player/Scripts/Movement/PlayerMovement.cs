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

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;
    private Vector3 originalCameraLocalPos;
    private Animator animator;

    public Transform armsTransform; // Drag the arms GameObject here in Inspector
    public Vector3 armsCrouchOffset = new Vector3(0, -0.4f, 0); // adjust as needed
    private Vector3 originalArmsLocalPos;


    private bool isCrouching = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalCameraLocalPos = playerCamera.transform.localPosition;

        if (playerBodyModel != null)
        {
            animator = playerBodyModel.GetComponent<Animator>();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalArmsLocalPos = armsTransform.localPosition;

    }

    void Update()
    {
        // --- INPUT ---
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isJumping = Input.GetButton("Jump");
        isCrouching = Input.GetKey(KeyCode.LeftControl); // hold to crouch

        bool hasMovementInput = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;

        // --- SPEED BASED ON STATE ---
        float speed = isRunning ? runSpeed : walkSpeed;
        if (isCrouching) speed = crouchSpeed;

        // --- ANIMATOR UPDATES ---
        if (animator != null)
        {
            animator.SetBool("isWalking", hasMovementInput);
            animator.SetBool("isRunning", isRunning && hasMovementInput && !isCrouching);
            animator.SetBool("isCrouching", isCrouching);

            // Control crouch animation layer weight
            animator.SetLayerWeight(1, isCrouching ? 1f : 0f); // Layer 1 is CrouchLayer
        }

        // --- MOVEMENT ---
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

        // Apply movement
        controller.Move(moveDirection * Time.deltaTime);

        // --- SMOOTH CROUCH TRANSITION ---
        float currentHeight = controller.height;
        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        controller.height = Mathf.Lerp(currentHeight, targetHeight, crouchTransitionSpeed * Time.deltaTime);
        Vector3 targetArmsPos = originalArmsLocalPos + (isCrouching ? armsCrouchOffset : Vector3.zero);
        armsTransform.localPosition = Vector3.Lerp(armsTransform.localPosition, targetArmsPos, crouchTransitionSpeed * Time.deltaTime);

        // Adjust center so feet stay grounded
        float heightDifference = controller.height - currentHeight;
        controller.center += new Vector3(0, heightDifference / 2f, 0);

        // Move camera smoothly
        Vector3 targetCameraPos = originalCameraLocalPos + new Vector3(0, isCrouching ? cameraCrouchOffset : 0, 0);
        playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, targetCameraPos, crouchTransitionSpeed * Time.deltaTime);

        // --- MOUSE LOOK ---
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
