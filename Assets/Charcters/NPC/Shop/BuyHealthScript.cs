using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthButtonShop : MonoBehaviour
{
    public float healAmount = 20f;
    public int cost = 10;

    public PlayerHealth playerHealth; 
    public TextMeshProUGUI messageText;
          

    public void BuyHealth()
    {
        if (playerHealth.currentHealth >= playerHealth.maxHealth)
        {
            ShowMessage("Health is already full");
            return;
        }

        if (CoinManager.Instance.coins < cost)
        {
            ShowMessage("Not enough coins");
            return;
        }

        // Apply healing and deduct coins
        playerHealth.Heal(healAmount);
        CoinManager.Instance.coins -= cost;
        CoinManager.Instance.SendMessage("UpdateCoinUI"); // Update coin display
        ShowMessage("Healed 20 HP");
    }

    void ShowMessage(string msg)
{
    if (messageText == null) return;

    messageText.text = msg;
    messageText.gameObject.SetActive(true);

    CancelInvoke(nameof(ClearMessage));
    Invoke(nameof(ClearMessage), 2f); 
}

void ClearMessage()
{
    messageText.gameObject.SetActive(false);
}

}
