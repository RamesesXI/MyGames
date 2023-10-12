using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class LaserBeam : MonoBehaviour
{
    private LineRenderer line;
    public Material preBeamMaterial;
    public Material beamMaterial;

    private Vector3 direction;
    private RaycastHit2D hit;
    private float range = 100.0f;
    public LayerMask collisionMask;

    public bool isChanneling = false;

    private PortalPlayerController portalPlayerScript;
    private Rigidbody2D rb;

    void Start()
    {
        portalPlayerScript = GetComponentInParent<PortalPlayerController>();
        rb = GetComponentInParent<Rigidbody2D>();

        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.material = preBeamMaterial;
        line.startWidth = 0.08f;
        line.endWidth = 0.08f;
        line.sortingLayerName = "Rope";
    }

    public IEnumerator Laser()
    {
        isChanneling = true;

        direction = getCursorPosition() - transform.position;
        hit = Physics2D.Raycast(transform.position, direction, range, collisionMask);

        rb.isKinematic = true;
        rb.simulated = false;

        line.enabled = true;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hit.point);

        yield return new WaitForSeconds(0.5f);

        line.material = beamMaterial;
        line.startWidth = 0.3f;
        line.endWidth = 0.3f;

        yield return new WaitForSeconds(0.5f);

        line.enabled = false;
        isChanneling = false;

        if (!portalPlayerScript.isBlocked)
        {
            rb.isKinematic = false;
            rb.simulated = true;
        }

        line.material = preBeamMaterial;
        line.startWidth = 0.03f;
        line.endWidth = 0.03f;
    }

    Vector3 getCursorPosition()
    {
        Vector3 cursorPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);
        return cursorPosition;
    }

    private void OnDestroy()
    {
        line.enabled = false;
        isChanneling = false;
    }
}
