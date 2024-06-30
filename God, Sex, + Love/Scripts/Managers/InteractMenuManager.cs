using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class InteractMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;

    //[SerializeField] GameObject hangoutMenu;
    //[SerializeField] GameObject requestMenu;
    //[SerializeField] GameObject serviceMenu;
    //[SerializeField] GameObject intimacyMenu;

    [SerializeField] GameObject[] subMenus;

    public CharacterInteract SelectedChar;

    // This manager is a singleton
    public static InteractMenuManager Instance { get; private set; }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            Debug.Log("Found more than one InteractMenuManager in the scene. Duplicates were destroyed.");
            return;
        }

        Instance = this;
    }

    public void Chat()
    {
        if (SelectedChar.isRead)
            return;

        SelectedChar.isRead = true;
        TimeStateManager.Instance.ConsumeAP();
        gameObject.SetActive(false);

        if (SelectedChar.IsSpriteStory)
            StoryManager.Instance.StartSSS(SelectedChar.StoryIdx);
        else
            StoryManager.Instance.StartISS(SelectedChar.StoryIdx);
    }

    public void OpenSubMenu(GameObject subMenu)
    {
        mainMenu.SetActive(false);
        subMenu.SetActive(true);
    }

    private void OnDisable()
    {
        foreach (GameObject subMenu in subMenus)
            if (subMenu.activeSelf)
                subMenu.SetActive(false);

        if (!mainMenu.activeSelf)
            mainMenu.SetActive(true);
    }
}