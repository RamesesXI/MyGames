using UnityEngine;
using System.Collections;

public abstract class GeneralPlayerController : MonoBehaviour
{  
    private bool facingRight = true;
    protected Rigidbody2D rb;
    protected Animator anim;
    public Transform gunMuzzle;

    //Moving variables
    public float maxSpeed;
    public bool isBlocked = false;
    public static bool isPushed = false;

    //Jumping variables
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpHeight;
    protected bool grounded;
    private float groundCheckRadius = 0.1f;

    //Shooting variables
    public GameObject missilePrefab;
    private GameObject missileInstance;
    private MissileController missileScript;
    public float missileSpeed;
    public float missileLifetime;
    public float fireRate;
    private float nextFire = 0f;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        Portal.OnTeleported += this.OnTeleported;
    }

    protected virtual void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    protected virtual void MoveControl()
    {
        if (isPushed)
            return;

        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(move));
        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);
    }

    protected virtual void JumpControl()
    {
        //Check if grounded and set vertical speed
        anim.SetBool("isGrounded", grounded);
        anim.SetFloat("VerticalSpeed", rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            grounded = false;
            anim.SetBool("isGrounded", grounded);
            rb.AddForce(new Vector2(rb.velocity.x, 300));
        }
    }

    protected virtual void FlipControl()
    {
        if (!facingRight && getCursorPosition().x > transform.position.x)
            Flip();
        else if (facingRight && getCursorPosition().x < transform.position.x)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    protected virtual void PrimaryFire()
    {
        //The fire rate is how many rounds per second
        if (Input.GetAxisRaw("Fire1") > 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + (1 / fireRate);
                Shoot(getCursorAngle(), missileSpeed, missileLifetime);
            }
        }
    }

    private void Shoot(float angle, float speed, float lifetime)
    {
        missileInstance = Instantiate(missilePrefab, gunMuzzle.position, Quaternion.identity) as GameObject;

        missileScript = missileInstance.GetComponent<MissileController>();
        
        missileScript.zAngle = angle;
        missileScript.lifetime = lifetime;
        missileScript.speed = speed;
    }

    public void OnTeleported(Vector2 dir)
    {
        StartCoroutine(Teleport(dir));
    }

    private IEnumerator Teleport(Vector2 dir)
    {
        isPushed = true;
        rb.velocity = dir * 20f;
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector2.zero;
        isPushed = false;
    }

    protected virtual void Freeze()
    {
        StartCoroutine(FreezeCo());
    }

    private IEnumerator FreezeCo()
    {
        isBlocked = true;
        anim.enabled = false;
        rb.isKinematic = true;

        yield return new WaitForSeconds(3f);

        rb.isKinematic = false;
        anim.enabled = true;
        isBlocked = false;
    }

    protected Vector3 getCursorPosition()
    {
        Vector3 cursorPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);
        return cursorPosition;
    }

    protected virtual float getCursorAngle()
    {
        float opposite = getCursorPosition().y - gunMuzzle.position.y;
        float adjacent = getCursorPosition().x - gunMuzzle.position.x;

        float angle = Mathf.Atan2(opposite, adjacent) * Mathf.Rad2Deg;

        if ((facingRight && adjacent >= 0f) || (!facingRight && adjacent <= 0f))
            return angle;
        else
            return opposite >= 0f ? 90f : -90f;
    }
}