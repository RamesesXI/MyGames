using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour
{
    [HideInInspector]
    public float zAngle;

    [HideInInspector]
    public float speed, lifetime;

    private Rigidbody2D rb;
    public Vector2 direction;

    void Start()
    {
        initRigidbody();

        gameObject.transform.eulerAngles = new Vector3(0f, 0f, zAngle);
        direction = Quaternion.Euler(0f, 0f, zAngle) * Vector2.right;

        rb.velocity = direction * speed;

        Destroy(gameObject, lifetime);
    }

    void initRigidbody()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (!rb)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();

            // No mass, no gravity. The entity is entirely managed by script
            rb.mass = 0f;
            rb.gravityScale = 0f;
        }

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.sleepMode = RigidbodySleepMode2D.StartAwake;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
        Debug.Log(rb.velocity);
    }

    void Update()
    {
        if (zAngle != transform.eulerAngles.z)
        {
            direction = transform.right;
            rb.velocity = direction * speed;
            zAngle = transform.eulerAngles.z;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        LayerMask otherLayer = other.gameObject.layer;
        string otherLayerName = LayerMask.LayerToName(otherLayer);

        switch (otherLayerName)
        {
            case "SideWall":
            case "Ground":
            case "Roof":
            case "Character":
            default:
                Destroy(gameObject);
                break;
        }
    }
}
