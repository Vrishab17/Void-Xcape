using UnityEngine;

public class CameraFixedFollow : MonoBehaviour
{
    [Header("References")]
    public Transform spaceShuttle;

    private Vector3 offset;
    private Quaternion initialRotation;

    void Start()
    {
        if (spaceShuttle == null)
        {
            Debug.LogError("Space Shuttle not assigned.");
            enabled = false;
            return;
        }

        // Store the initial offset and rotation
        offset = transform.position - spaceShuttle.position;
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Maintain exact offset and rotation
        transform.position = spaceShuttle.position + offset;
        transform.rotation = initialRotation;
    }
}
