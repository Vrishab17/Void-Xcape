using UnityEngine;

public class BuyMenuUI : MonoBehaviour
{
    public GameObject buyMenuPanel;
    public PlayerCameraController cameraController;
    public GameObject CanvasMiniMap;

    void Start()
    {
        buyMenuPanel.SetActive(false); // Hide by default
    }

    public void OpenMenu()
    {
        buyMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Optional: Pause game

        if (cameraController != null)
            cameraController.enabled = false;

        if (CanvasMiniMap != null) CanvasMiniMap.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        buyMenuPanel.SetActive(false);
        Time.timeScale = 1f;

        if (cameraController != null) cameraController.enabled = true;

        if (CanvasMiniMap != null) CanvasMiniMap.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }
}
