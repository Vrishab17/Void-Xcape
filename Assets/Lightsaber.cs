using UnityEngine;

public class Lightsaber : MonoBehaviour
{
    public int damage = 25;
    public AudioSource swingSound;
    private bool canSwing = true;
    public float swingCooldown = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSwing)
        {
            Swing();
        }
    }

    void Swing()
    {
        if (swingSound) swingSound.Play();
        canSwing = false;
        Invoke(nameof(ResetSwing), swingCooldown);
    }

    void ResetSwing()
    {
        canSwing = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponent<EnemyHealth>();
        if (canSwing && health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
