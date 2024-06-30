using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int AreaIndex; // Eventually make a dictionary of AreaName-Index for readability, or enum perhaps?
    public int DayNumber;
    public int DayPart;
    public int AP;

    public List<CharacterSO> CharactersSO = new List<CharacterSO>();
}

//public CharacterSO AthenaSO;
//public CharacterSO MayaSO;
//public CharacterSO FaithSO;

// Story.illust.line

// IMMEDIATE DATA
/* 
Checks if story or nav mode
If stroy mode:
    Check if general or which character  story it is + Illust
If Nav mode:
    Retrive Area
*/


// PROGRESS DATA
/*
- General Chapter Progress (SS index + illust)

- Character Progress
    Chapter Progress (SS index + illust)
    Only Care: Affection
    *Position
    *Clothes
    *Mood
 */

