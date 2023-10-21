using UnityEngine;
using System.Collections;

public class SlimeControle : MonoBehaviour {
    public bool canMove;
    public float forcejump;
    public float hopHeight = 1f;
    public float JumpTime = 0.5f;

    internal SpriteRenderer sr;
    internal Animator animator;
    internal AudioSource jump_aduio_src;

    //public Sprite BlueSprite;
    //public Sprite GreenSprite;
    //public Sprite DeadBlueSprite;
    //public Sprite DeadGreenSprite;

    public const float UpperLayer =0, DownLayer = -2, MidLayer = -1;

    private bool facingRight;

    bool shielded = false;
    //Shielded
    BoxControl last_hit_box_controller = null;
    private bool hopping = false;

    public bool HasShield
    {
        get
        {
            return shielded;
        }
        set
        {
            shielded = value;
            animator.SetBool("Shielded", shielded);
            //if (shielded)
            //    sr.sprite = GreenSprite;
            //else sr.sprite = BlueSprite;
        }
    }

    private bool HasHitBox = false;

    void Start ()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        HasShield = false;
        PlayerPrefs.SetString("played","1");
        PlayerPrefs.Save();
        jump_aduio_src = GetComponent<AudioSource>();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && canMove)
        {
            float target_y;

            if (transform.position.y >= (UpperLayer + 0.32f))
                target_y = UpperLayer;
            else if (transform.position.y >= (MidLayer + 0.32f))
                target_y = MidLayer;
            else
                target_y = DownLayer;

            StartCoroutine(Hop(new Vector3((int)transform.position.x + 1, target_y, 0), JumpTime));

            canMove = false;
            GetComponent<SpriteRenderer>().flipX = false;
            if (last_hit_box_controller != null)
                last_hit_box_controller.Jumped = false;

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && canMove)
        {
            float target_y = 0;
            if (transform.position.y >= (UpperLayer + 0.32f))
                target_y = UpperLayer;
            else if (transform.position.y >= (MidLayer + 0.32f))
                target_y = MidLayer;
            else
                target_y = DownLayer;

            StartCoroutine(Hop(new Vector3((int)transform.position.x - 1, target_y, 0), JumpTime));

            canMove = false;
            GetComponent<SpriteRenderer>().flipX = true;

            if (last_hit_box_controller != null)
                last_hit_box_controller.Jumped = false;
        }
        /*  if (Input.touchCount > 0)
          {
              var touch = Input.GetTouch(0);
              if (touch.position.x < Screen.height / 2 && canMove)
              {
                // left
                  float target_y = 0;
                  if (transform.position.y >= (UpperLayer + 0.32f))
                      target_y = UpperLayer;
                  else if (transform.position.y >= (MidLayer + 0.32f))
                      target_y = MidLayer;
                  else
                      target_y = DownLayer;
                  StartCoroutine(Hop(new Vector3((int)transform.position.x - 1, target_y, 0), JumpTime));



                  canMove = false;
                  GetComponent<SpriteRenderer>().flipX = true;


                  if (last_hit_box_controller != null)
                      last_hit_box_controller.Jumped = false;
              }
              else if (touch.position.x > Screen.height / 2 && canMove)
              {
                  //right
                  float target_y = 0;

                  if (transform.position.y >= (UpperLayer + 0.32f))
                      target_y = UpperLayer;
                  else if (transform.position.y >= (MidLayer + 0.32f))
                      target_y = MidLayer;
                  else
                      target_y = DownLayer;

                  StartCoroutine(Hop(new Vector3((int)transform.position.x + 1, target_y, 0), JumpTime));

                  canMove = false;
                  GetComponent<SpriteRenderer>().flipX = false;
                  if (last_hit_box_controller != null)
                      last_hit_box_controller.Jumped = false;
              }
          }*/

    }

    private void Flip(float horizontal)
    {
        if(horizontal < 0 && !facingRight || horizontal > 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;
           
            transform.localScale = theScale;  
        }
    }


    IEnumerator Hop(Vector3 dest, float time)
    {
        if (hopping)
            yield break;
        HasHitBox = false;
        hopping = true;
        var startPos = transform.localPosition;

        var timer = 0f;
      
        while (timer <= 1.0f && !HasHitBox)
        {
            var height = Mathf.Sin(Mathf.PI * timer) * hopHeight;
            transform.localPosition = Vector3.Lerp(startPos, dest, timer) + Vector3.up * height;

            timer += Time.deltaTime / time;
            yield return null;
        }

        transform.localPosition = new Vector3(dest.x, transform.localPosition.y);
        hopping = false;
    }

    public bool DoubleJump = false;
    public float StartedJumpTime;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "box")
        {
            StartedJumpTime = Time.time;
            HasHitBox = true;
            if (col.gameObject.name == "Red")
            {
                if (!col.gameObject.GetComponent<LevelManager>().GoToNextLevelIfPossible())
                {
                    col.gameObject.GetComponent<BoxJumpController>().Jump(this, GetComponent<Rigidbody2D>(), forcejump);
                    canMove = true;
                    
                }
                return;
            }

            last_hit_box_controller = col.gameObject.GetComponent<BoxControl>();
            
            if (last_hit_box_controller != null)
            {
                last_hit_box_controller.PlayerHitBox(this.gameObject);

                if (HasShield && last_hit_box_controller != null)
                {
                    last_hit_box_controller.Jumped = false;
                    HasShield = false;
                }
            }
            else if (col.gameObject.GetComponent<BoxJumpController>().BoxKind == BoxType.Shield)
            {
                HasShield = true;

                col.gameObject.GetComponent<BoxJumpController>().Jump(this,GetComponent<Rigidbody2D>(), forcejump);
                Destroy(col.gameObject);
                canMove = true;
                return;
            }

            col.gameObject.GetComponent<BoxJumpController>().Jump(this, GetComponent<Rigidbody2D>(), forcejump);
            canMove = true;

        }
    }
}
