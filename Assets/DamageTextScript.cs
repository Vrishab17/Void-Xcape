using UnityEngine;
using TMPro;

public class DamageTextSpawner : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public Canvas worldCanvas;

    public void ShowDamage(float amount, Vector3 hitPosition)
    {
        if (damageTextPrefab == null || worldCanvas == null)
        {
            Debug.LogError("❌ Missing prefab or canvas reference!");
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
            Debug.LogError("❌ TMP component not found on DamageText prefab.");
        }

        Destroy(textGO, 1.5f); // auto-destroy after 1.5 seconds
    }
}
