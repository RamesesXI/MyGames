using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEye : MonoBehaviour
{
    Camera cam;
    SpriteRenderer spRenderer;
    Bounds eyeBounds;
    bool seen = false;

    public EyeManager.Side eyeSide;
    public Transform playerTransform;
    public Transform correspondingWall;
    float wallStartxPos;
    bool wallAtTarget = false;

    public GameObject freedomImage;

    void Awake()
    {
        cam = Camera.main;
        spRenderer = GetComponent<SpriteRenderer>();
        eyeBounds = spRenderer.bounds;

        EyeManager.ListReset += ResetList; // Add the method ResetList as a delegate to the event ListReset

        wallStartxPos = correspondingWall.position.x; // Store the wall starting x position;
    }

    void Update()
    {
        if(!seen && EyeManager.playerSide == eyeSide)
        {
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);
            bool lookedAt = GeometryUtility.TestPlanesAABB(frustumPlanes, eyeBounds);

            if (lookedAt)
            {
                seen = true;
                EyeManager.unseenEyeRends.Remove(spRenderer);
            }
        }

        if (spRenderer.enabled == true)
        {
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);
            bool lookedAt = GeometryUtility.TestPlanesAABB(frustumPlanes, eyeBounds);

            if(lookedAt && !wallAtTarget)
                MovingWall();

            if(!lookedAt && wallAtTarget)
                correspondingWall.gameObject.SetActive(false);

            if (!freedomImage.activeInHierarchy)
            {
                float distance = transform.position.x - playerTransform.position.x; // Distance between the player and the active eye

                if (Mathf.Abs(distance) > 40f)
                    freedomImage.SetActive(true);
            }
        }
    }

    // The method is called when EyeManger triggers the ListRest event
    void ResetList()
    {
        EyeManager.unseenEyeRends.Add(spRenderer);
        seen = false;
    }

    void MovingWall()
    {
        // Get the x difference between the wall and the player
        float xDistance = correspondingWall.position.x - playerTransform.position.x;

        if (Mathf.Abs(xDistance) < 2f)
        {
            float xDirection = xDistance > 0 ? 2f : -2f;
            Vector3 targetWallPos = new Vector3
            {
                x = playerTransform.position.x + xDirection,
                y = correspondingWall.position.y,
                z = correspondingWall.position.z
            };

            correspondingWall.position = targetWallPos;
        }
            

        if(Mathf.Abs(correspondingWall.position.x - wallStartxPos) > 9f)
            wallAtTarget = true;
    }
}
