using UnityEngine;

public class ShopKeeperScript : MonoBehaviour
{
    [Header("UI References")]
    public GameObject menuUI;
    public GameObject talkPromptUI;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange)
        {
            if (!talkPromptUI.activeSelf)
                talkPromptUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Hide the prompt before freezing time
                talkPromptUI.SetActive(false);

                menuUI.SetActive(true);
                CoinManager.Instance.SendMessage("UpdateCoinUI");
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            menuUI.SetActive(false);
            talkPromptUI.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
