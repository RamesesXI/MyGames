using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeStateManager;

public class CharLocationManager : MonoBehaviour
{
    [SerializeField] GameObject[] charSprites;

    public void UpdatCharLocations(TimeStateManager.DayPart dayPart)
    {

        switch (dayPart)
        {
            case DayPart.Morning:

                break;
            case DayPart.Midday:
                charSprites[0].SetActive(false);
                charSprites[1].SetActive(false);

                charSprites[2].SetActive(true);
                charSprites[3].SetActive(true);
                break;
            case DayPart.Evening:
                charSprites[2].SetActive(false);
                charSprites[3].SetActive(false);

                //charSprites[4].SetActive(true);
                charSprites[5].SetActive(true);
                break;
            case DayPart.Night:
                // Handle night logic
                break;
        }
    }
}
