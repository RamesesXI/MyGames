using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float verticalSpeed;
    public float horizontalSpeed;

    bool isRotating = false;
    int rightSideFingerID = -1;

    Vector3 firstPoint;
    Vector3 secondPoint;

    float xAngle;
    float yAngle;
    float xAngleTemp;
    float yAngleTemp;

    Transform playerTransform;
	
    void Start()
    {
        playerTransform = transform.parent;

        xAngle = transform.rotation.eulerAngles.x;          // Vertical rotation angle
        yAngle = playerTransform.rotation.eulerAngles.y;    // Horizontal rotation angle
    }

    void Update ()
    {
        if (Input.touchCount > 0)
        {
            Touch[] myTouches = Input.touches; // Gets all the touches and stores them in an array

            // Loops through all the current touches
            foreach (Touch touch in myTouches)
            {
                // If this touch just started on the right side of screen
                if (touch.phase == TouchPhase.Began && touch.position.x > Screen.width / 2 && !isRotating)
                {
                    rightSideFingerID = touch.fingerId;
                    isRotating = true;

                    firstPoint = touch.position;

                    // Get temporary angles based on the firstPoint
                    xAngleTemp = xAngle;
                    yAngleTemp = yAngle;
                }

                if (touch.fingerId == rightSideFingerID && touch.phase == TouchPhase.Moved)
                {
                    secondPoint = touch.position;

                    // Calculate the new angles
                    xAngle = xAngleTemp + ((secondPoint.y - firstPoint.y) / Screen.height) * -90 * verticalSpeed;
                    yAngle = yAngleTemp + ((secondPoint.x - firstPoint.x) / Screen.width) * 180 * horizontalSpeed;

                    xAngle = Mathf.Clamp(xAngle, -90f, 90f); // Restrict vertical rotation

                    transform.localRotation = Quaternion.Euler(xAngle, 0f, 0f);     // Rotate the camera vertically
                    playerTransform.rotation = Quaternion.Euler(0f, yAngle, 0f);    // Rotate the player horizontally
                }

                // If this right sided finger id touch has ended
                if (touch.phase == TouchPhase.Ended && touch.fingerId == rightSideFingerID)
                {
                    isRotating = false;
                    rightSideFingerID = -1;
                }
            } // !foreach
        } // !if
    } // Update

    public void UpdateRotation()
    {
        yAngle = playerTransform.rotation.eulerAngles.y;

        if (isRotating)
        {
            // Resets firstPoint to current touch position
            Touch touch = Input.GetTouch(rightSideFingerID);
            firstPoint = touch.position;

            // Resets temporary angles
            xAngleTemp = xAngle;
            yAngleTemp = yAngle;
        }
    }
}
