using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWall : MonoBehaviour
{
     BoxCollider invisibleWallCol;

    void Start()
    {
        invisibleWallCol = transform.parent.GetComponent<BoxCollider>();
    }

	void OnTriggerEnter()
    {
        invisibleWallCol.enabled = false;
    }

    void OnTriggerExit()
    {
        invisibleWallCol.enabled = true;
    }
}
