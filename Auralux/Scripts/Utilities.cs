using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    static Texture2D whiteTexture;

    // Drawn Selection Rect Methods
    // Setting up whiteTexture the non-Object-Oriented way because this is a static class, can't use Start().
    public static Texture2D WhiteTexture
    {
        get
        {
            if (whiteTexture == null)
            {
                whiteTexture = new Texture2D(1, 1);
                whiteTexture.SetPixel(0, 0, Color.white);
                whiteTexture.Apply();
            }

            return whiteTexture;
        }
    }

    // Draws a rectangle
    public static void DrawRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    // Draws borders of a rectangle
    public static void DrawRectBorder(Rect rect, int thickness, Color color) // Thickness is in pixels
    {
        // Top
        DrawRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        DrawRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        DrawRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        DrawRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    // Returns a rectangle based on 2 corners, the initial touch position and the current touch position
    public static Rect GetSelectionRect(Vector2 initTouchPos, Vector2 currentTouchPos)
    {
        // the origin for rects is top left while the origin for screen touch is bottom left (in pixels)
        // Moving origin from bottom left to top left:
        initTouchPos.y = Screen.height - initTouchPos.y;
        currentTouchPos.y = Screen.height - currentTouchPos.y;

        // Calculate corners
        Vector2 topLeft = Vector2.Min(initTouchPos, currentTouchPos);
        Vector2 bottomRight = Vector2.Max(initTouchPos, currentTouchPos);

        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }
    // !Drawn Selection Rect Methods


    // Returns a rectangle based on 2 corners in Screen space rather than Rect space
    public static Rect GetScreenSelectionRect(Vector2 initTouchPos, Vector2 currentTouchPos)
    {
        // Calculate corners
        Vector2 topLeft = Vector2.Min(initTouchPos, currentTouchPos);
        Vector2 bottomRight = Vector2.Max(initTouchPos, currentTouchPos);

        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }
}
