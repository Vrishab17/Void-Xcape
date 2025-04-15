using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float crouchSpeed = 2f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float transitionDownSpeed = 6f;
    public float transitionUpSpeed = 10f;

    public Transform armsTransform;

    public Vector3 armsOffsetIdle = new Vector3(0f, 0f, 0f);
    public Vector3 armsOffsetWalk = new Vector3(0f, -0.1f, 0.05f);
    public Vector3 armsOffsetRun = new Vector3(0f, -0.15f, 0.1f);
    public Vector3 armsOffsetCrouchIdle = new Vector3(0f, -0.4f, 0f);
    public Vector3 armsOffsetCrouchWalk = new Vector3(0f, -0.3f, 0.1f);
    public Vector3 armsScaleCrouch = new Vector3(0.9f, 0.9f, 0.9f);

    private Vector3 originalArmsLocalPos;
    private Vector3 originalArmsLocalScale;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    public bool IsCrouching { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsWalking { get; private set; }
    public float CrouchLayerWeight { get; set; }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (armsTransform != null)
        {
            originalArmsLocalPos = armsTransform.localPosition;
            originalArmsLocalScale = armsTransform.localScale;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        IsCrouching = Input.GetKey(KeyCode.LeftControl);
        IsWalking = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;
        IsRunning = IsWalking && Input.GetKey(KeyCode.LeftShift);


        float speed = IsRunning ? runSpeed : walkSpeed;
        if (IsCrouching) speed = crouchSpeed;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 flatMovement = (forward * moveZ + right * moveX) * speed;

        float y = moveDirection.y;

        if (controller.isGrounded)
        {
            moveDirection = flatMovement;
            if (Input.GetButton("Jump") && !IsCrouching)
                moveDirection.y = jumpForce;
        }
        else
        {
            moveDirection = flatMovement;
            moveDirection.y = y;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        float currentHeight = controller.height;
        float targetHeight = IsCrouching ? crouchHeight : standingHeight;
        float transitionSpeed = CrouchLayerWeight >= 0.5f ? transitionDownSpeed : transitionUpSpeed;
        controller.height = Mathf.Lerp(currentHeight, targetHeight, transitionSpeed * Time.deltaTime);
        float heightDiff = controller.height - currentHeight;
        controller.center += new Vector3(0f, heightDiff / 2f, 0f);

        if (armsTransform != null)
        {
            Vector3 targetOffset = armsOffsetIdle;

            if (IsCrouching)
                targetOffset = IsWalking ? armsOffsetCrouchWalk : armsOffsetCrouchIdle;
            else if (IsRunning && IsWalking)
                targetOffset = armsOffsetRun;
            else if (IsWalking)
                targetOffset = armsOffsetWalk;

            Vector3 targetArmsPos = originalArmsLocalPos + targetOffset;
            armsTransform.localPosition = Vector3.Lerp(armsTransform.localPosition, targetArmsPos, transitionSpeed * Time.deltaTime);

            Vector3 targetScale = IsCrouching ? armsScaleCrouch : originalArmsLocalScale;
            armsTransform.localScale = Vector3.Lerp(armsTransform.localScale, targetScale, transitionSpeed * Time.deltaTime);
        }
    }
}