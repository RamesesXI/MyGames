using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprite Segment", menuName = "Scriptable Objects/Sprite Segment")]
public class SpriteStorySO : ScriptableObject
{
    [SerializeField] public List<Sprite>[] CharImage = new List<Sprite>[2];

    public List<Sprite> Char1Sprites;
    public List<Sprite> Char2Sprites;

    public Sprite backgroundImage;

    public TextAsset ChapterJSON;
    public int LandingArea_idx;
}
