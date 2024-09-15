using System.Collections;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    // Component References
    Rigidbody2D rb;
    Animator animator;
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;

    // General Variables
    bool isFacingRight = true;
    [SerializeField] float jumpBoostRate = 1.5f;
    bool oddBounce = true; // an odd bounce is the first, third, fifth... consecutive time the slime is about to touch a box

    // Idle Jump Variables
    [SerializeField] float defaultJumpForce = 220f;
    bool isJumpBoosted = false;

    // Leap Variables
    bool leapPending = false;
    bool isLeapingRight;
    [SerializeField] float leapHeight = 1f;
    [SerializeField] float leapDuration = 0.66f;
    [SerializeField] float boostedLeapDuration = 0.87f;

    // Shield Variables
    bool isShielded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Android touch input handling
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                leapPending = true; // Signals a pending leap on next contact

                float normalizedX = touch.position.x / Screen.width; // Get the normalized touch position along the horizontal axis
                isLeapingRight = normalizedX > 0.5f; // Sets pending leap's direction

                if (isLeapingRight != isFacingRight)
                    Flip();
            }
        }
        // PC input handling
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            leapPending = true; // Signals a pending leap on next contact
            isLeapingRight = Input.GetKeyDown(KeyCode.RightArrow); // Sets pending leap's direction

            if (isLeapingRight != isFacingRight)
                Flip();
        }
    }
    
    void Flip() // Simply inverts the x scale
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y); 
        isFacingRight = isLeapingRight;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Mutable":
                MutateBox(col.gameObject);
                break;

            case "Jump Boost":
                isJumpBoosted = true;
                break;

            case "Shield":
                isShielded = true;
                spriteRenderer.color = new Color(47, 243, 0); // Dark Green when Shielded
                Destroy(col.gameObject);
                break;

            case "Barrier":
                rb.bodyType = RigidbodyType2D.Kinematic;
                GameManager.Instance.LevelFailed();
                return;

            case "Goal":
                if (GameObject.FindGameObjectsWithTag("Mutable").Length > 0)
                    break;
                rb.bodyType = RigidbodyType2D.Kinematic;
                GameManager.Instance.LevelCleared();
                return;
        }

        if (!leapPending)
            Jump();
        else
        {
            if (isJumpBoosted)
            {
                StartCoroutine(BoostedLeap());
                isJumpBoosted = false;
            }
            else
                StartCoroutine(Leap());

            leapPending = false;
        }
    }

    void Jump() // Idle vertical jump
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, IdleJumpForce()));


        PlayAnimationAndSound();
    }

    float IdleJumpForce() // Returns double the jump force if slime is affected by jump boost box
    {
        if (!isJumpBoosted)
            return defaultJumpForce;

        isJumpBoosted = false;
        return defaultJumpForce * jumpBoostRate;
    }

    IEnumerator Leap() // Simulates a parabolic leap motion in function of duration and max height
    {
        PlayAnimationAndSound();

        float elapsedTime = 0f;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = isLeapingRight ?
            startPosition + Vector2.right : startPosition + Vector2.left;

        // Simulates the jump throughout a set duration
        while (elapsedTime < leapDuration)
        {
            // Percentage of elapsed time
            float t = elapsedTime / leapDuration;
            // Determines current height based on elapsed time, using sine to achieve a half-egg parabolic motion
            float y = Mathf.Sin(t * Mathf.PI) * leapHeight;

            // Calculates current position based on elapsed time by linear interpolation, adding in the height y to simulate a parabolic movement.
            transform.position = Vector2.Lerp(startPosition, targetPosition, t) + new Vector2(0, y); 

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetPosition = new Vector2(Mathf.Round(targetPosition.x), targetPosition.y);  // Eliminates marginal positional deviations which can accumulate over multiple leaps
        transform.position = targetPosition; 
    }

    // Simulates a boosted leap motion while assuring the slime falls in the appropriate x position regardless whether it's landing on an elevated box or not
    IEnumerator BoostedLeap()
    {
        PlayAnimationAndSound();

        float elapsedTime = 0f;

        Vector2 startPosition = transform.position;
        Vector2 targetPosition = isLeapingRight ?
            startPosition + new Vector2(1, 2): startPosition + new Vector2(-1, 2);

        while (elapsedTime < boostedLeapDuration)
        {
            float t = elapsedTime / boostedLeapDuration;
            float y;

            if (t < 0.66f)
                y = Mathf.Sin(Mathf.PI * t * 0.75f) * 3f;
            else
                y = Mathf.Sin(Mathf.PI * (1.5f * t - 0.5f)) + 2f;

            transform.position = Vector2.Lerp(startPosition, targetPosition - new Vector2(0, 2f), t) + new Vector2(0, y);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    void PlayAnimationAndSound()
    {
        animator.SetTrigger("Jumping");

        if (!GameManager.Instance.Mute)
            audioSource.Play(); // Plays bouncing sound on contact
    }

    // The puzzle is designed in such a way as to have the touched mutable box decrement in size on an 'odd' consecutive bounce, and increment on an 'even' consecutive bounce
    void MutateBox(GameObject box)
    {
        if (oddBounce)
        {
            if (box.transform.localScale.x <= 0.5f)
            {
                Destroy(box);
                return;
            }

            box.transform.localScale -= new Vector3(0.5f, 0.5f);
            box.transform.position += new Vector3(0, 0.16f); // Adjust vertical position to keep it leveled

            // If slime is shielded when first hitting the box, the second consecutive bounce would be an extra odd bounce (i.e. size decreasing bounce) before alternating again
            if (!isShielded)
                oddBounce = false;
            else
            {
                isShielded = false;
                spriteRenderer.color = Color.white;
            }
        }
        else
        {
            box.transform.localScale += new Vector3(0.5f, 0.5f);
            box.transform.position -= new Vector3(0, 0.16f);

            oddBounce = true;
        }

        if (leapPending)
            oddBounce = true; // Given the puzzle's axiom, when you leave a box, it resets: the first touch after you return, is always a size decreasing touch
    }
}