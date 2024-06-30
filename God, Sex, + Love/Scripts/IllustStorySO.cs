using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Illustration Segment", menuName = "Scriptable Objects/Illustration Segment")]
public class IllustStorySO : ScriptableObject
{
    public List<Sprite> Illustrations;
    public TextAsset ChapterJSON;
    public int LandingArea_idx;
}
