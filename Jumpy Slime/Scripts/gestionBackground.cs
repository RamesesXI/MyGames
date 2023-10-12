using UnityEngine;
using System.Collections;

public class gestionBackground : MonoBehaviour {
    public GameObject sp1, sp2;

    void Start () {
	
	}
	
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D col)
    {

        col.GetComponent<Rigidbody2D>().velocity = (new Vector2(5, 0));
    }
}
