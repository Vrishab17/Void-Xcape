using UnityEngine;

public class PotionInventory : MonoBehaviour
{
    private GameObject equippedPotion;
    private int equippedHealAmount = 0;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2) && equippedPotion != null)
        {
            UsePotion();
        }
    }

    public void EquipPotion(GameObject potion, int healAmount)
    {
        equippedPotion = potion;
        equippedHealAmount = healAmount;
        Debug.Log("Potion equipped.");
    }

    void UsePotion()
    {
        if (playerHealth.currentHealth < playerHealth.maxHealth)
        {
            playerHealth.Heal(equippedHealAmount);
            Destroy(equippedPotion); // Use up the potion
            equippedPotion = null;
            Debug.Log("Potion used!");
        }
        else
        {
            Debug.Log("Health is already full.");
        }
    }
}



