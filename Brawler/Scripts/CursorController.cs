using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

    public Camera cam;

    private Renderer rend;
    private float maxWidth;
    private float maxHeight;

    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();

        if (cam == null)
            cam = Camera.main;

        Vector3 topRightCorner = new Vector3(Screen.width, Screen.height, 0.0f);
        Vector3 targetCoord = cam.ScreenToWorldPoint(topRightCorner);
        float cursorWidth = rend.bounds.extents.x;
        float cursorHeight = rend.bounds.extents.y;
        maxWidth = targetCoord.x - cursorWidth;
        maxHeight = targetCoord.y - cursorHeight;
    }

    void Update()
    {
        Vector3 rawPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        float targetWidth = Mathf.Clamp(rawPosition.x, -maxWidth, maxWidth);
        float targetHeight = Mathf.Clamp(rawPosition.y, -maxHeight, maxHeight);
        Vector3 targetPosition = new Vector3(targetWidth, targetHeight, 0.0f);
        transform.position = targetPosition;
        Debug.Log("hi");
    }
}