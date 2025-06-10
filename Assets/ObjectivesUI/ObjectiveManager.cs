using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    [System.Serializable]
    public class Objective
    {
        public string description;
        public bool isComplete = false;
    }

    public List<Objective> objectives = new List<Objective>();
    public GameObject objectiveSlotPrefab;
    public Transform objectiveParent;

    private GameObject currentSlot;
    private int currentIndex = 0;

    void Start()
    {
        ShowNextObjective();
    }

    public void CompleteCurrentObjective()
    {
        if (currentSlot == null) return;

        ObjectiveSlot slot = currentSlot.GetComponent<ObjectiveSlot>();
        if (slot != null)
        {
            slot.checkBoxImage.sprite = slot.checkedCheckboxSprite;
            slot.CompleteObjective(); // triggers fade-out
        }

        StartCoroutine(NextObjectiveAfterDelay());
    }

    private IEnumerator NextObjectiveAfterDelay()
    {
        yield return new WaitForSeconds(1.5f); // match fade time
        currentIndex++;
        ShowNextObjective();
    }

    private void ShowNextObjective()
    {
        if (currentIndex >= objectives.Count) return;

        currentSlot = Instantiate(objectiveSlotPrefab, objectiveParent);

        ObjectiveSlot slot = currentSlot.GetComponent<ObjectiveSlot>();
        if (slot != null)
        {
            // Special case for dynamic tracking (e.g. Defeat enemies objective)
            if (currentIndex == 1) // index 1 = Defeat enemies
            {
                slot.objectiveText.text = $"{objectives[currentIndex].description} (0/6)";
            }
            else
            {
                slot.objectiveText.text = objectives[currentIndex].description;
            }

            slot.checkBoxImage.sprite = slot.emptyCheckboxSprite;
        }

        // Optional fade-in setup
        CanvasGroup cg = currentSlot.GetComponent<CanvasGroup>();
        if (cg != null) cg.alpha = 1;
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }

    public Objective GetCurrentObjective()
    {
        if (currentIndex < objectives.Count)
            return objectives[currentIndex];
        return null;
    }

    public ObjectiveSlot GetCurrentSlot()
    {
        return currentSlot != null ? currentSlot.GetComponent<ObjectiveSlot>() : null;
    }
}
