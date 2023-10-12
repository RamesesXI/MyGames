using UnityEngine;
using System.Collections;
using System.Linq;
public class BarrierController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            var sc = col.gameObject.GetComponent<SlimeControle>();
            sc.canMove = false;
          //TODO:ADD DEATH ANIMATION
            GameObject.FindGameObjectsWithTag("box").Where(x => (x.GetComponent<BoxJumpController>().BoxKind & BoxType.Red) != 0).First().GetComponent<LevelManager>().FailGame();
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
