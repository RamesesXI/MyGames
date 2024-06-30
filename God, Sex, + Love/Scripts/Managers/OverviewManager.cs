using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TimeStateManager;

public class OverviewManager : MonoBehaviour
{
    [SerializeField] TMP_Text[] charAffectionTexts;
    [SerializeField] TMP_Text[] charLocationTexts;

    // This manager is a singleton
    public static OverviewManager Instance { get; private set; }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            Debug.Log("Found more than one TimeStateManager in the scene. Duplicates were destroyed.");
            return;
        }

        Instance = this;
    }

    public void UpdateCharLocationTexts(TimeStateManager.DayPart dayPart)
    {
        
        switch (dayPart)
        {
            case DayPart.Morning:
                
                break;
            case DayPart.Midday:
                charLocationTexts[0].SetText("Location: Bathhouse");
                charLocationTexts[1].SetText("Location: Athena's Room");
                charLocationTexts[2].SetText("Location: Maya's Room");
                break;
            case DayPart.Evening:
                charLocationTexts[0].SetText("Location: Zendo");
                charLocationTexts[1].SetText("Location: Zendo");
                charLocationTexts[2].SetText("Location: Zendo");
                break;
            case DayPart.Night:
                // Handle night logic
                break;
        }
    }

    public void UpdateAffection(bool athena)
    {
        if (athena)
            charAffectionTexts[1].SetText("Affection: 1");
        else
            charAffectionTexts[1].SetText("Affection: 1");
    }

}
