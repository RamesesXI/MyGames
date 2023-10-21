using UnityEngine;
using System.Collections;

public class BackGroundAnimation : MonoBehaviour
{
    int a;
    public Sprite sp1, sp2, sp3, sp4;

	void Start ()
    {
        a = Random.Range(1, 5);

        switch(a)
        {
            case 1:
                transform.GetComponent<SpriteRenderer>().sprite = sp1;
                break;

            case 2:
                transform.GetComponent<SpriteRenderer>().sprite = sp2;
                break;

            case 3:
                transform.GetComponent<SpriteRenderer>().sprite = sp3;
                break;

            case 4:
                transform.GetComponent<SpriteRenderer>().sprite = sp4;
                break;
        }
    }
}
