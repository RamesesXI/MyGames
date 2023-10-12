#define DEBUG_NINJA_ROPE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class NinjaRope: MonoBehaviour
{
    private static int criticalIterationCount = 1000; // Debug, useful with DEBUG_NINJA_ROPE

    private Rigidbody2D rigidbody_;
    private LineRenderer lineRenderer_;
    private BoxCollider2D collider_;

    [System.Serializable]
    public class RopeAnchor
    {
        public Vector3 anchor; // The position of the anchor
        public float side; // The side of the perpendicular comparison

        public RopeAnchor(Vector3 a, float s)
        {
            anchor = a;
            side = s;
        }
    }

    private List<RopeAnchor> anchors_ = new List<RopeAnchor>();

    public GameObject baseEntity; // The hook
    public GameObject endEntity; // The player

    public float ropeWidth = 0.12f; // The width of the rope
    public float ropeLength = 3f; // The length

    public float raycastStep = 10f;
    public LayerMask collisionMask;

    public RopeAnchor baseAnchor          { get { return anchors_[0]; }                     set { anchors_[0] = value; } }
    public RopeAnchor postFirstAnchor     { get { return anchors_[1]; }                     set { anchors_[1] = value; } }
    public RopeAnchor postPostFirstAnchor { get { return anchors_[2]; }                     set { anchors_[2] = value; } }
    public RopeAnchor preLastAnchor       { get { return anchors_[anchors_.Count - 3]; }    set { anchors_[anchors_.Count - 3] = value; } }
    public RopeAnchor lastAnchor          { get { return anchors_[anchors_.Count - 2]; }    set { anchors_[anchors_.Count - 2] = value; } }
    public RopeAnchor targetAnchor        { get { return anchors_[anchors_.Count - 1]; }    set { anchors_[anchors_.Count - 1] = value; } }

    protected void Start()
    {
        initRigidbody_();

        lineRenderer_ = gameObject.GetComponent<LineRenderer>();
        //lineRenderer_.SetWidth(ropeWidth, ropeWidth);  ..Obsolete
        lineRenderer_.startWidth = ropeWidth;
        lineRenderer_.endWidth = ropeWidth;
        lineRenderer_.sortingLayerName = "Rope";

        anchors_.Add(new RopeAnchor(baseEntity.transform.position, 0f));
        anchors_.Add(new RopeAnchor(endEntity.transform.position, 0f));

        updateJoint_();
    }
	
	protected void Update()
    {
        baseAnchor = new RopeAnchor(baseEntity.transform.position, 0f);
        targetAnchor = new RopeAnchor(endEntity.transform.position, 0f);

        popAnchors_();                   
        updateRaycastCollision_();       //here
        updateJoint_();
        updateRendering_();
    }


#if DEBUG_NINJA_ROPE
    void OnDrawGizmos() {
        float   opacity = 2f / 3f;
        Color   baseColor = Gizmos.color;

        Gizmos.color = new Color(0f, 0.5f, 1f, opacity);
        for (int i = 1 ; anchors_.Count - 1 > i ; ++i) {
            Gizmos.DrawSphere(anchors_[i].anchor, ropeWidth);
        }

        Gizmos.color = new Color(1f, 0.5f, 0f, opacity);
        Gizmos.DrawSphere(baseAnchor.anchor, ropeWidth);
        Gizmos.color = new Color(1f, 0f, 0.5f, opacity);
        Gizmos.DrawSphere(targetAnchor.anchor, ropeWidth);

        Gizmos.color = baseColor;
    }
#endif

    // Initialization
    private void initRigidbody_()
    {
        // Rigidbody is necessary for the joint collision to work
        rigidbody_ = gameObject.GetComponent<Rigidbody2D>();
        if (!rigidbody_)
            rigidbody_ = gameObject.AddComponent<Rigidbody2D>();

        rigidbody_.mass = float.MaxValue;

        rigidbody_.interpolation = RigidbodyInterpolation2D.None;
        rigidbody_.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigidbody_.sleepMode = RigidbodySleepMode2D.StartAwake;

        rigidbody_.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void initCollider_()
    {
        collider_ = gameObject.GetComponent<BoxCollider2D>();
        if (!collider_)
            collider_ = gameObject.AddComponent<BoxCollider2D>();

        collider_.isTrigger = false;

        collider_.offset = Vector2.zero;
        collider_.size = new Vector2(ropeWidth / 2f, 0f);
    }
    // !Initialization


    // Update
    private void updateRaycastCollision_()
    {
        int loopCount = 0;
        int realLoopCount = 0;
        Vector3 prevPos = lastAnchor.anchor;

        do {
            Vector3 linecastTarget = prevPos + (targetAnchor.anchor - lastAnchor.anchor) / raycastStep;

            // Check that the linecast is still being done in bounds of the last anchor -> target anchor vector
            // This is when lincastTarget gets one raycastStep beyond targetAnchor
            if (Vector3.Dot(targetAnchor.anchor - prevPos, targetAnchor.anchor - linecastTarget) <= 0f)
                linecastTarget = targetAnchor.anchor;

            RaycastHit2D rch = Physics2D.Linecast(prevPos, linecastTarget, collisionMask);
            if (!rch || Vector3.Distance(prevPos, rch.point) <= 0.0001f)
                rch = Physics2D.Linecast(linecastTarget, prevPos, collisionMask);


            if (rch && Vector3.Distance(prevPos, rch.point) > 0.0001f)
            {
                Vector3 point = rch.point;

                // We get the angle between the latest segment and the anchor-middle of platform
                float   angle = Mathf.DeltaAngle(pointsToAngle(point - lastAnchor.anchor, point - rch.collider.gameObject.transform.position), pointsToAngle(point, lastAnchor.anchor));
                float   side = angle >= 0f ? 1f : -1f;

                // Finally, we add the anchor to the list
                addAnchor_(new RopeAnchor(point, side));

                prevPos = point;
                loopCount = 0;
            }
            else
            {
                prevPos += (targetAnchor.anchor - lastAnchor.anchor) / raycastStep; // We try to move the previous position forward a bit
                // Note that it means the collision detection gets less and less accurate the further we get to the target anchor
                ++loopCount;
            }

            ++realLoopCount;
        } while (Mathf.Sign(lastAnchor.anchor.x - targetAnchor.anchor.x) == Mathf.Sign(prevPos.x - targetAnchor.anchor.x) &&
                 Mathf.Sign(lastAnchor.anchor.y - targetAnchor.anchor.y) == Mathf.Sign(prevPos.y - targetAnchor.anchor.y)
#if DEBUG_NINJA_ROPE
                 && criticalIterationCount > realLoopCount
#endif
        );

#if DEBUG_NINJA_ROPE
        //if (criticalIterationCount == realLoopCount)
            //Debug.Log("NinjaRope: Critical iteration count (" + criticalIterationCount + ", loop count was " + loopCount + ") attained");
#endif
    }

    private void popAnchors_()
    {
        bool goOn = true;

        while (anchors_.Count > 2 && goOn == true)
        {
            goOn = false;

            // Calculate the dot product of the vector perpendicular to the anchor
            // and the actual segment vector. If it's greater than zero, it means
            // they are kind of facing the same way, so the anchor can be safely removed
            Vector3 prev = lastAnchor.anchor - preLastAnchor.anchor;
            Vector3 act = targetAnchor.anchor - lastAnchor.anchor;
            Vector3 prevPerpendicular = Quaternion.Euler(0f, 0f, 90f * lastAnchor.side) * prev;

            if (Vector3.Dot(prevPerpendicular, act) > 0f)
            {
                lastAnchor = targetAnchor;
                anchors_.RemoveAt(anchors_.Count - 1);

                goOn = true;
            }
        }

        goOn = true;
        while (anchors_.Count > 2 && goOn == true)
        {
            // Same goes from the beginning of the rope, because the base of the rope (the hook) could be moving
            goOn = false;

            Vector3 prev = postFirstAnchor.anchor - baseAnchor.anchor;
            Vector3 act = postPostFirstAnchor.anchor - postFirstAnchor.anchor;
            Vector3 prevPerpendicular = Quaternion.Euler(0f, 0f, 90f * postFirstAnchor.side) * prev;

            if (0f <= Vector3.Dot(prevPerpendicular, act))
            {
                postFirstAnchor = postPostFirstAnchor;
                anchors_.RemoveAt(2);

                goOn = true;
            }
        }
    }

    private void updateJoint_()
    {
        DistanceJoint2D joint = gameObject.GetComponent<DistanceJoint2D>();
        if (!joint)
            joint = gameObject.AddComponent<DistanceJoint2D>();

        if (ropeLength < float.MaxValue - 0.001f)
            joint.maxDistanceOnly = false;
        else
            joint.maxDistanceOnly = true;

        joint.anchor = gameObject.transform.InverseTransformPoint(lastAnchor.anchor);
        joint.connectedBody = endEntity.GetComponent<Rigidbody2D>();
        joint.connectedAnchor = endEntity.transform.InverseTransformPoint(targetAnchor.anchor);

        float distance = ropeLength - getStableRopeLength_();
        if (distance < 0f)
            distance = 0f;
        joint.distance = distance;
    }

    private void updateRendering_()
    {
        //lineRenderer_.SetVertexCount(anchors_.Count);   ..Obsolete
        lineRenderer_.positionCount = anchors_.Count;

        for (int i = 0 ; anchors_.Count > i ; ++i)
            lineRenderer_.SetPosition(i, anchors_[i].anchor);
    }

    private void addAnchor_(RopeAnchor anc)
    {
        anchors_.Insert(anchors_.Count - 1, anc);
    }
    // !Update


    // Utils
    private static float pointsToAngle(Vector2 anc1, Vector2 anc2) {
        // Returns the angle between both vectors; note that the angle is NOT between -180 and 180
        return (Mathf.Atan2(anc2.y - anc1.y, anc2.x - anc1.x) * Mathf.Rad2Deg - 90f);
    }
   
    private float getStableRopeLength_()
    {
        float dist = 0f;
        for (int i = 0 ; anchors_.Count - 1 > i + 1 ; ++i) // Note: -1 because we don't want to account for the target
            dist += Vector3.Distance(anchors_[i].anchor, anchors_[i + 1].anchor);
        return dist;
    }

    // Different from the previous one because it also accounts for the last chunk, ending with the target
    public float getRopeLength()
    {
        float dist = 0f;
        for (int i = 0 ; anchors_.Count > i + 1 ; ++i)
            dist += Vector3.Distance(anchors_[i].anchor, anchors_[i + 1].anchor);
        return dist;
    }

    public void setLength(float length)
    {
        ropeLength = length;

        DistanceJoint2D joint = gameObject.GetComponent<DistanceJoint2D>();
        if (joint)
            joint.maxDistanceOnly = false;
    }

    public void removeLength()
    {
        ropeLength = float.MaxValue;

        DistanceJoint2D joint = gameObject.GetComponent<DistanceJoint2D>();
        if (joint)
            joint.maxDistanceOnly = true;
    }
    // !Utils
}
