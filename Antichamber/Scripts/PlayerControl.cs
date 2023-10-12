using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public Joystick joystickScript;
    CharacterController controller;

    public float speed;

	void Start ()
    {
        controller = GetComponent<CharacterController>();
    }
	
	void Update ()
    {
        Movement();
	}

    void Movement()
    {
        Vector3 velocity = joystickScript.GetInputDirection();  // Get the joystick input

        if (velocity != Vector3.zero)
        {
            velocity = transform.TransformDirection(velocity);          // Convert the input from the joystick to player local space
            controller.SimpleMove(velocity * Time.deltaTime * speed);   // Apply movement to character controller
        }
    }
}
