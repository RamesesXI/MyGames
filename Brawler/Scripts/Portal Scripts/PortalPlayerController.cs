using UnityEngine;
using System.Collections;

public class PortalPlayerController : GeneralPlayerController
{
    // Portal Missile variables
    public GameObject portalMissilePrefab;
    private GameObject portalMissleInstance;
    private PortalMissile portalMissileScript;
    private float fireRate_ = 5f;
    private float nextFire_ = 0f;

    // Shield variables
    private BoxCollider2D boxCol;
    private float shieldCooldown = 3f;
    private float shieldCooldownTimer = 0f;
    [HideInInspector]
    public bool isShielded = false;

    // LaserBeam variables
    private LaserBeam laserBeamScript;
    private float laserCooldown = 2f;
    private float laserCooldownTimer = 0f;

    protected override void Start()
    {
        base.Start();

        laserBeamScript = GetComponentInChildren<LaserBeam>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    protected override void Update ()
    {
        base.Update();
        FlipControl();

        //Shield();
        FireLaserBeam();

        JumpControl();
        PrimaryFire();
        FirePortalMissile();
    }

    void FixedUpdate()
    {
        MoveControl();
        
    }

    // Portal
    void FirePortalMissile()
    {
        //Debuffs
        if (isShielded || isBlocked)
            return;

        //Portal conditions
        if (Portal.complete || PortalMissile.missileCount >= 2f || (Portal.built && PortalMissile.missileCount >= 1f))
            return;

        //Launch Missile
        if (Input.GetAxisRaw("Fire2") == 1)
        { 
            if (Time.time > nextFire_)
            {
                nextFire_ = Time.time + (1 / fireRate_);
                LaunchPortalMissile(Quaternion.Euler(0f, 0f, getCursorAngle()) * Vector2.right);
            }
        }
    }

    void LaunchPortalMissile(Vector2 dir)
    {
        portalMissleInstance = Instantiate(portalMissilePrefab, gunMuzzle.position, Quaternion.identity) as GameObject;
        portalMissileScript = portalMissleInstance.GetComponent<PortalMissile>();

        portalMissileScript.direction = dir;
    }
    // !Portal

    
    // Shield
    void Shield()
    {
        //Cooldown
        if (shieldCooldownTimer < shieldCooldown)
        {
            shieldCooldownTimer += Time.deltaTime;

            if (shieldCooldownTimer > shieldCooldown)
                shieldCooldownTimer = shieldCooldown;

            if (!isShielded)
                return;
        }

        //Debuffs
        if (laserBeamScript.isChanneling || isBlocked)
            return;

        //Shielding
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isShielded)
        {
            StartCoroutine(ShieldCo());
            return;
        }

        //Removing Shield before its full duration
        if (Input.GetKeyDown(KeyCode.LeftShift) && isShielded)
        {
            isShielded = false;
            anim.SetBool("isShielded", isShielded);
            ReshapeCollider(isShielded);
            if (!isBlocked)
                rb.isKinematic = false;

            StopCoroutine(ShieldCo());
        }

        //Block movement if grounded
        if (isShielded && grounded)   
            rb.isKinematic = true;
    }

    IEnumerator ShieldCo()
    {
        isShielded = true;
        anim.SetBool("isShielded", isShielded);
        ReshapeCollider(isShielded);
        shieldCooldownTimer = 0f;

        yield return new WaitForSeconds(4f);

        isShielded = false;
        anim.SetBool("isShielded", isShielded);
        ReshapeCollider(isShielded);
        if(!isBlocked)
            rb.isKinematic = false;
    }

    void ReshapeCollider(bool isSielded)
    {
        if (isShielded)
        {
            boxCol.offset = new Vector2(-0.0006f, -0.01f);
            boxCol.size = new Vector2(0.759f, 1.501f);
        }
        else
        {
            boxCol.offset = new Vector2(-0.07f, -0.006f);
            boxCol.size = new Vector2(0.9f, 1.469f);
        }
    }
    // !Shield

    
    // LaserBeam
    void FireLaserBeam()
    {
        //Cooldown
        if (laserCooldownTimer < laserCooldown)
        {
            laserCooldownTimer += Time.deltaTime;

            if (laserCooldownTimer > laserCooldown)
                laserCooldownTimer = laserCooldown;

            return;
        }

        //Debuffs
        if (isShielded || isBlocked)
            return;

        //Fire LaserBeam
        if (Input.GetKeyDown(KeyCode.E))
        {
            laserCooldownTimer = 0f;
            StartCoroutine(laserBeamScript.Laser());
        }
    }
    // !LaserBeam
    

    // Debuff Control
    protected override void MoveControl()
    {
        if (isShielded || laserBeamScript.isChanneling || isBlocked)
            return;

        base.MoveControl();
    }

    protected override void JumpControl()
    {
        if (isShielded || laserBeamScript.isChanneling || isBlocked)
            return;

        base.JumpControl();
    }

    protected override void FlipControl()
    {
        if (isShielded || laserBeamScript.isChanneling || isBlocked)
            return;

        base.FlipControl();
    }

    protected override void PrimaryFire()
    {
        if (isShielded || laserBeamScript.isChanneling || isBlocked)
            return;

        base.PrimaryFire();
    }
    // !Debuff Control
}