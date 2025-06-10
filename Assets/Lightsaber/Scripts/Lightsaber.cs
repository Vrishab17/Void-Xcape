using UnityEngine;

public class Lightsaber : MonoBehaviour
{
    public Animator animator;
    public int damage = 25;
    public float swingCooldown = 0.6f;
    public AudioSource swingSound;

    private bool canSwing = true;

    void OnEnable()
    {
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSwing && gameObject.activeSelf)
        {
            Swing();
        }
    }

    void Swing()
    {
        if (animator != null)
            animator.SetTrigger("Swing");

        if (swingSound != null)
            swingSound.Play();

        canSwing = false;
        Invoke(nameof(ResetSwing), swingCooldown);
    }

    void ResetSwing()
    {
        canSwing = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canSwing || !gameObject.activeSelf) return;

        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
