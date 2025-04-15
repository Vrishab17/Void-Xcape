using UnityEngine;

public class WeaponStabilizer : MonoBehaviour
{
    [Header("References")]
    public Transform weaponAnchor;          
    public Transform weaponModel;           
    public Transform player;                

    [Header("Weapon Position Offsets")]
    public Vector3 idleOffset = new Vector3(0f, 0f, 0f);
    public Vector3 walkOffset = new Vector3(0f, -0.03f, 0.02f);
    public Vector3 crouchIdleOffset = new Vector3(0f, -0.08f, -0.04f);
    public Vector3 crouchWalkOffset = new Vector3(0f, -0.06f, -0.02f);

    [Header("Weapon Rotation Offsets")]
    public Vector3 idleRotationOffset = Vector3.zero;
    public Vector3 walkRotationOffset = Vector3.zero;
    public Vector3 crouchIdleRotationOffset = new Vector3(5f, 0f, 0f);  // slight tilt
    public Vector3 crouchWalkRotationOffset = new Vector3(3f, 1f, 0f);  // subtle motion

    [Header("Smooth Settings")]
    public float positionSmoothSpeed = 10f;
    public float rotationSmoothSpeed = 10f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 defaultLocalPos;
    private Quaternion defaultLocalRot;

    private PlayerMovement playerMovement;

    void Start()
    {
        if (weaponAnchor == null)
            Debug.LogError("WeaponAnchor is not assigned!");

        if (weaponModel == null)
            Debug.LogError("WeaponModel is not assigned!");

        if (player == null)
            Debug.LogError("Player reference not assigned!");

        playerMovement = player.GetComponent<PlayerMovement>();
        defaultLocalPos = weaponModel.localPosition;
        defaultLocalRot = weaponModel.localRotation;
    }

    void LateUpdate()
    {
        if (playerMovement == null) return;

        Vector3 targetOffset = idleOffset;
        Vector3 targetRotOffset = idleRotationOffset;

        if (playerMovement.IsCrouching)
        {
            if (playerMovement.IsWalking)
            {
                targetOffset = crouchWalkOffset;
                targetRotOffset = crouchWalkRotationOffset;
            }
            else
            {
                targetOffset = crouchIdleOffset;
                targetRotOffset = crouchIdleRotationOffset;
            }
        }
        else if (playerMovement.IsRunning && playerMovement.IsWalking)
        {
            targetOffset = walkOffset;
            targetRotOffset = walkRotationOffset;
        }
        else if (playerMovement.IsWalking)
        {
            targetOffset = walkOffset;
            targetRotOffset = walkRotationOffset;
        }

        // Smooth position transition
        Vector3 targetPos = defaultLocalPos + targetOffset;
        weaponModel.localPosition = Vector3.SmoothDamp(weaponModel.localPosition, targetPos, ref velocity, 1f / positionSmoothSpeed);

        // Smooth rotation transition
        Quaternion targetRot = defaultLocalRot * Quaternion.Euler(targetRotOffset);
        weaponModel.localRotation = Quaternion.Slerp(weaponModel.localRotation, targetRot, Time.deltaTime * rotationSmoothSpeed);
    }
}
