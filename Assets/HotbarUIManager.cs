using UnityEngine;
using UnityEngine.UI;

public class HotbarUIManager : MonoBehaviour
{
    public Image[] slotIcons;        
    public GameObject[] highlights;   

    void Start()
    {
        foreach (var icon in slotIcons)
            icon.gameObject.SetActive(false);

        UpdateHighlight(0);
    }

    public void SetIcon(int slotIndex, Sprite sprite)
    {
        if (slotIndex < slotIcons.Length)
        {
            slotIcons[slotIndex].sprite = sprite;
            slotIcons[slotIndex].gameObject.SetActive(true);
        }
    }

    public void UpdateHighlight(int selectedIndex)
    {
        for (int i = 0; i < highlights.Length; i++)
        {
            highlights[i].SetActive(i == selectedIndex);
        }
    }
}
