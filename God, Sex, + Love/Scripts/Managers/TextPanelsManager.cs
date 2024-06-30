using UnityEngine;
using TMPro;

public class TextPanelsManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialPanel;

    [SerializeField] GameObject reachedEndPanel;


    void Start()
    {
        tutorialPanel.SetActive(true);
        AreaManager.Instance.navAllowed = false;
    }

    public void OkButton(GameObject panel)
    {
        panel.SetActive(false);
        AreaManager.Instance.navAllowed = true;
    }

    public void RechedEndButton()
    {
        AreaManager.Instance.navAllowed = false;
        reachedEndPanel.SetActive(true);
    }




}
