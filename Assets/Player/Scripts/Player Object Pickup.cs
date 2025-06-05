using UnityEngine;
using UnityEngine.UI;

public class PlayerObjectPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Camera playerCamera;
    public Transform holdPoint;
    public float pickupRange = 3f;
    public KeyCode pickupKey = KeyCode.E;
    public float moveForce = 500f;

    [Header("Highlight")]
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer lastRenderer;

    [Header("UI")]
    public GameObject pickupPromptUI; // Assign a UI Text GameObject in Canvas

    private GameObject heldObject;
    private Rigidbody heldRB;

    void Update()
    {
        if (heldObject == null)
        {
            HighlightObjectInView();

            if (Input.GetKeyDown(pickupKey))
                TryPickup();
        }
        else
        {
            if (Input.GetKeyDown(pickupKey))
                Drop();
        }
    }

    void FixedUpdate()
    {
        if (heldObject != null && heldRB != null)
        {
            Vector3 toTarget = holdPoint.position - heldObject.transform.position;
            float distance = toTarget.magnitude;
            heldRB.linearVelocity = toTarget.normalized * moveForce * Time.fixedDeltaTime * distance;
        }
    }

    void HighlightObjectInView()
    {
        if (lastRenderer != null)
        {
            lastRenderer.material = originalMaterial;
            lastRenderer = null;
            pickupPromptUI.SetActive(false);
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag("Pickup"))
            {
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    lastRenderer = renderer;
                    originalMaterial = renderer.material;
                    renderer.material = highlightMaterial;

                    pickupPromptUI.SetActive(true);
                }
            }
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag("Pickup"))
            {
                heldObject = hit.collider.gameObject;
                heldRB = heldObject.GetComponent<Rigidbody>();

                if (heldRB == null)
                {
                    heldObject = null;
                    return;
                }

                heldRB.useGravity = false;
                heldRB.linearDamping = 10f;
                heldRB.angularDamping = 10f;
                heldRB.constraints = RigidbodyConstraints.FreezeRotation;
                heldRB.interpolation = RigidbodyInterpolation.Interpolate;

                pickupPromptUI.SetActive(false);
                if (lastRenderer != null)
                {
                    lastRenderer.material = originalMaterial;
                    lastRenderer = null;
                }
            }
        }
    }

    void Drop()
    {
        if (heldObject == null || heldRB == null) return;

        heldRB.useGravity = true;
        heldRB.linearDamping = 0f;
        heldRB.angularDamping = 0.05f;
        heldRB.constraints = RigidbodyConstraints.None;

        heldObject = null;
        heldRB = null;
    }
}
