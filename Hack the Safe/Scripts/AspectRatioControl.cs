using UnityEngine;

public class AspectRatioControl : MonoBehaviour
{

    void Start()
    {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetAspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float screenAspect = (float)Screen.width / Screen.height;

        // current viewport height should be scaled by this amount
        float scaleRatio = targetAspect / screenAspect;

        // obtain camera component so we can modify its viewport
        if (!Mathf.Approximately(screenAspect, 1.0f))
        {
            Camera camera = GetComponent<Camera>();
            camera.fieldOfView *= scaleRatio;
        }
    }
}