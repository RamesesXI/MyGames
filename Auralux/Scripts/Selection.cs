using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    Touch touch;
    Vector2 initTouchPos;

    Camera cam;
    int borderThickness;
    const int thicknessRatio = 400; // The screen diagonal length to border thickness ratio
    float screenWidth;
    float screenHeight;

    bool isSelecting = false;
    public static bool unitsSelected = false;
    bool canMoveUnits = true;

    static List<UnitMovement> selectedScripts = new List<UnitMovement>();

    static int circleCount = 0;
    static int squareCount = 0;
    static int triangleCount = 0;

    public GameObject SelectionPanel;
    static GameObject SelectPanel;
    public Text circle, square, triangle;
    static Text circleText, squareText, triangleText;

    void Start()
    {
        cam = Camera.main;

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        // Set static variables to their references
        SelectPanel = SelectionPanel;
        circleText = circle;
        squareText = square;
        triangleText = triangle;

        // We get the border thickness by dividing the screen diagonal length by the ratio, rounded to the nearest integer
        borderThickness = (int)Mathf.Round(Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2)) / thicknessRatio);
    }

    void Update()
    {
        // If there are fingers on screen
        if (Input.touchCount > 0)
        {
            // If exactly one finger on screen
            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);

                // If no units are selected, check for selection
                if (!unitsSelected)
                {
                    // This condition prevents starting selection by 1 finger down from 2 or more fingers
                    if (touch.phase == TouchPhase.Began)
                    {
                        // Initiate selection
                        isSelecting = true;
                        initTouchPos = touch.position;
                    }

                    // If the sole finger is lifted while selecting
                    if (touch.phase == TouchPhase.Ended && isSelecting)
                    {
                        // Cancel selection and boxcast
                        isSelecting = false;
                        CastSelection(touch.position);
                    }
                }
                // Else if there are selected units, check for movement
                else
                {
                    if(touch.phase == TouchPhase.Ended && canMoveUnits)
                    {
                        Vector2 targetPosition = cam.ScreenToWorldPoint(touch.position);

                        foreach (UnitMovement unitScript in selectedScripts)
                            unitScript.MoveToPosition(targetPosition);
                    }
                }
            }
            // Else if more than one finger on screen
            else
            {
                // Cancel selection when user decides to move camera while selecting
                if (isSelecting)
                    isSelecting = false;

                // Prevent user from moving units down from moving the camera
                if (canMoveUnits)
                    canMoveUnits = false;
            }
        }
        //Else if no fingers on screen
        else
        {
            // User is able to move units again after he lifts both fingers down from moving the camera
            if (!canMoveUnits)
                canMoveUnits = true;
        }
    }

    void CastSelection(Vector2 endTouchPos)
    {
        // Get selection rect in screen space
        Rect selectionRect = Utilities.GetScreenSelectionRect(initTouchPos, endTouchPos);

        // Multiply by this to convert Screen distance to World distance
        float pixelsToWorld = cam.orthographicSize / (screenHeight / 2f);

        // Get BoxCast center and size by converting the rect's center, width, and height from Screen to World space
        Vector2 boxCenter = cam.ScreenToWorldPoint(selectionRect.center);
        float boxWidth = selectionRect.size.x * pixelsToWorld;
        float boxHeight = selectionRect.size.y * pixelsToWorld;
        Vector2 boxSize = new Vector2(boxWidth, boxHeight);

        RaycastHit2D[] hitUnits = Physics2D.BoxCastAll(boxCenter, boxSize, 0f, transform.forward);

        if (hitUnits.Length > 0)
        {
            unitsSelected = true;
            SelectPanel.SetActive(true);
        }

        for (int i = 0; i < hitUnits.Length; i++)
        {
            selectedScripts.Add(hitUnits[i].transform.gameObject.GetComponent<UnitMovement>());
            selectedScripts[i].Selected();

            switch (selectedScripts[i].tag)
            {
                case "C":
                    circleCount++;
                    break;
                case "S":
                    squareCount++;
                    break;
                case "T":
                    triangleCount++;
                    break;
            }
        }

        UpdateText();
    }

    // Called by the Deselect Button
    public void DeselectAll()
    {
        for (int i = 0; i < selectedScripts.Count; i++)
            selectedScripts[i].Deselected();

        selectedScripts.Clear();

        circleCount = 0;
        squareCount = 0;
        triangleCount = 0;

        unitsSelected = false;
        SelectPanel.SetActive(false);
    }

    public void DeselectAllExcept(string buttonTag)
    {
        for (int i = 0; i < selectedScripts.Count; i++)
        {
            if (!selectedScripts[i].CompareTag(buttonTag))
            {
                selectedScripts[i].Deselected();
                selectedScripts.RemoveAt(i);
                i--;    // Decrement i because an element from the looped list was removed
            }
        }

        switch (buttonTag)
        {
            case "C":
                squareCount = 0;
                triangleCount = 0;
                break;
            case "S":
                circleCount = 0;
                triangleCount = 0;
                break;
            case "T":
                circleCount = 0;
                squareCount = 0;
                break;
        }

        UpdateText();
    }

    // Sets text to actual count for each kind of unit
    static void UpdateText()
    {
        circleText.text = circleCount.ToString();
        squareText.text = squareCount.ToString();
        triangleText.text = triangleCount.ToString();
    }

    // OnGUI is what displays the selection rectangle and its border
    void OnGUI()
    {
        if (isSelecting && !unitsSelected)
        {
            Rect selectionRect = Utilities.GetSelectionRect(initTouchPos, touch.position);

            Utilities.DrawRect(selectionRect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utilities.DrawRectBorder(selectionRect, borderThickness, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    public static void OnUnitDestroyed(string unitTag, UnitMovement unitScript)
    {
        if (selectedScripts.Contains(unitScript))
        {
            selectedScripts.Remove(unitScript);

            if(selectedScripts.Count > 0)
            {
                switch (unitTag)
                {
                    case "C":
                        circleCount--;
                        break;
                    case "S":
                        squareCount--;
                        break;
                    case "T":
                        triangleCount--;
                        break;
                }
            }
            else
            {
                circleCount = 0;
                squareCount = 0;
                triangleCount = 0;

                unitsSelected = false;
                SelectPanel.SetActive(false);
            }

            UpdateText();
        } 
    }
}
