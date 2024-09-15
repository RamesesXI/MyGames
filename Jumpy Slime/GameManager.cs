using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Audio Management Variables
    AudioSource audioSource;
    [HideInInspector]  public bool Mute = false;
    [SerializeField] Button SoundButton;
    [SerializeField] Sprite MuteSprite;
    [SerializeField] Sprite PlayingSprite;

    // Level-End Management Variables
    [SerializeField] AudioClip levelClearedClip;
    [SerializeField] AudioClip levelFailedClip;
    [SerializeField] GameObject levelClearedMenu;
    [SerializeField] GameObject levelFailedMenu;

    // GameManager is a singleton
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        // Singleton Protection Check
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Found more than one GameManager in the scene. Duplicates were destroyed.");
            return;
        }
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }


    // ***************** Main Menu Buttons

    public void StartButton()
    {
        SceneManager.LoadScene("Lvl 1");
    }

    public void GoToLevelsMenu()
    {
        SceneManager.LoadScene("Levels Menu");
    }

    public void ToggleAudio()
    {
        if (!Mute)
            audioSource.Pause();
        else
            audioSource.Play();

        // Alternative to the above conditional statement:
        // ToggleAudioSource(Mute)
        // bool ToggleAudioSource(bool mute) => mute ? audioSource.Pause() : audioSource.Play();

        SoundButton.image.sprite = Mute ? MuteSprite : PlayingSprite;
        Mute = !Mute;
    }


    // ***************** Levels Menu

    public void GoToLevel(int levelNum)
    {
        SceneManager.LoadScene("Lvl " + levelNum.ToString());
    }


    // ***************** Game Over Popups

    public void LevelCleared()
    {
        levelClearedMenu.SetActive(true);

        if (!Mute)
            audioSource.PlayOneShot(levelClearedClip);
    }

    public void LevelFailed()
    {
        levelFailedMenu.SetActive(true);

        if (!Mute)
            audioSource.PlayOneShot(levelFailedClip);
    }


    // ***************** Game Over Menu Buttons

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToNextLevel() // Retrieves the number of the next level and loads its Scene formatted "Lvl x"
    {
        int nextLvlNum = int.Parse(SceneManager.GetActiveScene().name.Substring(4)) + 1;

        if (nextLvlNum <= 8)
            SceneManager.LoadScene("Lvl " + nextLvlNum.ToString());
    }
}
