using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    AudioSource audioSource;

    // This Manager Class is a singleton
    public static SoundManger Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            Debug.Log("Found more than one SoundManager in the scene. Duplicates were destroyed.");
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }
}
