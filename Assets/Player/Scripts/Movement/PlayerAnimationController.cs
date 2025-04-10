using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement movement;
    public int crouchLayerIndex = 1;
    public float layerBlendSpeed = 8f;

    void Update()
    {
        if (animator == null || movement == null) return;

        animator.SetBool("isWalking", movement.IsWalking);
        animator.SetBool("isRunning", movement.IsRunning);
        animator.SetBool("isCrouching", movement.IsCrouching);

        float targetWeight = movement.IsCrouching ? 1f : 0f;
        float currentWeight = animator.GetLayerWeight(crouchLayerIndex);
        float blendedWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * layerBlendSpeed);
        animator.SetLayerWeight(crouchLayerIndex, blendedWeight);

        movement.CrouchLayerWeight = blendedWeight;
    }
}