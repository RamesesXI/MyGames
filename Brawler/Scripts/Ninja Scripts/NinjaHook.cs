using UnityEngine;
using System.Collections;

public class NinjaHook: MonoBehaviour
{
    public Vector3 direction;
    private float speed = 20f;

    private bool isHooked_ = false;
    public delegate void HookAction();
    public event HookAction onHooked;

    private Rigidbody2D rigidbody_;

	protected void Start()
    {
        initRigidbody_();

        // Normalize direction and adapt hook angle
	    direction.Normalize();
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, pointsToAngle(Vector3.zero, direction));
	}
	
	protected void FixedUpdate()
    {
        // When hooked, the hook stops moving
        if (isHooked_)
            return;

        // Make sure the rotation is right (because of the parent)...
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, pointsToAngle(Vector3.zero, direction));
        // ... and also make sure the velocity hasn't changed
        rigidbody_.velocity = direction * speed;
	}


    // Initialization
    private void initRigidbody_()
    {
        rigidbody_ = gameObject.GetComponent<Rigidbody2D>();
        if (!rigidbody_)
        {
            rigidbody_ = gameObject.AddComponent<Rigidbody2D>();

            // No mass, no gravity. The entity is entirely managed by script
            rigidbody_.mass = 0f;
            rigidbody_.gravityScale = 0f;
        }

        rigidbody_.interpolation = RigidbodyInterpolation2D.Interpolate;
        rigidbody_.sleepMode = RigidbodySleepMode2D.StartAwake;
        rigidbody_.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
    // !Initialization

   
    // Listeners
    protected void OnCollisionEnter2D(Collision2D c)
    {
        // Make kinematic to ensure the hook won't move anymore
        rigidbody_.isKinematic = true;
        rigidbody_.simulated = false;

        gameObject.transform.eulerAngles = new Vector3(0f, 0f, pointsToAngle(Vector3.zero, direction));

        isHooked_ = true;
        if (onHooked != null)
            onHooked();
    }

    protected void OnTriggerEnter2D(Collider2D c)
    {
        // Make kinematic to ensure the hook won't move anymore
        rigidbody_.isKinematic = true;
        rigidbody_.simulated = false;

        gameObject.transform.eulerAngles = new Vector3(0f, 0f, pointsToAngle(Vector3.zero, direction));

        isHooked_ = true;
        if (onHooked != null)
            onHooked();
    }
    // !Listeners


    // Util
    public bool isHooked()
    {
        return isHooked_;
    }

    public void rotateTo(Vector3 pos)
    {
        transform.eulerAngles = new Vector3(0f, 0f, 180f + pointsToAngle(gameObject.transform.position, pos));
    }

    private static float pointsToAngle(Vector2 anc1, Vector2 anc2)
    {
        // Returns the angle between both positions
        return (Mathf.Atan2(anc2.y - anc1.y, anc2.x - anc1.x) * Mathf.Rad2Deg);
    }
    // !Util
}
