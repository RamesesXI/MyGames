using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed;     // Panning speed of the camera    
    public float zoomSpeed; // The rate of change of the field of view in perspective mode.

    Camera cam;
    float screenDiagonalLength;

    void Start()
    {
        cam = GetComponent<Camera>();
        screenDiagonalLength = Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2));
    }

    void Update()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            
            // Pinch to zoom
            // Find the position in the previous frame of each touch
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            // This difference depends on screen pixels, that's why we divide by screen diagonal
            float deltaMagnitudeDiff = (prevTouchDeltaMag - touchDeltaMag) / screenDiagonalLength;

            // Change the orthographic size based on the change in distance between the touches
            cam.orthographicSize += deltaMagnitudeDiff * zoomSpeed;

            // Make sure the orthographic size never drops below zero
            cam.orthographicSize = Mathf.Max(cam.orthographicSize, 1f);
            // !Pinch to zoom

            // Translation
            Vector2 combinedDeltaPostion = (touchZero.deltaPosition + touchOne.deltaPosition) / screenDiagonalLength;
            transform.Translate(-combinedDeltaPostion.x * speed * cam.orthographicSize, 
                                -combinedDeltaPostion.y * speed * cam.orthographicSize, 0);
            // !Translation
        }
    }
}
