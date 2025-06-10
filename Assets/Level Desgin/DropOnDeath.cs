using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [Header("Drop Settings")]
    public GameObject dropPrefab;
    public bool shouldDrop = true;

    public void DropItem(Vector3 position)
    {
        if (shouldDrop && dropPrefab != null)
        {
            Instantiate(dropPrefab, position, Quaternion.identity);
        }
    }
}
