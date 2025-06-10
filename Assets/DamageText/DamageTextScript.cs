using UnityEngine;
using TMPro;

public class DamageTextSpawner : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public Canvas worldCanvas;

    private void Awake()
    {
        // Automatically assign the canvas if not set in the Inspector
        if (worldCanvas == null)
        {
            worldCanvas = FindObjectOfType<Canvas>();
            if (worldCanvas == null)
            {
                Debug.LogError("No Canvas found in the scene. Please assign one to DamageTextSpawner.");
            }
        }
    }

    public void ShowDamage(float amount, Vector3 hitPosition)
    {
        if (damageTextPrefab == null || worldCanvas == null)
        {
            Debug.LogError("Missing prefab or canvas reference in DamageTextSpawner!");
            return;
        }

        if (Camera.main == null)
        {
            Debug.LogError("Main camera not found. Make sure your camera is tagged as 'MainCamera'.");
            return;
        }

        GameObject textGO = Instantiate(damageTextPrefab, worldCanvas.transform);
        textGO.transform.position = Camera.main.WorldToScreenPoint(hitPosition);

        TextMeshProUGUI tmp = textGO.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.text = amount.ToString("F0");
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found in the damageTextPrefab.");
        }

        Destroy(textGO, 1.5f);
    }
}
