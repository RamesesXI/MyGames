using UnityEngine;
using UnityEngine.EventSystems;

public class Raycaster : MonoBehaviour
{

    [SerializeField] GameObject interactPanel;
    [SerializeField] GameObject overviewPanel;

    bool doneMast = false;
    bool doneAandM = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            if (overviewPanel.activeSelf)
                overviewPanel.SetActive(false);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Define a ray from camera to mouse click position
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero); // Perform raycast
            if (hit.collider != null) // Check for if collider is hit
            {
                // Get the hit object and its layer
                GameObject hitGameobject = hit.collider.gameObject;
                int hitLayer = hitGameobject.layer;

                // Perform different actions based on the layer
                switch (hitLayer)
                {
                    case 6: // Warp layer
                        if (AreaManager.Instance.navAllowed)
                        {
                            if(interactPanel.activeSelf)
                                interactPanel.SetActive(false);

                            if (hitGameobject.GetComponent<Warp>().TargetArea == 2)
                            {
                                if (TimeStateManager.Instance.currentDayPart == TimeStateManager.DayPart.Morning && !doneAandM)
                                {
                                    TimeStateManager.Instance.ConsumeAP();
                                    AreaManager.Instance.ActiveArea.SetActive(false);
                                    doneAandM=true;
                                    StoryManager.Instance.StartSSS(0);
                                    return;
                                }
                            }

                            if (hitGameobject.GetComponent<Warp>().TargetArea == 6)
                            {
                                if (TimeStateManager.Instance.currentDayPart == TimeStateManager.DayPart.Midday && !doneMast)
                                {
                                    TimeStateManager.Instance.ConsumeAP();
                                    AreaManager.Instance.ActiveArea.SetActive(false);
                                    doneMast=true;
                                    StoryManager.Instance.StartISS(2);
                                    return;
                                }
                            }

                            if (hitGameobject.GetComponent<Warp>().TargetArea == 5)
                            {
                                if (TimeStateManager.Instance.currentDayPart == TimeStateManager.DayPart.Midday)
                                {
                                    AreaManager.Instance.ActiveArea.SetActive(false);
                                    StoryManager.Instance.StartISS(3);
                                    return;
                                }
                            }

                            AreaManager.Instance.SwitchArea(hitGameobject.GetComponent<Warp>().TargetArea);
                        }
                        break;
                    case 7: // Character Layer
                        interactPanel.SetActive(!interactPanel.activeSelf);

                        InteractMenuManager.Instance.SelectedChar = hitGameobject.GetComponent<CharacterInteract>();

                        //InteractMenuManager.Instance.IsSpriteStory = isSpriteStory;
                        //InteractMenuManager.Instance.StoryIdx = storyIdx;

                        break;
                    default:
                        Debug.Log("Hit an object on an unhandled layer");
                        break;
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            // Toggle Overview Panel
            overviewPanel.SetActive(!overviewPanel.activeSelf);
        }
    }
}
