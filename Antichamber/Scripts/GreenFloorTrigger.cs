using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFloorTrigger : MonoBehaviour
{
    public Transform destTransform; // Destination floor transform
    public GameObject stripe;

    CameraRotation rotationScript;  // Player rotation script
    Camera cam;

    bool teleportPending = false;
    Transform playerTransform;
    Bounds stripeBounds;

    void Start ()
    {
        cam = Camera.main;
        rotationScript = cam.GetComponent<CameraRotation>();
    }

    void OnTriggerEnter(Collider playerCol)
    {
        if (!teleportPending)
        {
            playerTransform = playerCol.transform;              // Get a reference to the player's Transform
            stripeBounds = GetChildrenColliderBounds(stripe);   // Get the stripe bounds
            teleportPending = true;
        }
    }

    // Cancels a pending teleport if player comes back from where he came from
    void OnTriggerExit(Collider playerCol)
    {
        if (teleportPending)
        {
            // Get Player's relative position to trigger floor
            Vector3 tFloorRelativePos = transform.InverseTransformPoint(playerCol.transform.position);

            // Checks if player exited trigger collider from the entrance side 
            // The floor's trigger collider bounds are between 0.25 (entrance side) and -0.25 (exit side)
            if (tFloorRelativePos.z > 0.2f)
                teleportPending = false;
        }
    }

    void Update()
    {
        if (teleportPending)
        {
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);                // Get the camera's frustum planes
            bool objectLookedAt = GeometryUtility.TestPlanesAABB(frustumPlanes, stripeBounds);  // Check if the stripe bounds are within the frustum planes

            // Check whether there are any walls between the stripe and the camera
            if (objectLookedAt)
            {
                // Get Player's relative position to trigger floor
                Vector3 tFloorRelativePos = transform.InverseTransformPoint(playerTransform.position);

                // Check if player is deep enough around the corner such that the stripe is covered by the wall
                if (Mathf.Abs(tFloorRelativePos.x) > 0.9f)
                    objectLookedAt = false;
            }

            if (!objectLookedAt)
            {
                teleportPending = false;

                // Get Player's relative position to trigger floor
                Vector3 tFloorRelativePos = transform.InverseTransformPoint(playerTransform.position);
                // Get the world space position that coresponds to a position relative to destination floor
                Vector3 targetPos = destTransform.TransformPoint(tFloorRelativePos);

                // Get the difference in rotaion between the two floors
                Vector3 targetRot = destTransform.rotation.eulerAngles - transform.rotation.eulerAngles;

                // Apply calculated position and rotation to Player
                playerTransform.position = targetPos;
                playerTransform.rotation *= Quaternion.Euler(targetRot);

                // Updates the player rotation in the CameraRotation script
                rotationScript.UpdateRotation();

                // Free up no longer needed memory
                playerTransform = null;
            }
        }
    }

    // Get the encapsulated bounds of an object's children
    Bounds GetChildrenColliderBounds(GameObject parentObject)
    {
        Collider[] colliders = parentObject.GetComponentsInChildren<BoxCollider>();

        Bounds bounds = colliders[0].bounds;

        for (int i = 1; i < colliders.Length; i++)
            bounds.Encapsulate(colliders[i].bounds);

        return bounds;
    }
}
