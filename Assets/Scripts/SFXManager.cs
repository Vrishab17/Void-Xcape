using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    
    [SerializeField] private AudioSource SFXObje;
    
    [Header("Distance Settings")]
    [SerializeField] private Transform player; // Reference to player
    [SerializeField] private float maxDistance = 25f; // Max distance to hear sound
    [SerializeField] private float minDistance = 3f; // Distance for full volume
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        // Auto-find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }
    
    public float CalculateVolumeByDistance(Vector3 soundPosition)
    {
        if (player == null) return 1f; // Full volume if no player reference
        
        float distance = Vector3.Distance(player.position, soundPosition);
        
        if (distance <= minDistance)
            return 1f; // Full volume
        else if (distance >= maxDistance)
            return 0f; // No sound
        else
        {
            // Linear falloff between min and max distance
            float normalizedDistance = (distance - minDistance) / (maxDistance - minDistance);
            return Mathf.Lerp(1f, 0f, normalizedDistance);
        }
    }
    
    public void PlayClip(AudioClip audioClip, Transform spawnTransform)
    {
        if (audioClip == null)
        {
            Debug.LogError("AudioClip is null!");
            return;
        }
        if (spawnTransform == null)
        {
            Debug.LogError("SpawnTransform is null!");
            return;
        }
        if (SFXObje == null)
        {
            Debug.LogError("SFXObje (AudioSource prefab) is not assigned in the inspector!");
            return;
        }
        
        // Calculate distance-based volume
        float volume = CalculateVolumeByDistance(spawnTransform.position);
        
        // Don't play if too far away
        if (volume <= 0.01f) return;
        
        AudioSource audioSource = Instantiate(SFXObje, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume; // Set volume based on distance
        audioSource.Play();
        
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
}