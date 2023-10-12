using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{ 
    Rigidbody2D rb;
    GameObject childRing; //Selction indicator

    // Movement variables
    bool isMoving = false;
    public float moveSpeed;
    const float idleSpeed = 0.025f;
    Vector3 targetPosition;
    float lastSqrMag = Mathf.Infinity;

    [HideInInspector]
    public bool isDamaged = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
	}

    void Start()
    {
        Invoke("NoDrag", 4.5f);
        InvokeRepeating("IdleMovement", Random.Range(4.5f, 5f), Random.Range(1.25f, 1.75f));

        childRing = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (isMoving)
        {
            // check the current sqare magnitude
            float sqrMag = (targetPosition - transform.position).sqrMagnitude;

            // if rigidbody has reached target and is now moving past it
            if (sqrMag > lastSqrMag)
            {
                rb.velocity = Vector2.zero;
                isMoving = false;
            }

            lastSqrMag = sqrMag;
        }
    }

    //This function gets called by Base script when spawning
    public void InitialDirction(Vector2 velocity)
    {
        rb.AddForce(velocity, ForceMode2D.Impulse);
    }

    void NoDrag()
    {
        rb.drag = 0f;
    }

    void IdleMovement()
    {
        if (!isMoving)
        {
            Vector2 dir = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)) * Vector2.right;
            rb.velocity = dir * idleSpeed; // Sets velocity to a randomly generated direction
        }
    }

    public void MoveToPosition(Vector3 targetPos)
    {
        // In case unit is moved before
        if (rb.drag > 0)
            rb.drag = 0;

        rb.velocity = (targetPos - transform.position).normalized * moveSpeed; // Set direction based on targetPosition

        // Set variables for Update to check when to stop
        targetPosition = targetPos;
        lastSqrMag = Mathf.Infinity;
        isMoving = true;
    }

    public void Selected()
    {
        childRing.SetActive(true);
    }

    public void Deselected()
    {
        childRing.SetActive(false);
    }

    void OnDestroy()
    {
        if (Selection.unitsSelected)
            Selection.OnUnitDestroyed(tag, this);
    }
}
