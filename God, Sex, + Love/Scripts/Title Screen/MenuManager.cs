using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject gameScreen;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject pauseMenu;

    [SerializeField] GameObject saveLoadMenu;

    [SerializeField] GameObject settingsMenu;

    [SerializeField] SaveSlot[] saveSlots;

    [NonSerialized] public bool isLoadingGame = false;

    GameObject activePanel;

    bool newSession;

    // This Manager Class is a singleton
    public static MenuManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Found more than one DataPersistenceManager in the scene. Duplicates were destroyed.");
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this;

        //activePanel = new GameObject();
        newSession = true;
    }

    public void PatreonLink()
    {
        string patUrl = "https://www.patreon.com/whitelynx";
        Application.OpenURL(patUrl);
    }

    public void Resume()
    {
        titleScreen.SetActive(false);
        gameScreen.SetActive(true);
    }

    public void NewGame()
    {
        newSession = false;

        SwitchToPauseMenu();

        titleScreen.SetActive(false);
        gameScreen.SetActive(true);

        newSession = false; 
        StoryManager.Instance.StartISS(0);
    }

    public void SaveOrLoadGame(bool isLoadingGame)
    {
        if (activePanel != null && activePanel.activeSelf)
            if (activePanel == saveLoadMenu)
            {
                activePanel.SetActive(false);
                return;
            }
            else
                activePanel.SetActive(false);

        // Set Menu Active
        saveLoadMenu.SetActive(true);
        activePanel = saveLoadMenu;

        // Set Mode
        this.isLoadingGame = isLoadingGame;

        // Load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();


        // Loop through each save slot in the UI and set the content appropriately
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileID(), out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && isLoadingGame)
                saveSlot.saveSlotButton.interactable = false;
            else
                saveSlot.saveSlotButton.interactable = true;
        }
    }

    public void SlotButtonClicked(string profileID)
    {
        if (isLoadingGame)
        {
            newSession = false;
            DataPersistenceManager.Instance.LoadGame(profileID);
        }
        else
        {
            DataPersistenceManager.Instance.SaveGame(profileID);
        }
    }

    public void Settings()
    {
        if (activePanel != null && activePanel.activeSelf)
            if (activePanel == settingsMenu)
            {
                activePanel.SetActive(false);
                return;
            }
            else
                activePanel.SetActive(false);

        // Set Menu Active
        settingsMenu.SetActive(true);
        activePanel = settingsMenu;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // On Pressing ESC
    public void ToggleTitleScreen(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (newSession)
                return;

            if (gameScreen.activeSelf)
            {
                gameScreen.SetActive(false);
                titleScreen.SetActive(true);

                if (activePanel != null && activePanel.activeSelf)
                    activePanel.SetActive(false);
            }
            else
            {
                titleScreen.SetActive(false);
                gameScreen.SetActive(true);
            }
        }
    }

    public void SwitchToPauseMenu()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }


    //// Implement once you switch to Toggles instead of Buttons!
    //public void togglePanel(gameobject panel)
    //{
    //    panel.setactive(!panel.activeself);
    //}

}