using UnityEngine;
using UnityEngine.InputSystem;

public class PotionInventory : MonoBehaviour
{
    private InventorySlot equippedSlot;
    private PlayerHealth playerHealth;
    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.performed += OnInteract;
    }

    void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Disable();
    }

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
            Debug.LogError("PlayerHealth not found on FirstPerson!");
    }


    public void EquipPotion(InventorySlot slot)
    {
        equippedSlot = slot;
        Debug.Log("Potion equipped in slot.");
    }

    
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (equippedSlot != null && equippedSlot.HasItem() && equippedSlot.item is Potion potion)
        {
            if (playerHealth.currentHealth < playerHealth.maxHealth)
            {
                potion.Use();               
                equippedSlot.RemoveItem(1); 
                Debug.Log("Equipped potion used.");
            }
            else
            {
                Debug.Log("Health is full. Cannot use potion.");
            }
        }
        else
        {
            Debug.Log("No potion is currently equipped.");
        }
    }
}
