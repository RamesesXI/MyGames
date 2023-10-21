using UnityEngine;
using System.Collections;
using System.Linq;
public class BarrierController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            var sc = col.gameObject.GetComponent<SlimeControle>();
            sc.canMove = false;
            GameObject.FindGameObjectsWithTag("box").Where(x => (x.GetComponent<BoxJumpController>().BoxKind & BoxType.Red) != 0).First().GetComponent<LevelManager>().FailGame();
        }
    }
}
