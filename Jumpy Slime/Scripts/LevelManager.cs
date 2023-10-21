using UnityEngine;
using System.Collections;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public GameObject fail, win, rest, main, next;
    public AudioClip GameOverClip, WinnerClip;
    public int LevelNumber;


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

    public bool GoToNextLevelIfPossible()
    {
        if (GameObject.FindGameObjectsWithTag("box").Where(x => x.GetComponent<BoxJumpController>().BoxKind != BoxType.Normal).Any())
        {
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
            return false;
    }
}
