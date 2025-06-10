using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInput : MonoBehaviour
{
    [Header("Inventory UI")]
    public GameObject inventoryUI;

    private InputSystem_Actions inputActions;
    private bool isOpen = false;

    public static bool InventoryOpen { get; private set; }
    public static bool BlockNextInput { get; private set; }

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
        InventoryOpen = isOpen;

        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
        Time.timeScale = isOpen ? 0f : 1f;

        BlockNextInput = true; // block input for one frame
    }

    private void LateUpdate()
    {
        BlockNextInput = false; // clear at end of frame
    }
}
