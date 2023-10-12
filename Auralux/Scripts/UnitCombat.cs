using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    public string victimTag;
    public Color32 damagedColor;
    static bool combat = true;

    const float bornDamagedChance = 0.02f;
    bool isDamaged = false;

    void Start()
    {
        // There's a chance the unit is spawned damaged
        if (Random.value < bornDamagedChance)
            Damage();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.tag == victimTag)
        //{
        //    other.GetComponent<UnitCombat>().Damage();
        //}
        //else
        //{
        //    Destroy(other.gameObject);
        //}
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == victimTag)
            combat = true;
    }

    void Damage()
    {
        if (isDamaged)
            Destroy(gameObject);

        isDamaged = true;
        GetComponent<UnitMovement>().isDamaged = true;
        GetComponent<SpriteRenderer>().color = damagedColor;
    }
}
