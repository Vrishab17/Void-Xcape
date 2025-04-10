using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Camera playerCamera;
    public PlayerMovement movement;

    [Header("Mouse Look")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Head Bob Settings")]
    public float bobFrequency = 1.5f;
    public float bobHorizontalAmplitude = 0.05f;
    public float bobVerticalAmplitude = 0.05f;

    [Header("Crouch Settings")]
    public float cameraCrouchOffset = -0.5f;

    private Vector3 originalCameraLocalPos;
    private float rotationX = 0f;
    private float headBobTimer = 0f;

    void Start()
    {
        if (playerCamera == null) playerCamera = Camera.main;
        originalCameraLocalPos = playerCamera.transform.localPosition;
    }

    void Update()
    {
        float transitionSpeed = movement.CrouchLayerWeight >= 0.5f ? movement.transitionDownSpeed : movement.transitionUpSpeed;

        Vector3 targetCameraOffset = Vector3.Lerp(Vector3.zero, new Vector3(0, cameraCrouchOffset, 0), movement.CrouchLayerWeight);
        Vector3 targetCameraPos = originalCameraLocalPos + targetCameraOffset;

        if (movement.IsWalking && !movement.IsCrouching)
        {
            headBobTimer += Time.deltaTime * bobFrequency * (movement.IsRunning ? 1.5f : 1f);
            float bobOffsetY = Mathf.Sin(headBobTimer * 2f) * bobVerticalAmplitude;
            float bobOffsetX = Mathf.Cos(headBobTimer) * bobHorizontalAmplitude;
            targetCameraPos += new Vector3(bobOffsetX, bobOffsetY, 0f);
        }
        else
        {
            headBobTimer = 0f;
        }

        playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, targetCameraPos, transitionSpeed * Time.deltaTime);

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}