using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInput : MonoBehaviour
{
    public GameObject inventoryUI;
    private InputSystem_Actions inputActions;
    private bool isOpen = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Inventory.performed += OnInventoryToggle;
    }

    private void OnDisable()
    {
        inputActions.Player.Inventory.performed -= OnInventoryToggle;
        inputActions.Disable();
    }

    private void OnInventoryToggle(InputAction.CallbackContext context)
    {
        isOpen = !isOpen;
        inventoryUI.SetActive(isOpen);
        
        if (isOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Debug.Log("Inventory is" + (isOpen ? "open" : "closed"));
    }
}
