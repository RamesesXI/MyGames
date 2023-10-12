using UnityEngine;
using System.Collections;

public class BackGroundAnimation : MonoBehaviour {
    int a;
    public Sprite s1, s2;
	// Use this for initialization
	void Start () {
        a = Random.Range(1, 3);
       
        if (a == 1)
        {
            transform.GetComponent<SpriteRenderer>().sprite = s1;
        }
        if (a == 2)
        {
            transform.GetComponent<SpriteRenderer>().sprite = s2;
        }
        //if (a == 3)
        //{
        //    transform.GetComponent<SpriteRenderer>().sprite = s3;
        //}
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
