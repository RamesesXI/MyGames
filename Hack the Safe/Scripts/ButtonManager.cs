using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public Button soundButton;
    public Sprite soundOn;
    public Sprite soundOff;
    private bool isMute = false;

    public Button helpButton;
    public Sprite helpOn;
    public Sprite helpOff;
    public GameObject helpPanel;

    public Button restartButton;

    public Button complexityButton;
    public Sprite complexityOn;
    public Sprite complexityOff;
    public GameObject complexityPanel;

    public GameObject winnerPanel;

    public void Mute()
    {
        isMute = !isMute;

        AudioListener.volume = isMute ? 0 : 1;
        soundButton.image.sprite = isMute ? soundOff : soundOn;
    }

    public void Help()
    {
        if (!helpPanel.activeInHierarchy)
        {
            if (complexityPanel.activeInHierarchy)
            {
                complexityPanel.SetActive(false);
                complexityButton.image.sprite = complexityOff;
            }

            if (winnerPanel.activeInHierarchy)
                winnerPanel.SetActive(false);

            helpPanel.SetActive(true);
            helpButton.image.sprite = helpOn;
        }
        else
        {
            helpPanel.SetActive(false);
            helpButton.image.sprite = helpOff;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        AudioListener.volume = isMute ? 0 : 1;
        soundButton.image.sprite = isMute ? soundOff : soundOn;
    }

    public void Complexity()
    {
        if (!complexityPanel.activeInHierarchy)
        {
            if (helpPanel.activeInHierarchy)
            {
                helpPanel.SetActive(false);
                helpButton.image.sprite = helpOff;
            }

            if (winnerPanel.activeInHierarchy)
                winnerPanel.SetActive(false);

            complexityPanel.SetActive(true);
            complexityButton.image.sprite = complexityOn;
        }
        else
        {
            complexityPanel.SetActive(false);
            complexityButton.image.sprite = complexityOff;
        }
    }

    public void ComplexityLevel(int level)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Submission.complexityLevel = level;
    }

    public void RestartAvailable()
    {
        restartButton.interactable = true;
    }

    public void youWonPopup()
    {
        winnerPanel.SetActive(true);
    }
}
