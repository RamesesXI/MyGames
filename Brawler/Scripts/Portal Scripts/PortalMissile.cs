using UnityEngine;
using System.Collections;

public class PortalMissile : MonoBehaviour
{
    [HideInInspector]
    public Vector3 direction;

    public GameObject Portal1;
    public GameObject Portal2;
    public Sprite blueSprite;
    public Sprite orangeSprite;
    private Rigidbody2D rb;
    private SpriteRenderer rend;
    public float speed;
    public static float missileCount = 0f;

    void Start()
    {
        missileCount++;
        initRigidbody();             
        rend = GetComponent<SpriteRenderer>();

        // Normalize direction and adapt angle
        direction.Normalize();
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, pointsToAngle(Vector3.zero, direction));
    }

    void Update()
    {
        if (Portal.built && rend.sprite != orangeSprite)
            rend.sprite = orangeSprite;
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!Portal.built)
            OpenPortal(Portal1, other);
        else
            OpenPortal(Portal2, other);
    }

    void OpenPortal(GameObject thePortal, Collision2D other)
    {
        string wallTag = other.gameObject.tag;

        switch (wallTag)
        {
            case "Right":
                Instantiate(thePortal, transform.position, Quaternion.Euler(0, 180, 0));
                Destroy(gameObject);
                break;
            case "Left":
                Instantiate(thePortal, transform.position, Quaternion.Euler(0, 0, 0));
                Destroy(gameObject);
                break;
            case "Roof":
                Instantiate(thePortal, transform.position, Quaternion.Euler(0, 0, -90));
                Destroy(gameObject);
                break;
            case "Floor":
                Instantiate(thePortal, transform.position, Quaternion.Euler(0, 0, 90));
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    static float pointsToAngle(Vector2 pos1, Vector2 pos2)
    {
        // Returns the angle between both positions
        return (Mathf.Atan2(pos2.y - pos1.y, pos2.x - pos1.x) * Mathf.Rad2Deg);
    }

    void OnDestroy()
    {
        missileCount--;
    }
}
