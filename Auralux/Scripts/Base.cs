using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int level;

    public Transform unitsParent;
    public GameObject unitPrefab;

    void Start()
    {
        InvokeRepeating("SpawnUnit", 1f, 1f);
    }

    void SpawnUnit()
    {
        // If base is unclaimed, no units would spawn
        if (level == 0)
            return;

        Vector2 unitVelocity = Random.insideUnitCircle.normalized * Random.Range(1.25f, 1.75f); // Generates random vector
        InstantiateWithDirection(unitVelocity);

        if (level == 2)
        {
            InstantiateWithDirection(-unitVelocity); // Push second unit in the inversed direction
        }
        else if (level == 3)
        {
            // Push second and third unit 120 degrees apart from the first unit
            InstantiateWithDirection(Quaternion.AngleAxis(120, Vector3.forward) * unitVelocity);
            InstantiateWithDirection(Quaternion.AngleAxis(-120, Vector3.forward) * unitVelocity);
        }
    }

    // Instantiates unit and gives it an intitial direction
    void InstantiateWithDirection(Vector2 velocity)
    {
        GameObject unitInstance = Instantiate(unitPrefab, transform.position, Quaternion.identity, unitsParent) as GameObject;
        unitInstance.GetComponent<UnitMovement>().InitialDirction(velocity);    // Passes a velocity to the unit script
    }
}
