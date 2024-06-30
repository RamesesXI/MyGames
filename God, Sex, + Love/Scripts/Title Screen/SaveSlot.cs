using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] string profileID = "";

    [NonSerialized] public Button saveSlotButton;
    GameObject DataContent;

    public bool hasData { get; private set; } = false;

    void Awake()
    {
        saveSlotButton = GetComponent<Button>();
        DataContent = transform.GetChild(0).gameObject;
    }

    public void SetData(GameData data)
    {
        // there's no data for this profileId
        if (data == null)
        {
            hasData = false;
            DataContent.SetActive(false);
        }
        // there is data for this profileId
        else
        {
            hasData = true;
            DataContent.SetActive(true);
        }
    }

    public string GetProfileID()
    {
        return profileID;
    }
}
