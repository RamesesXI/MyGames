using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeManager : MonoBehaviour
{

    public static List<SpriteRenderer> unseenEyeRends = new List<SpriteRenderer>();

    public delegate void ResetList();
    public static event ResetList ListReset;

    public enum Side { left, right, neither}
    public static Side playerSide = Side.neither;

    SpriteRenderer visibleEye;  // a reference to the visible to be able to disable it wehn player leaves the level

    void Start()
    {
        ListReset();
    }

    void Update()
    {
        if(unseenEyeRends.Count == 1)
        {
            visibleEye = unseenEyeRends[0];
            visibleEye.enabled = true;
            unseenEyeRends.Clear();
        }

        if(unseenEyeRends.Count == 0 && playerSide == Side.neither)
        {
            ListReset();
            visibleEye.enabled = false;
        }
    }


}
