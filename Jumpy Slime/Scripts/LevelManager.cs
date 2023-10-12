using UnityEngine;
using System.Collections;
using System.Linq;
public class LevelManager : MonoBehaviour {
    public GameObject fail, win, rest, main, next;
    public AudioClip GameOverClip, WinnerClip;
    public int LevelNumber;
	// Use this for initialization
	void Start () {
	
	}

  public void FailGame()
    {
        fail.gameObject.SetActive(true);
        rest.gameObject.SetActive(true);
        next.gameObject.SetActive(false);
        main.gameObject.SetActive(true);
        if (!mainManager.Mute)
        {
            var asource = GetComponent<AudioSource>();
            asource.clip = GameOverClip;
            asource.loop = false;
            asource.Play();
        }
    }
    public  bool GoToNextLevelIfPossible()
    {

        if (!GameObject.FindGameObjectsWithTag("box").Where(x => ((x.GetComponent<BoxJumpController>().BoxKind & BoxType.ReservedBoxes )== 0) ).Any())
        {
            Debug.Log("Go");
            /*
            win.active = true;
            rest.active = true;
            next.active = true;
            main.active = true;*/
            win.gameObject.SetActive(true);
            rest.gameObject.SetActive(true);
            next.gameObject.SetActive(true);
            main.gameObject.SetActive(true);

            if (!mainManager.Mute)
            {
                var asource = GetComponent<AudioSource>();
                asource.clip = WinnerClip;
                asource.loop = false;
                asource.Play();
            }
            return true;
        }
        else
        {
            //fail.active = true;
            //rest.active = true;
            //next.active = true;
            //main.active = true;
            return false;
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
