using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonScripts : MonoBehaviour
{
    public Button LV1, LV2, LV3, LV4, LV5, LV6, LV7, LV8;
 
    void Start()
    {
        if (PlayerPrefs.GetInt("MaxLevelReached", 0) != 0)
        {
            int level = PlayerPrefs.GetInt("MaxLevelReached", 0);
            LV1.enabled = level >= 1;
            LV2.enabled = level >= 2;
            LV3.enabled = level >= 3;
            LV4.enabled = level >= 4;
            LV5.enabled = level >= 5;
            LV6.enabled = level >= 6;
            LV7.enabled = level >= 7;
            LV8.enabled = level >= 8;
        }
        else
        {
            LV1.enabled = true;
            LV2.enabled = false;
            LV3.enabled = false;
            LV4.enabled = false;
            LV5.enabled = false;
            LV6.enabled = false;
            LV7.enabled = false;
            LV8.enabled = false;
        }

    }
    public void Level1() {
        SceneManager.LoadScene("1");
    }

    public void Level2() {
        SceneManager.LoadScene("2");
    }

    public void Level3() {
        SceneManager.LoadScene("3");
    }

    public void Level4() {
        SceneManager.LoadScene("4");
    }
    public void Level5()
    {
        SceneManager.LoadScene("5");
    }

    public void Level6()
    {
        SceneManager.LoadScene("6");
    }

    public void Level7()
    {
        SceneManager.LoadScene("7");
    }

    public void Level8()
    {
        SceneManager.LoadScene("8");
    }

   

}
