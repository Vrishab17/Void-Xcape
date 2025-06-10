using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GameObject playerGun; // The gun in the scene

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerGun != null)
                playerGun.SetActive(true);

            
            ObjectiveManager manager = FindObjectOfType<ObjectiveManager>();
            if (manager != null)
                manager.CompleteCurrentObjective();

            {
                playerGun.SetActive(true);

                WeaponManager wm = other.GetComponentInChildren<WeaponManager>();
                if (wm != null)
                {
                    wm.AddWeapon(playerGun);
                }
            }

            Destroy(gameObject);
        }
    }
}
