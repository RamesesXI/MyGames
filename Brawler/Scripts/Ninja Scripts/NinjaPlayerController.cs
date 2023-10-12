
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class NinjaPlayerController : GeneralPlayerController
{
    private float speed_ = 200f, ropeSpeed_ = 5f;
    private float minRopeLength_ = 0.1f, maxRopeLength_ = 12f;

    public GameObject hookPrefab;
    private GameObject hookInstance_;
    private NinjaHook hookScript_;
    private bool isHooking_ = false;

    public GameObject ropePrefab;
    private GameObject ropeInstance_;
    private NinjaRope ropeScript;

    protected override void Update()
    {
        base.Update();

        PrimaryFire();

        if (!isHooking_)
        {
            MoveControl();
            JumpControl();
            
            // No hook launched / hooked, so can launch it
            if (Input.GetMouseButtonDown(1))
                LaunchHook_(Quaternion.Euler(0f, 0f, getCursorAngle()) * Vector3.right);               
        }
        else
        {
            // Hook is either launched or hooked
            hookScript_.rotateTo(ropeScript.postFirstAnchor.anchor);

            // No input, remove the hook
            if (/*Input.GetKeyDown(KeyCode.E)*/ Input.GetMouseButtonDown(1))
            {
                anim.SetBool("isHooked", false);
                removeHook_();
                return;
            }

            if (!hookScript_.isHooked())
            {
                // The hook is launched, but not yet hooked
                // Remove hook if it took too long to find a spot to hook on
                if (maxRopeLength_ < Vector3.Distance(hookInstance_.transform.position, gameObject.transform.position)) 
                    removeHook_();
            }
            else
            {
                // The hook is hooked on a surface
                anim.SetBool("isHooked", true);

                Vector3 upVector = (ropeScript.lastAnchor.anchor - gameObject.transform.position).normalized;
                Vector3 rightVector = (Quaternion.Euler(0f, 0f, -90f) * upVector).normalized;
                Vector3 posDelta = Vector3.zero;
                

                // Move the player                
                if (Input.GetKey(KeyCode.A))
                    posDelta -= rightVector * speed_ * Time.fixedDeltaTime;
                if (Input.GetKey(KeyCode.D))
                    posDelta += rightVector * speed_ * Time.fixedDeltaTime;

                if (0.0001f < posDelta.magnitude)
                    rb.AddForce(posDelta);

                // Move up or down the rope
                if (Input.GetKey(KeyCode.W))
                    ropeScript.ropeLength -= ropeSpeed_ * Time.fixedDeltaTime;
                if (Input.GetKey(KeyCode.S))
                    ropeScript.ropeLength += ropeSpeed_ * Time.fixedDeltaTime;

                if (minRopeLength_ > ropeScript.ropeLength)
                    ropeScript.ropeLength = minRopeLength_;
                if (maxRopeLength_ < ropeScript.ropeLength)
                    ropeScript.ropeLength = maxRopeLength_;
            }
        }
    }

    void FixedUpdate()
    {
        FlipControl();
    }

    private void LaunchHook_(Vector3 dir)
    {
        // Instantiate hook, and make the player the hook parent to ensure the hook position is affected by the player
        hookInstance_ = Instantiate(hookPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        hookScript_ = hookInstance_.GetComponent<NinjaHook>();
        hookScript_.direction = dir;

        ropeInstance_ = Instantiate(ropePrefab);
        ropeScript = ropeInstance_.GetComponent<NinjaRope>();
        ropeScript.baseEntity = hookInstance_;
        ropeScript.endEntity = gameObject;
        ropeScript.removeLength(); // Remove the max length of the rope

        hookScript_.onHooked += this.onHooked;

        isHooking_ = true;
    }

    private void removeHook_()
    {
        // Destroy both the hook and the rope (duh!)
        Destroy(ropeInstance_);
        Destroy(hookInstance_);

        isHooking_ = false;
    }

    public void onHooked()
    {
        // Reset the max length of the rope
        ropeScript.setLength(ropeScript.getRopeLength());
    }

    protected override float getCursorAngle()
    {
        float opposite = getCursorPosition().y - transform.position.y;
        float adjacent = getCursorPosition().x - transform.position.x;

        return Mathf.Atan2(opposite, adjacent) * Mathf.Rad2Deg;
    }
}