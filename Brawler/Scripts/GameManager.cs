using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Texture2D cursorTexture;
    private Vector2 hotSpot;
    private CursorMode cursorMode = CursorMode.ForceSoftware;

    void Start ()
    {
        hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void Update()
    {
        MainMenu();
    }

    public void LoadNinjaScene()
    {
        SceneManager.LoadScene("Ninja");
    }

    public void LoadTpScene()
    {
        SceneManager.LoadScene("Teleportist");
    }

    public void LoadTracerScene()
    {
        SceneManager.LoadScene("Tracer");
    }

    public void MainMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        if(SceneManager.GetActiveScene().name != "CharSelection")
        SceneManager.LoadScene("CharSelection");
    }
}