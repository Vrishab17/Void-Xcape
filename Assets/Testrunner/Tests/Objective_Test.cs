using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveManagerTest
{
    private GameObject managerObject;
    private ObjectiveManager manager;
    private GameObject mockPrefab;
    private GameObject mockParent;

    [SetUp]
    public void SetUp()
    {
        managerObject = new GameObject("ObjectiveManager");
        manager = managerObject.AddComponent<ObjectiveManager>();

        // Proper mock prefab setup
        mockPrefab = new GameObject("MockPrefab");
        mockPrefab.AddComponent<ObjectiveSlot>(); // must match what ShowNextObjective expects

        mockParent = new GameObject("MockParent"); // no need to add Transform

        manager.objectiveSlotPrefab = mockPrefab;
        manager.objectiveParent = mockParent.transform;

        manager.objectives = new List<ObjectiveManager.Objective>
        {
            new() { description = "First objective", isComplete = false },
            new() { description = "Second objective", isComplete = false },
            new() { description = "Third objective", isComplete = false }
        };
    }

    [TearDown]
    public void TearDown()
    {
        if (managerObject != null)
            Object.DestroyImmediate(managerObject);
        if (mockPrefab != null)
            Object.DestroyImmediate(mockPrefab);
        if (mockParent != null)
            Object.DestroyImmediate(mockParent);
    }

    [Test]
    public void StartsWithObjective()
    {
        // Assert - should start at index 0 by default
        Assert.AreEqual(0, manager.GetCurrentIndex(), "Should start with first objective (index 0)");
        Assert.AreEqual("First objective", manager.GetCurrentObjective().description);
    }

    [Test]
    public void ReturnsCorrectCurrentObjective()
    {
        // Act
        var current = manager.GetCurrentObjective();
        
        // Assert
        Assert.IsNotNull(current, "Should return current objective");
        Assert.AreEqual("First objective", current.description);
        Assert.IsFalse(current.isComplete, "Should not be complete initially");
    }

    [Test]
    public void HandlesObjectiveListCorrectly()
    {
        // Assert
        Assert.AreEqual(3, manager.objectives.Count, "Should have 3 test objectives");
        Assert.AreEqual("First objective", manager.objectives[0].description);
        Assert.AreEqual("Second objective", manager.objectives[1].description);
        Assert.AreEqual("Third objective", manager.objectives[2].description);
    }

}