using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlayerObjectPickupTest
{
    private GameObject testPlayer;
    private GameObject testPickupObject;
    private PlayerObjectPickup pickupScript;
    private Camera testCamera;
    private Transform holdPoint;

    [SetUp]
    public void SetUp()
    {
        // Create test player GameObject
        testPlayer = new GameObject("TestPlayer");
        
        // Add and configure camera
        testCamera = testPlayer.AddComponent<Camera>();
        testCamera.transform.position = Vector3.zero;
        testCamera.transform.rotation = Quaternion.identity;
        
        // Create hold point
        GameObject holdPointObj = new GameObject("HoldPoint");
        holdPointObj.transform.SetParent(testPlayer.transform);
        holdPointObj.transform.localPosition = Vector3.forward * 2f;
        holdPoint = holdPointObj.transform;
        
        // Add pickup script
        pickupScript = testPlayer.AddComponent<PlayerObjectPickup>();
        pickupScript.playerCamera = testCamera;
        pickupScript.holdPoint = holdPoint;
        pickupScript.pickupRange = 3f;
        pickupScript.moveForce = 500f;
        
        // Create test pickup object
        testPickupObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        testPickupObject.transform.position = Vector3.forward * 2f;
        testPickupObject.tag = "Pickup";
        
        // Add Rigidbody to pickup object
        Rigidbody rb = testPickupObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
    }
    
    [TearDown]
    public void TearDown()
    {
        if (testPlayer != null)
            Object.DestroyImmediate(testPlayer);
        if (testPickupObject != null)
            Object.DestroyImmediate(testPickupObject);
    }
    
    [Test]
    public void TestPickupScriptInitialization()
    {
        // Test that the script initializes properly
        Assert.IsNotNull(pickupScript);
        Assert.IsNotNull(pickupScript.playerCamera);
        Assert.IsNotNull(pickupScript.holdPoint);
        Assert.AreEqual(3f, pickupScript.pickupRange);
        Assert.AreEqual(KeyCode.E, pickupScript.pickupKey);
    }
    
    [Test]
    public void TestPickupObjectDetection()
    {
        // Position pickup object directly in front of camera
        testPickupObject.transform.position = testCamera.transform.position + testCamera.transform.forward * 2f;
        
        // Test raycast detection
        Ray ray = new Ray(testCamera.transform.position, testCamera.transform.forward);
        bool hitDetected = Physics.Raycast(ray, out RaycastHit hit, pickupScript.pickupRange);
        
        Assert.IsTrue(hitDetected, "Should detect pickup object in range");
        Assert.AreEqual("Pickup", hit.collider.tag, "Hit object should have Pickup tag");
    }
    
}