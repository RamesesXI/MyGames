using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class scriptManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void restart(int stager)
    {
        SceneManager.LoadScene(stager.ToString());
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

