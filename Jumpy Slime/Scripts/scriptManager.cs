using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class scriptManager : MonoBehaviour {

    public void restart(int stage)
    {
        SceneManager.LoadScene(stage.ToString());
    }

    public void main()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void nextlvl(int stage)
    {
        if (stage > 1)
            PlayerPrefs.SetInt("MaxLevelReached", stage );

        SceneManager.LoadScene(stage.ToString());
    }
}

