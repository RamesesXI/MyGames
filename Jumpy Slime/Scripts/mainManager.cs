using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class mainManager : MonoBehaviour {
    public static bool Mute = false;

    public Button SoundButton;
    public Sprite MuteSprite;
    public Sprite PlayingSprite;

	// Use this for initialization
	void Start () {

        if (Mute)
        {
            GameObject.Find("Main Camera").GetComponent<AudioSource>().Pause();
            SoundButton.image.sprite = MuteSprite;
        }
        else
        {
            GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
            SoundButton.image.sprite = PlayingSprite;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void nusicONOFF()
    {

        if (!Mute)
        {
            Mute = true;
            GameObject.Find("Main Camera").GetComponent<AudioSource>().Pause();
            
            SoundButton.image.sprite = MuteSprite;
             
        }
        else 
        {
            Mute = false;
            GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
            SoundButton.image.sprite = PlayingSprite;
        }

    }
}
