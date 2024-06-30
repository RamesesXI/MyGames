using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OldStoryManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] GameObject dialoguePanel; //
    [SerializeField] TextMeshProUGUI dialogueText; //
    [SerializeField] Image activeIllust; //
    [SerializeField] List<IllustStorySO> storySegmentSOs = new List<IllustStorySO>(); //

    public int currentSSI = -1; // Current Story Segment Index //

    List<Sprite> activeIllustrations; // ditch
    int activeIllustIndex = 0; //

    Story currentStory;//
    bool dialogueIsPlaying;//

    const string SPEAKER = "speaker";//
    const string PORTRAIT = "portrait";//
    const string ILLUSTRATION = "illustration";//

    string speakerText;//

    // This manager is a singleton
    static OldStoryManager Instance;
    public static OldStoryManager GetInstance() { return Instance; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
            Debug.Log("Found more than one StoryManager in the scene. Duplicates were destroyed.");
            return;
        }

        Instance = this;
    }

    void Start()
    {
        StartSegment();
    }

    public void StartSegment()
    {
        currentSSI++;

        // Setting up segment text
        currentStory = new Story(storySegmentSOs[currentSSI].ChapterJSON.text);

        // Setting up segment illustrations
        activeIllustrations = storySegmentSOs[currentSSI].Illustrations; 
        activeIllustIndex = 0;
        //activeIllust.sprite = activeIllustrations[activeIllustIndex];
        //activeIllust.gameObject.SetActive(true);


        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    void ContinueStory()
    {
        if (!dialogueIsPlaying)
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

    void ExitStoryMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        activeIllust.gameObject.SetActive(false);

        OldAreaManager.Instance.EnterNavMode(storySegmentSOs[currentSSI].LandingArea_idx);
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

                    // Change font based on speaker?;
                    break;

                case ILLUSTRATION:
                    if (tagValue == "next")
                        nextIllust(activeIllust);
                    else if (tagValue == "black")
                        activeIllust.gameObject.SetActive(false);
                    break;
            }
        }
    }

    void nextIllust(Image illust)
    {
        if (!illust.gameObject.activeSelf)
            illust.gameObject.SetActive(true);

        //illust.sprite = activeIllustrations[activeIllustIndex++];

        // ALTERNATIVE (DITCH MAKING A NEW LIST OF SS ILLUSTRATIONS)
        illust.sprite = storySegmentSOs[currentSSI].Illustrations[activeIllustIndex++];

        //float illustAspectRatio = illust.sprite.textureRect.width / illust.sprite.textureRect.height;

        //RectTransform rt = activeIllust.GetComponent<RectTransform>();
        //rt.sizeDelta = new Vector2(rt.sizeDelta.x, 1080f);

        //activeIllust.GetComponent<AspectRatioFitter>().aspectRatio = illustAspectRatio;
    }

    #region Input Methods

    void ContinueStoryInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            ContinueStory();
    }

    void ReverseStoryInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {

        }
    }

    #endregion

}
