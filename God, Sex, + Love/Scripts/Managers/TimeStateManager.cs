using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeStateManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] TMP_Text timeStateText;
    [SerializeField] TMP_Text AP_Text;

    OverviewManager overviewManager;
    CharLocationManager charLocationManager;

    public enum DayPart
    {
        Morning,
        Midday,
        Evening,
        Night
    }

    public DayPart currentDayPart = DayPart.Morning;
    int dayNumber = 1;
    public int AP = 2;

    // This manager is a singleton
    public static TimeStateManager Instance { get; private set; }


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

    void Start()
    {
        overviewManager = GetComponent<OverviewManager>();
        charLocationManager = GetComponent<CharLocationManager>();
    }

    public void ConsumeAP()
    {
        AP--;

        if (AP == 1)
            UpdateAP_UI();
        else
        {
            AP = 2;
            AdvanceDayPart();
        }
    }

    void AdvanceDayPart()
    {
        currentDayPart = (DayPart)(((int)currentDayPart + 1) % 4); // Cycle through enum values
        // Increment day if applicable
        UpdateDayUI();
        UpdateAP_UI();
        UpdateLighting();

        Debug.Log("DayPart advanced;");
        // Update Char Locations
        charLocationManager.UpdatCharLocations(currentDayPart);
        overviewManager.UpdateCharLocationTexts(currentDayPart);
    }

    void UpdateAP_UI()
    {
        AP_Text.SetText("(" + AP.ToString() + " AP)");
    }

    void UpdateDayUI()
    {
        timeStateText.SetText("Day " + dayNumber + ": " + currentDayPart.ToString());
    }

    void UpdateLighting()
    {
        switch (currentDayPart)
        {
            case DayPart.Morning:
                // Handle morning logic (e.g., change lighting, sound effects)
                break;
            case DayPart.Midday:
                // Handle midday logic
                break;
            case DayPart.Evening:
                // Handle evening logic
                break;
            case DayPart.Night:
                // Handle night logic
                break;
        }
    }

    public void SaveData(GameData data)
    {
        data.DayNumber = dayNumber;
        data.DayPart = (int)currentDayPart;
        data.AP = AP;
    }

    public void LoadData(GameData data)
    {
        dayNumber = data.DayNumber;
        currentDayPart = (DayPart)data.DayPart;
        AP = data.AP;
    }
}
