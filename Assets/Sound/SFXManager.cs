using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance; //
    [SerializeField] private AudioSource SFXObje;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

        AudioSource audioSource = Instantiate(SFXObje, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
}
