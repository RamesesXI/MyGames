using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueText;

    [Header("Navigation")]
    [SerializeField] List<GameObject> areas = new List<GameObject>();
    [NonSerialized] public GameObject ActiveArea;
    [NonSerialized] public int currentAreaIdx; // Index of the active Area Illustration // Not sure if useful or redundant

    public bool navAllowed;

    // This manager is a singleton
    public static AreaManager Instance { get; private set; }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            Debug.Log("Found more than one AreaManager in the scene. Duplicates were destroyed.");
            return;
        }

        Instance = this;
    }

    void Start()
    {
        ActiveArea = areas[0]; // Default initializer to avoid null exception. Probably not needed
    }

    public void EnterNavMode(int areaIdx)
    {
        // Set passed area active
        ActiveArea = areas[areaIdx];
        ActiveArea.SetActive(true);
        
        currentAreaIdx = areaIdx;

    }

    public void SwitchArea(int areaIdx)
    {
        // Switch active area
        ActiveArea.SetActive(false);
        ActiveArea = areas[areaIdx];
        ActiveArea.SetActive(true);

        currentAreaIdx = areaIdx;
    }

}
