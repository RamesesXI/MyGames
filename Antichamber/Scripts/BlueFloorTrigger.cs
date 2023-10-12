using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFloorTrigger : MonoBehaviour
{
    public GameObject stripe;
    public Transform destFloor; // Destination floor 
    public Transform nextLevelFloor;

    CameraRotation rotationScript;  // Player rotation script
    Camera cam;

    bool teleportPending = false;
    Transform playerTransform;
    Bounds stripeBounds;

    public EyeManager.Side corridorSide;

    void Start()
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
            Vector3 tFloorRelativePos = transform.InverseTransformPoint(playerCol.transform.position);

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

            if (objectLookedAt)
            {
                Vector3 tFloorRelativePos = transform.InverseTransformPoint(playerTransform.position);

                // Check if player is deep enough into the corridor to teleport to next level
                if (tFloorRelativePos.z < -0.4f)
                {
                    teleportPending = false;
                    Teleport(nextLevelFloor);
                    EyeManager.playerSide = corridorSide;   // Player side takes whatever side he went into in the eye level
                }
            }

            if (!objectLookedAt)
            {
                teleportPending = false;
                Teleport(destFloor);
            }
        }
    }

    // Comments for this method can be found on the GreenFloorTrigger script
    void Teleport(Transform destTransform)
    {
        Vector3 tFloorRelativePos = transform.InverseTransformPoint(playerTransform.position);
        Vector3 targetPos = destTransform.TransformPoint(tFloorRelativePos);

        Vector3 targetRot = destTransform.rotation.eulerAngles - transform.rotation.eulerAngles;

        playerTransform.position = targetPos;
        playerTransform.rotation *= Quaternion.Euler(targetRot);

        rotationScript.UpdateRotation();

        playerTransform = null;
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
