using UnityEngine;

public class LightsaberPickup : MonoBehaviour
{
    public GameObject lightsaberObject;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (lightsaberObject != null)
            {
                lightsaberObject.SetActive(true);

                WeaponManager wm = other.GetComponentInChildren<WeaponManager>();
                if (wm != null)
                {
                    wm.AddWeapon(lightsaberObject);
                }
            }

            Destroy(gameObject);
        }
    }
}
