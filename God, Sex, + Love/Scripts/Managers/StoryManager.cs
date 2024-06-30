using Ink.Runtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using Steamworks;


public class StoryManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject stortyModePanel;
    [SerializeField] GameObject navModePanel;

    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;

    [SerializeField] Image illustImage;

    [SerializeField] Image char1Image;
    [SerializeField] Image char2Image;

    [SerializeField] TMP_Text timerText; 

    [Header("Story")]
    Story currentStory;

    [SerializeField] List<IllustStorySO> illustStorySOs = new List<IllustStorySO>(); // List of all illust story segments
    int currentIS_idx; // Index of the current illust story segment
    int currentIllust_idx; // Index of the current illustration of the current story segment

    [SerializeField] List<SpriteStorySO> spriteStorySOs = new List<SpriteStorySO>(); // List of all sprite story segments
    int currentSS_idx; // Index of the current sprite story segment
    int currentChar1_idx; // Index of the current char1 sprite of the current story segment
    int currentChar2_idx; // Index of the current char2 sprite of the current story segment

    int landingArea_idx;
    string speakerText;
    bool storyMode;
    bool isCountingDown;

    const string SPEAKER = "speaker";
    const string ILLUSTRATION = "illustration";
    const string CHAR1 = "char1";
    const string CHAR2 = "char2";

    //public static UnityEvent<int> OnStoryEnded = new UnityEvent<int>();

    // This manager is a singleton
    public static StoryManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            Debug.Log("Found more than one StoryManager in the scene. Duplicates were destroyed.");
            return;
        }

        Instance = this;
    }

    public void StartISS(int IS_idx)
    {
        if (!illustImage.gameObject.activeSelf)
            illustImage.gameObject.SetActive(true);

        if (AreaManager.Instance.ActiveArea != null)
            AreaManager.Instance.ActiveArea.SetActive(false);

        // Setting proper indices
        currentIS_idx = IS_idx;
        currentIllust_idx = 0;
        landingArea_idx = illustStorySOs[currentIS_idx].LandingArea_idx;

        // Setting up story text
        currentStory = new Story(illustStorySOs[currentIS_idx].ChapterJSON.text);
        BindingExternalFuntions();

        storyMode = true;
        navModePanel.SetActive(false);
        stortyModePanel.SetActive(true);

        ContinueStory();
    }

    public void StartSSS(int SS_idx)
    {
        if(!illustImage.gameObject.activeSelf)
            illustImage.gameObject.SetActive(true);


        // Setting proper indices
        currentSS_idx = SS_idx;
        currentChar1_idx = 0;
        currentChar2_idx = 0;
        landingArea_idx = spriteStorySOs[currentSS_idx].LandingArea_idx;

        // Setting up story text
        currentStory = new Story(spriteStorySOs[currentSS_idx].ChapterJSON.text);
        BindingExternalFuntions();

        storyMode = true;
        navModePanel.SetActive(false);
        illustImage.sprite = spriteStorySOs[currentSS_idx].backgroundImage;
        stortyModePanel.SetActive(true);

        ContinueStory();
    }

    void ContinueStory()
    {
        if (!storyMode || isCountingDown)
            return;

        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            HandleTags(currentStory.currentTags);
            dialogueText.text = speakerText + dialogueText.text;
        }
        else
            ExitStoryMode();
    }

    void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER:
                    if (tagValue != "narrator")
                        speakerText = tagValue + ":\n";
                    else
                        speakerText = "";

                    //Change Color
                    switch (tagValue)
                    {
                        case "narrator":
                            //dialogueText.color = new Color(255, 255, 255);
                            //Debug.Log("Changed Color?");
                            break;
                        case "Aphrodite":

                            break;
                        case "Sol":

                            break;
                        case "Athena":
                            //Debug.Log("Changed Color?");
                            //dialogueText.color = new Color(162,162,162);
                            break;
                        case "Maya":

                            break;
                        case "Faith":

                            break;
                    }

                    // Change font based on speaker?;
                    break;

                case ILLUSTRATION:
                    if (tagValue == "next")
                        nextIllust();
                    else if (tagValue == "black")
                        illustImage.gameObject.SetActive(false);
                    break;

                case CHAR1:
                    if (tagValue == "next")
                        nextChar(1);
                    else if (tagValue == "off")
                        char1Image.gameObject.SetActive(false);
                    break;

                case CHAR2:
                    if (tagValue == "next")
                        nextChar(2);
                    else if (tagValue == "off")
                        char2Image.gameObject.SetActive(false);
                    break;
            }
        }
    }

    void nextIllust()
    {
        // This check accounts for when starting a new segment and the occasional black screens
        if (!illustImage.gameObject.activeSelf)
            illustImage.gameObject.SetActive(true);

        // Assigns the next Illustration of the current Story Segment
        illustImage.sprite = illustStorySOs[currentIS_idx].Illustrations[currentIllust_idx++];
    }

    void nextChar(int charNum)
    {
        if(charNum == 1)
        {
            // This check accounts for when starting a new segment and the occasional char out
            if (!char1Image.gameObject.activeSelf)
                char1Image.gameObject.SetActive(true);

            char1Image.sprite = spriteStorySOs[currentSS_idx].Char1Sprites[currentChar1_idx];
        }
        else
        {
            // This check accounts for when starting a new segment and the occasional char out
            if (!char2Image.gameObject.activeSelf)
                char2Image.gameObject.SetActive(true);

            char2Image.sprite = spriteStorySOs[currentSS_idx].Char2Sprites[currentChar2_idx];
        }

        // Assigns the next Illustration of the current Story Segment
        //character.sprite = spriteStorySOs[currentSS_idx].CharImage[charNum][spriteIdx++];
    }

    void ExitStoryMode()
    {
        UnbindingExternalFunctions();

        //illustImage.gameObject.SetActive(false);
        char1Image.gameObject.SetActive(false);
        char2Image.gameObject.SetActive(false);

        storyMode = false;
        dialogueText.text = "";
        stortyModePanel.SetActive(false);
        navModePanel.SetActive(true);

        AreaManager.Instance.EnterNavMode(landingArea_idx);

        // Brodcast the end of story n
        //OnStoryEnded.Invoke(currentIS_idx);
    }

    IEnumerator FadeInIllust(float duration)
    {
        Color newColor = illustImage.color;
        newColor.a = 0;
        illustImage.color = newColor;

        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            float t = elapsedTime / duration;
            newColor.a = t;
            illustImage.color = newColor;

            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }
    }

    //gggggggggggggggggggggggggggggggggggg
    IEnumerator WaitaSec(float t)
    {
        yield return new WaitForSeconds(t);
    }

    IEnumerator CountDown(float duration)
    {
        isCountingDown = true;
        timerText.gameObject.SetActive(true);
        dialoguePanel.SetActive(false);

        Color newColor = illustImage.color;
        newColor.a = 0;
        illustImage.color = newColor;

        float currentTime = duration;

        while (currentTime > 0)
        {
            timerText.text = currentTime.ToString("F0");
            yield return new WaitForSeconds(1f);
            currentTime -= 1;
        }

        timerText.gameObject.SetActive(false);
        dialoguePanel.SetActive(true);
        isCountingDown = false;

        StartCoroutine(FadeInIllust(3));
    }

    void BindingExternalFuntions()
    {
        currentStory.BindExternalFunction("FadeIn", (float duration) => {
            StartCoroutine(CountDown(duration));
        });

        currentStory.BindExternalFunction("CountdownTimer", (float duration) => {
            StartCoroutine(FadeInIllust(duration));
        });

        currentStory.BindExternalFunction("ConsumeAP", () => {
            TimeStateManager.Instance.ConsumeAP();
        });

        currentStory.BindExternalFunction("UpdateAffection", (bool athena) => {
            OverviewManager.Instance.UpdateAffection(athena);
        });
    }

    void UnbindingExternalFunctions()
    {
        currentStory.UnbindExternalFunction("FadeIn");
        currentStory.UnbindExternalFunction("CountdownTimer");
        currentStory.UnbindExternalFunction("ConsumeAP");
        currentStory.UnbindExternalFunction("UpdateAffection");
    }

    #region Input Methods

    public void ContinueStoryInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            ContinueStory();
    }

    public void ReverseStoryInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {

        }
    }

    #endregion
}
