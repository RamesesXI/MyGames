using UnityEngine;
using System.Collections;

public class DashPlayerController : GeneralPlayerController
{
    //Dash variables
    private enum DashState { Ready, Dashing, Cooldown }
    private DashState dashState = DashState.Ready;

    public float dashSpeed;
    public float dashTime;
    public float chargeCooldown;

    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    private float chargeTimer = 0f;

    private Vector2 dashDir;
    private float chargeCount = 3f;

    //Rewind variables
    private RewindController rewindScript;
    private float rewindCooldown = 3f;
    private float rewindCooldownTimer = 0f;

    //Time Freeze variables
    private TimeFreeze timeFrScript;
    private float timeFrCooldown = 5f;
    private float timeFrCooldownTimer = 0f;


    protected override void Start()
    {
        base.Start();

        rewindScript = GetComponent<RewindController>();
        timeFrScript = GetComponent<TimeFreeze>();
    }

    protected override void Update()
    {
        base.Update();

        Rewind();
        //TimeFreeze();

        PrimaryFire();
        JumpControl();
    }

    void FixedUpdate ()
    {
        Dashing();

        MoveControl();
        FlipControl();
    }

    void Dashing()
    {
        //Cooldown
        if (chargeCount < 3f)
        {
            chargeTimer += Time.fixedDeltaTime;

            if (chargeTimer >= chargeCooldown)
            {
                chargeTimer = 0f;
                chargeCount++;
                //Debug.Log(chargeCount);
            }
        }

        //Debuffs
        if (isBlocked || rewindScript.isRewinding || timeFrScript.isChanneling)
        {
            dashState = DashState.Cooldown;
            return;
        }

        //Dash Cycle
        switch (dashState)
        {
            case DashState.Ready:
                if (Input.GetAxisRaw("Fire2") > 0f)
                {
                    rb.gravityScale = 0f;
                    dashDir = new Vector2(getCursorPosition().x - transform.position.x, getCursorPosition().y - transform.position.y).normalized;
                    rb.velocity = dashDir * dashSpeed;

                    chargeCount--;
                    //Debug.Log(chargeCount);
                    dashState = DashState.Dashing;
                }
                break;

            case DashState.Dashing:
                dashTimer += Time.fixedDeltaTime;
                if (dashTimer >= dashTime)
                {
                    dashTimer = 0f;
                    rb.gravityScale = 1f;
                    rb.velocity = Vector2.zero;

                    dashState = DashState.Cooldown;
                }
                break;

            case DashState.Cooldown:
                cooldownTimer += Time.fixedDeltaTime;
                if (cooldownTimer >= 0.1f && chargeCount > 0f)
                {
                    cooldownTimer = 0f;
                    dashTimer = 0f;
                    dashState = DashState.Ready;
                }
                break;
        }
    }

    void Rewind()
    {
        //Cooldown
        if (rewindCooldownTimer < rewindCooldown)
        {
            rewindCooldownTimer += Time.deltaTime;

            if (rewindCooldownTimer > rewindCooldown)
                rewindCooldownTimer = rewindCooldown;

            return;
        }

        //Debuffs
        if (dashState == DashState.Dashing || isBlocked)
            return;

        //Rewind
        if (Input.GetKeyDown(KeyCode.E))
        {
            rewindCooldownTimer = 0f;
            StartCoroutine(rewindScript.Rewind());
        }
    }

    void TimeFreeze()
    {
        //Cooldown
        if(timeFrCooldownTimer < timeFrCooldown)
        {
            timeFrCooldownTimer += Time.deltaTime;

            if (timeFrCooldownTimer > timeFrCooldown)
                timeFrCooldownTimer = timeFrCooldown;

            Debug.Log(timeFrCooldownTimer);

            return;
        }

        //Debuffs
        if (dashState == DashState.Dashing || rewindScript.isRewinding || isBlocked)
            return;

        //Launch TimeFreeze
        if (Input.GetKeyDown(KeyCode.Q))
        {
            timeFrCooldownTimer = 0f;
            StartCoroutine(timeFrScript.LaunchTimeFreeze());
        }
    }

    // Debuff Control
    protected override void MoveControl()
    {
        if (dashState == DashState.Dashing || rewindScript.isRewinding || isBlocked || timeFrScript.isChanneling)
            return;

        base.MoveControl();
    }

    protected override void JumpControl()
    {
        if (dashState == DashState.Dashing || rewindScript.isRewinding || isBlocked || timeFrScript.isChanneling)
            return;

        base.JumpControl();
    }

    protected override void FlipControl()
    {
        if (dashState == DashState.Dashing || rewindScript.isRewinding || isBlocked || timeFrScript.isChanneling)
            return;

        base.FlipControl();
    }

    protected override void PrimaryFire()
    {
        if (dashState == DashState.Dashing || rewindScript.isRewinding || isBlocked || timeFrScript.isChanneling)
            return;

        base.PrimaryFire();
    }
    // !Debuff Control

    //Block immunity
    protected override void Freeze()
    {

    }
}
