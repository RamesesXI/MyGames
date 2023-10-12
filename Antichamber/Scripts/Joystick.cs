using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    Image bgImage; // Background of the joystick, this is the part of the joystick that recieves input
    Image knobImage; // The handle part of the joystick, it just moves to provide feedback

    bool joystickVisible = false;
    int leftSideFingerID = -1; // Unique finger id for touches on the left-side half of the screen
    public float joystickHandleDistance = 0.4f; // Sets the maximum distance the handle (knob) stays away from the center of this joystick

    Vector3 inputVector; // A direction vector that will be ouput from this joystick class

    void Start()
    {
        bgImage = GetComponent<Image>(); // gets the background image of this joystick
        knobImage = transform.GetChild(0).GetComponent<Image>(); // gets the joystick "knob" image (the handle of the joystick)
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch[] myTouches = Input.touches; // Gets all the touches and stores them in an array

            // Loops through all the current touches
            foreach (Touch touch in myTouches)
            {
                // If this touch just started (finger is down for the first time), for this particular touch 
                if (touch.phase == TouchPhase.Began)
                {
                    // If this touch is on the left-side half of screen, this enables the joystick image on touch in its proper position
                    if (touch.position.x < Screen.width / 2 && !joystickVisible)
                    {
                        joystickVisible = true;
                        leftSideFingerID = touch.fingerId;

                        // Calculates the x and y positions of the joystick to where the screen was touched
                        Vector3 currentPosition = new Vector3
                        {
                            x = touch.position.x + (bgImage.rectTransform.sizeDelta.x / 2),
                            y = touch.position.y - (bgImage.rectTransform.sizeDelta.y / 2)
                        };

                        // Keeps the joystick image from going through the edges of the screen
                        currentPosition.x = Mathf.Clamp(currentPosition.x, bgImage.rectTransform.sizeDelta.x, Screen.width / 2);
                        currentPosition.y = Mathf.Clamp(currentPosition.y, 0f, Screen.height - bgImage.rectTransform.sizeDelta.y);

                        // Sets the position of joystick image to where the screen was touched (limited to the left half of the screen)
                        bgImage.rectTransform.position = currentPosition;

                        // Enables left joystick on touch
                        bgImage.enabled = true;
                        knobImage.enabled = true;
                    }
                }

                // This is where we calculate the input vector and handle the joystick knob movement
                if (touch.fingerId == leftSideFingerID)
                {
                    // Resets the localPoint out parameter of the RectTransformUtility.ScreenPointToLocalPointInRectangle function on each drag event
                    Vector2 localPoint = Vector2.zero;

                    // If the point touched on the screen is within the background image of this joystick
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, touch.position, null, out localPoint))
                    {
                        // localPoint is the point within the joystick's background image that was touched

                        // Divide the local screen point touched within the image by the size of the image itself, in order to get the following values
                        localPoint.x /= bgImage.rectTransform.sizeDelta.x; // localPoint.x becomes (from Left to Right, -1 to 0)
                        localPoint.y /= bgImage.rectTransform.sizeDelta.y; // localPoint.y becomes (from Bottom to Top, 0 to 1)

                        // The correct x and y point values are created here
                        // inputVector.x becomes(from Left to Right -1 to 1)
                        // inputVector.y becomes(from Bottom to Top -1 to 1)
                        inputVector = new Vector3(localPoint.x * 2 + 1, localPoint.y * 2 - 1, 0);

                        // Normalizes the vector, this will be used to ouput to a game object controller to control movement
                        inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

                        // Moves the joystick handle "knob" image
                        knobImage.rectTransform.anchoredPosition = new Vector3
                        {
                            x = inputVector.x * (bgImage.rectTransform.sizeDelta.x * joystickHandleDistance),
                            y = inputVector.y * (bgImage.rectTransform.sizeDelta.y * joystickHandleDistance)
                        };
                    }
                }

                // if this touch has ended (finger is up and now off of the screen), for this particular touch
                if (touch.phase == TouchPhase.Ended)
                {
                    // if this touch is the touch that began on the left half of the screen
                    if (touch.fingerId == leftSideFingerID)
                    {
                        joystickVisible = false;
                        leftSideFingerID = -1;

                        inputVector = Vector3.zero; // Resets the inputVector so that output will no longer affect movement of the game object
                        knobImage.rectTransform.anchoredPosition = Vector3.zero; // Resets the knob back to the center

                        // TO DO: a Coroutine that makes the knob smoothly go back to center before disabling the images

                        //makes the left joystick disappear
                        bgImage.enabled = false;
                        knobImage.enabled = false;
                    }
                }
            } // !foreach
        } // !if
    } // Update

    // Ouputs the direction vector, use this public function from another script to control movement of a game object
    public Vector3 GetInputDirection()
    {
        return new Vector3(inputVector.x, 0f, inputVector.y);
    }
}
