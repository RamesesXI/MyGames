using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
    
    private GameObject otherPortal;
    private float portalWidth;
    private float portalHeight;
    private static bool trigger = true;
    private enum Stance { right, left, up, down }
    private Stance currentStance;
    private Vector3 pos;

    public delegate void Teleportation(Vector2 dir);
    public static event Teleportation OnTeleported;

    public static bool built = false;
    public static bool complete = false;
    private static float waitingTime;

    void Start()
    {
        DetermineStance();

        if (CompareTag("Portal1"))
        {
            built = true;
            waitingTime = Time.time + 3;
        }
        else
        {
            waitingTime += 5;
            built = false;
            complete = true;         
        }
    }

    void Update()
    {
        if (!otherPortal)
        {
            if (Time.time > waitingTime)
            {
                built = false;
                Destroy(gameObject);
                return;
            }

            DetermineStance();
        }
        else
            if (Time.time > waitingTime)
            {
                complete = false;
                Destroy(gameObject);
            }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(Teleport(other));
    }

    IEnumerator Teleport(Collider2D other)
    {
        if (trigger && otherPortal && other.gameObject.GetComponent<Renderer>())
        {
            trigger = false;

            float otherWidth = other.gameObject.GetComponent<Renderer>().bounds.extents.x;
            float otherHeight = other.gameObject.GetComponent<Renderer>().bounds.extents.y;

            Vector3 otherPos = other.gameObject.transform.position;
            float offsetDistance = GetOffsetDistance(otherPos);

            float zAngle = other.gameObject.transform.eulerAngles.z;
            float offsetAngle = GetOffsetAngle(zAngle);

            switch (currentStance)
            {
                case Stance.right:
                    pos.x -= portalWidth + otherWidth;
                    if (!other.gameObject.CompareTag("Player"))
                        pos.y += offsetDistance;
                    zAngle = 180f - offsetAngle;
                    break;

                case Stance.left:
                    pos.x += portalWidth + otherWidth;
                    if (!other.gameObject.CompareTag("Player"))
                        pos.y += offsetDistance;
                    zAngle = offsetAngle;
                    break;

                case Stance.up:
                    if (!other.gameObject.CompareTag("Player"))
                        pos.x += offsetDistance;
                    pos.y -= portalHeight + otherHeight;
                    zAngle = offsetAngle - 90f;
                    break;

                case Stance.down:
                    if (!other.gameObject.CompareTag("Player"))
                        pos.x += offsetDistance;
                    pos.y += portalHeight + otherHeight;
                    zAngle = 90f - offsetAngle;
                    break;
            }
            
            other.gameObject.transform.position = pos;

            if(other.gameObject.CompareTag("Rotatable"))
                other.gameObject.transform.eulerAngles = new Vector3(0f, 0f, zAngle);

            if (other.gameObject.CompareTag("Player"))            
                if (OnTeleported != null)
                    OnTeleported(otherPortal.transform.right);
                                                
            pos = otherPortal.transform.position;

            yield return null;

            trigger = true;
        }
    }

    void DetermineStance()
    { 
        //This method determines the stance of the exiting portal
        if (CompareTag("Portal1"))
            otherPortal = GameObject.FindWithTag("Portal2");
        else
            otherPortal = GameObject.FindWithTag("Portal1");

        if (otherPortal)
        {
            pos = otherPortal.transform.position;
            Quaternion rot = otherPortal.transform.rotation;

            if (rot == Quaternion.Euler(0, 180, 0))
                currentStance = Stance.right;
            else if (rot == Quaternion.Euler(0, 0, 0))
                currentStance = Stance.left;
            else if (rot == Quaternion.Euler(0, 0, -90))
                currentStance = Stance.up;
            else
                currentStance = Stance.down;

            portalWidth = otherPortal.GetComponent<Renderer>().bounds.extents.x;
            portalHeight = otherPortal.GetComponent<Renderer>().bounds.extents.y;
        }
    }

    float GetOffsetAngle(float zAngle)
    {
        Quaternion rot = transform.rotation;
        float offset;

        if (rot == Quaternion.Euler(0, 180, 0))
            offset = zAngle;
        else if (rot == Quaternion.Euler(0, 0, 0))
            offset = 180f - zAngle;
        else if (rot == Quaternion.Euler(0, 0, -90))
            offset = 90f - zAngle;
        else
            offset = zAngle + 90f;

        return offset;
    }

    float GetOffsetDistance(Vector3 otherPos)
    {
        float offset;

        if (transform.eulerAngles.z == 0f)
            offset = otherPos.y - transform.position.y;
        else
            offset = otherPos.x - transform.position.x;

        return offset;
    }

    void OnDestroy()
    {
        GeneralPlayerController.isPushed = false;
        trigger = true;
    }
}