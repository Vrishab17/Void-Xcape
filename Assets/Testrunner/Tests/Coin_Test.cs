using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CoinManagerTest
{
    private GameObject coinManagerObject;
    private CoinManager coinManager;
    private Text coinText;

    [UnityTest]
    public IEnumerator CoinText_Updates_When_AddCoins_Called()
    {
        // Create and set up CoinManager
        coinManagerObject = new GameObject("CoinManager");
        coinManager = coinManagerObject.AddComponent<CoinManager>();

        // Create and assign a Text UI element
        GameObject textObj = new GameObject("CoinText");
        coinText = textObj.AddComponent<Text>();
        coinManager.coinText = coinText;

        // Add coins
        coinManager.AddCoins(10);

        // Wait a frame in case Unity UI needs to update
        yield return null;

        // Validation check: UI matches coin count
        Assert.AreEqual("10", coinManager.coinText.text);
    }
}