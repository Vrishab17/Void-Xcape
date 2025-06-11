using UnityEngine;

public class ShuttleMoveForward : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float startDelay = 1.5f;

    private bool move = false;

    void Start()
    {
        Invoke(nameof(StartMoving), startDelay);
    }

    void StartMoving()
    {
        move = true;
    }

    void Update()
    {
        if (move)
        {
            // Move in local forward direction
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
