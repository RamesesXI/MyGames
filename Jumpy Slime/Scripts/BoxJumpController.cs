using UnityEngine;
using System.Collections;
using System;

[Flags]
public enum BoxType
{
    None = 0,
    Green = 1,
    Red = 2, 
    Normal=4,
    JumpBoost=8,
    Shield = 16,

    ReservedBoxes = Red | Green | JumpBoost | Shield
}
public class BoxJumpController : MonoBehaviour {
   
    public static GameObject CurrentLevelRedBox;
    public BoxType BoxKind;

    public void Jump(SlimeControle sc,Rigidbody2D rigid, float forcejump)
    {

        if(!mainManager.Mute)
                sc.jump_aduio_src.Play();

        rigid.velocity = Vector2.zero;
        sc.animator.SetTrigger("Jumping");
        if ((BoxKind & BoxType.JumpBoost) != 0)
        {
            rigid.AddForce(new Vector3(0, 2f * forcejump, 0));
            sc.DoubleJump = true;

        }
        else {
            rigid.AddForce(new Vector3(0, forcejump, 0));
            sc.DoubleJump = false;
        }
        sc.JumpTime = 0.5f;
    }
    
    void Start () {

        if ((BoxKind & BoxType.Red) == BoxType.Red)
        {
            CurrentLevelRedBox = this.gameObject;
            if (mainManager.Mute)
                this.gameObject.GetComponent<AudioSource>().Pause();

        }
	}
	
	void Update () {
	
	}
}
