using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuController : MonoBehaviour {

    public Button resume;
    void Start() {
       // if (PlayerPrefs.HasKey("played"))
           // resume.GetComponentInChildren<Text>().text = "resume";

    }

    public void starting() {

        if (!PlayerPrefs.HasKey("played"))
            SceneManager.LoadScene("1");
        else
        {
         
            SceneManager.LoadScene(PlayerPrefs.GetString("played"));
        }
    }

    public void levels() {

        SceneManager.LoadScene("Levels");
    }

}
