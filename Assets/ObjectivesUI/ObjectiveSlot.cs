using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveSlot : MonoBehaviour
{
    public Image checkBoxImage;
    public Sprite emptyCheckboxSprite;
    public Sprite checkedCheckboxSprite;
    public TextMeshProUGUI objectiveText;

    public void CompleteObjective()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = gameObject.AddComponent<CanvasGroup>();
        }

        for (float t = 1f; t > 0; t -= Time.deltaTime)
        {
            cg.alpha = t;
            yield return null;
        }

        Destroy(gameObject);
    }
}
