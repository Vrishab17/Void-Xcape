using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ObjectiveType
{
    Basic,
    KillEnemies,
    KillBoss,
    CollectKeycards
}

public class ObjectiveManager : MonoBehaviour
{
    [System.Serializable]
    public class Objective
    {
        public string description;
        public ObjectiveType type = ObjectiveType.Basic;
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
            var obj = objectives[currentIndex];

            switch (obj.type)
            {
                case ObjectiveType.CollectKeycards:
                    if (ItemCollectionTracker.Instance != null)
                    {
                        int current = ItemCollectionTracker.Instance.GetCollectedCount();
                        int total = ItemCollectionTracker.Instance.requiredCount;
                        slot.objectiveText.text = $"Find key cards ({current}/{total})";
                    }
                    else
                    {
                        slot.objectiveText.text = obj.description;
                    }
                    break;

                case ObjectiveType.KillEnemies:
                    slot.objectiveText.text = $"{obj.description} (0/6)";
                    break;

                default:
                    slot.objectiveText.text = obj.description;
                    break;
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
