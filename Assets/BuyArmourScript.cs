using UnityEngine;
using TMPro;

public class ArmourButtonShop : MonoBehaviour
{
    public float armourAmount = 20f;
    public int cost = 10;

    public PlayerHealth playerHealth;
    public TextMeshProUGUI messageText;

    public void BuyArmour()
    {
        if (playerHealth.currentArmour >= playerHealth.maxArmour)
        {
            ShowMessage("Armour is already full");
            return;
        }

        if (CoinManager.Instance.coins < cost)
        {
            ShowMessage("Not enough coins");
            return;
        }

        // Apply armour and deduct coins
        playerHealth.AddArmour(armourAmount);
        CoinManager.Instance.coins -= cost;
        CoinManager.Instance.SendMessage("UpdateCoinUI");
        ShowMessage("Armour increased");
    }

    void ShowMessage(string msg)
    {
        if (messageText == null) return;

        messageText.text = msg;
        CancelInvoke(nameof(ClearMessage));
        Invoke(nameof(ClearMessage), 2f);
    }

    void ClearMessage()
    {
        messageText.text = "";
    }
}
