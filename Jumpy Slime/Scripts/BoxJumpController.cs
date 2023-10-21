using UnityEngine;
using System.Collections;
using System;

[Flags]
public enum BoxType
{
    None = 0,       //00000
    Green = 1,      //00001
    Red = 2,        //00010
    Normal = 4,     //00100
    JumpBoost = 8,  //01000
    Shield = 16,    //10000

    ReservedBoxes = Red | Green | JumpBoost | Shield //11011
}

public class BoxJumpController : MonoBehaviour
{ 
    public static GameObject CurrentLevelRedBox;
    public BoxType BoxKind;

    public void Jump(SlimeControle slime,Rigidbody2D rb, float forcejump)
    {
        if(!mainManager.Mute)
                slime.jump_aduio_src.Play();

        rb.velocity = Vector2.zero;
        slime.animator.SetTrigger("Jumping");

        if ((BoxKind & BoxType.JumpBoost) != 0)
        {
            rb.AddForce(new Vector3(0, 2f * forcejump, 0));
            slime.DoubleJump = true;

        }
        else
        {
            rb.AddForce(new Vector3(0, forcejump, 0));
            slime.DoubleJump = false;
        }

        slime.JumpTime = 0.5f;
    }
    
    void Start ()
    {
        if (BoxKind == BoxType.Red)
            CurrentLevelRedBox = this.gameObject;
            if (mainManager.Mute)
                this.gameObject.GetComponent<AudioSource>().Pause();
	}
}
