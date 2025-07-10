using NUnit.Framework;
using UnityEngine;

public class SoundEffects_Test
{
    private GameObject playerObject;
    private GameObject managerObject;
    private SFXManager sfxManager;

    [SetUp]
    public void Setup()
    {
        // Create player and set position
        playerObject = new GameObject("Player");
        playerObject.tag = "Player";
        playerObject.transform.position = Vector3.zero;

        // Create SFXManager and assign player
        managerObject = new GameObject("SFXManager");
        sfxManager = managerObject.AddComponent<SFXManager>();
        typeof(SFXManager).GetField("player", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(sfxManager, playerObject.transform);
    }

    [Test]
    public void Volume_IsFull_When_At_MinDistance()
    {
        Vector3 soundPos = playerObject.transform.position + Vector3.forward * 3f; // minDistance = 3
        float volume = sfxManager.CalculateVolumeByDistance(soundPos);
        Assert.AreEqual(1f, volume, 0.01f);
    }

    [Test]
    public void Volume_IsZero_When_Beyond_MaxDistance()
    {
        Vector3 soundPos = playerObject.transform.position + Vector3.forward * 30f; // maxDistance = 25
        float volume = sfxManager.CalculateVolumeByDistance(soundPos);
        Assert.AreEqual(0f, volume, 0.01f);
    }

    [Test]
    public void Volume_IsHalfWay_In_TheMiddle()
    {
        Vector3 soundPos = playerObject.transform.position + Vector3.forward * 14f;
        float volume = sfxManager.CalculateVolumeByDistance(soundPos);
        Assert.That(volume, Is.InRange(0.45f, 0.55f)); // Approximate halfway between min and max
    }
}
