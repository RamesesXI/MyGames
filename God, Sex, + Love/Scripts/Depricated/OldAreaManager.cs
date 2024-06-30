using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class OldAreaManager : MonoBehaviour
{
    [SerializeField] List<GameObject> areas = new List<GameObject>(); //
    GameObject activeArea;
    public int ActiveAreaIndex; //

    [SerializeField] GameObject sidePanel; //
    [SerializeField] TextMeshProUGUI sideText; //

    [SerializeField] List<GameObject> temporary = new List<GameObject>(); //ditch
    [SerializeField] GameObject solMeditating; //ditch

    [SerializeField] GameObject inventoryText; //ditch

    // This manager is a singleton
    public static OldAreaManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
            Debug.Log("Found more than one AreaManager in the scene. Duplicates were destroyed.");
            return;
        }

        Instance = this;
    }

    void Start()
    {

    }

    public void EnterNavMode(int areaIndex)
    {
        ActiveAreaIndex = areaIndex;

        activeArea = areas[areaIndex];
        activeArea.SetActive(true);

        sidePanel.SetActive(true);

        UpdateSideText();
    }

    public void ExitNavMode()
    {
        activeArea.SetActive(false);

        sidePanel.SetActive(false);
    }

    public void SwitchArea(int areaIndex)
    {
        ActiveAreaIndex = areaIndex;

        activeArea.SetActive(false);
        activeArea = areas[areaIndex];
        activeArea.SetActive(true);

        UpdateSideText();
    }


    /*
     * We will switch to story mode when player clicks on an object which triggers the transition:
     * Zendo (start satsang)
     * Maya Sleeping
     * Faith inside hot spring
     * 
     * Dialogue mode:
     * Faith outside
     * Maya & Faith Arguing
     * 
     */

    void UpdateSideText()
    {
        switch (OldStoryManager.GetInstance().currentSSI)
        {
            case 0:
                sideText.text = "Click on the yellow arrow to move to the corridor.\n\nThen click on the second door down the hallway to get to Athena's room.";

                if (ActiveAreaIndex == 2)
                {
                    foreach (GameObject temp in temporary)
                        temp.SetActive(false);

                    ExitNavMode();
                    OldStoryManager.GetInstance().StartSegment();
                }
                break;
            case 1:
                sideText.text = "Go back to the Tatami Room.\n\nClick on the sliding door at the top right side to get to the kitchen.\n\nClick on the Table Sink found in the middle to fill up a glass of water.\n\nGo back to Athena's Room and click on her.";

                if (ActiveAreaIndex == 1)
                {
                    ExitNavMode();
                    OldStoryManager.GetInstance().StartSegment();
                }
                break;
            case 2:
                if (ActiveAreaIndex == 2 && inventoryText.activeInHierarchy)
                {
                    inventoryText.SetActive(false);
                    ExitNavMode();
                    OldStoryManager.GetInstance().StartSegment();
                }
                break;
            case 3:
                sideText.text = "Click on the white cushion in the Tatami Room to meditate while waiting for Maya and Faith to arrive.\n\nMeditation increases the Ashram's overall Frequency which unlocks interactions and story segments. (coming soon)\n\nThis game is still in early development, please stay tuned for future updates!\nThanks for playing :)" ;

                if (ActiveAreaIndex != 0)
                    solMeditating.SetActive(false);
                break;

        }
    }



}
