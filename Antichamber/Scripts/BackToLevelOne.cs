using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToLevelOne : MonoBehaviour
{
    public Transform destTransform;
    CameraRotation rotationScript;

    void Start()
    {
        rotationScript = Camera.main.GetComponent<CameraRotation>();
    }

    void OnTriggerEnter(Collider playerCol)
    {
        EyeManager.playerSide = EyeManager.Side.neither;    // Player Side is neither once he leaves the eye level

        Vector3 tFloorRelativePos = transform.InverseTransformPoint(playerCol.transform.position);
        Vector3 targetPos = destTransform.TransformPoint(tFloorRelativePos);
       
        Vector3 targetRot = destTransform.rotation.eulerAngles - transform.rotation.eulerAngles;

        playerCol.transform.position = targetPos;
        playerCol.transform.rotation *= Quaternion.Euler(targetRot);

        rotationScript.UpdateRotation();
    }
}
