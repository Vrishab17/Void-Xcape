using UnityEngine;
using UnityEngine.InputSystem;

public class PotionInventory : MonoBehaviour
{
    private GameObject equippedPotion;
    private int equippedHealAmount = 0;
    private PlayerHealth playerHealth;

    private InputSystem_Actions inputActions;

    // Awake Method
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    // Enabling input actions
    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.performed += OnInteract;
    }

    // Disabling input actions
    void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Disable();
    }

    // Start Method
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth not found on the player!");
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("E was pressed - Interact triggered!");

        if (equippedPotion != null)
        {
            Debug.Log("Trying to use potion...");
            UsePotion();
        }
        else
        {
            Debug.Log("No potion equipped.");
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
            Destroy(equippedPotion);
            equippedPotion = null;
            Debug.Log("Potion used!");
        }
        else
        {
            Debug.Log("Health is already full.");
        }
    }
}
