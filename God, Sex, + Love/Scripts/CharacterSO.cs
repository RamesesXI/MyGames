using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character")]
public class CharacterSO : ScriptableObject
{
    public int ChapterProgress;
    public int Affection;
    public int Mood;

    public int Position;
    public int Action;
    public int Clothes;
}
