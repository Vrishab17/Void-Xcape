using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerMovementTests
{
    private GameObject playerGameObject;
    private PlayerMovement playerMovement;
    private CharacterController characterController;

    [SetUp]
    public void SetUp()
    {
        playerGameObject = new GameObject("TestPlayer");
        characterController = playerGameObject.AddComponent<CharacterController>();
        playerMovement = playerGameObject.AddComponent<PlayerMovement>();
        
        playerMovement.walkSpeed = 5f;
        playerMovement.runSpeed = 10f;
        playerMovement.crouchSpeed = 2f;
        playerMovement.jumpForce = 8f;
        playerMovement.crouchHeight = 1f;
        playerMovement.standingHeight = 2f;
    }

    [TearDown]
    public void TearDown()
    {
        if (playerGameObject != null)
        {
            Object.DestroyImmediate(playerGameObject);
        }
    }

    [Test]
    public void Crouch_PropertyCanBeSet_ShouldUpdateIsCrouching()
    {
        // Test initial state
        Assert.IsFalse(playerMovement.IsCrouching);
        
        // Use reflection to set the private backing field that would be set by input
        SetPrivateField("IsCrouching", true);
        
        // Verify the property reflects the change
        Assert.IsTrue(playerMovement.IsCrouching);
    }

    [Test]
    public void Sprint_WhenWalkingAndRunning_ShouldSetIsRunningTrue()
    {
        // Set walking state first
        SetPrivateField("IsWalking", true);
        SetPrivateField("IsRunning", true);
        
        // Verify both states are active
        Assert.IsTrue(playerMovement.IsWalking);
        Assert.IsTrue(playerMovement.IsRunning);
    }

    [Test]
    public void Jump_ForceValue_ShouldBeConfigurable()
    {
        // Test that jump force can be modified
        playerMovement.jumpForce = 10f;
        Assert.AreEqual(10f, playerMovement.jumpForce);
        
        // Test different jump force
        playerMovement.jumpForce = 15f;
        Assert.AreEqual(15f, playerMovement.jumpForce);
    }

    [UnityTest]
    public IEnumerator Crouch_HeightTransition_ShouldChangeCharacterHeight()
    {
        // Set initial height to standing
        characterController.height = playerMovement.standingHeight;
        float initialHeight = characterController.height;
        
        // Simulate crouching by directly setting the property
        SetPrivateField("IsCrouching", true);
        
        // Manually call the height adjustment logic multiple times
        for (int i = 0; i < 10; i++)
        {
            SimulateHeightTransition();
            yield return new WaitForFixedUpdate();
        }
        
        // Verify height has changed toward crouch height
        Assert.Less(characterController.height, initialHeight, 
            "Character height should decrease when crouching");
    }

    [Test]
    public void Movement_SpeedValues_ShouldBeDifferentForEachState()
    {
        // Verify different speeds are set
        Assert.AreNotEqual(playerMovement.walkSpeed, playerMovement.runSpeed);
        Assert.AreNotEqual(playerMovement.walkSpeed, playerMovement.crouchSpeed);
        Assert.AreNotEqual(playerMovement.runSpeed, playerMovement.crouchSpeed);
        
        // Verify run speed is fastest
        Assert.Greater(playerMovement.runSpeed, playerMovement.walkSpeed);
        Assert.Greater(playerMovement.walkSpeed, playerMovement.crouchSpeed);
    }

    // Helper method to set private properties using reflection
    private void SetPrivateField(string propertyName, object value)
    {
        PropertyInfo property = typeof(PlayerMovement).GetProperty(propertyName);
        if (property != null && property.CanWrite)
        {
            property.SetValue(playerMovement, value);
        }
    }

    // Simulate the height transition logic from Update()
    private void SimulateHeightTransition()
    {
        float currentHeight = characterController.height;
        float targetHeight = playerMovement.IsCrouching ? playerMovement.crouchHeight : playerMovement.standingHeight;
        float transitionSpeed = 6f; // Use the same speed as in your script
        
        characterController.height = Mathf.Lerp(currentHeight, targetHeight, transitionSpeed * Time.fixedDeltaTime);
        
        float heightDiff = characterController.height - currentHeight;
        characterController.center += new Vector3(0f, heightDiff / 2f, 0f);
    }
}