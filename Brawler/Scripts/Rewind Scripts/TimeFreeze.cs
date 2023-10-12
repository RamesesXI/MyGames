using UnityEngine;
using System.Collections;

public class TimeFreeze : MonoBehaviour
{
    [HideInInspector]
    public bool isChanneling = false;

    public LayerMask affectedLayers;

    public IEnumerator LaunchTimeFreeze()
    {
        isChanneling = true;

        yield return new WaitForSeconds(0.5f);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 3f, affectedLayers);

        for (int i = 0; i < hitColliders.Length; i++)
            hitColliders[i].SendMessage("Freeze");

        isChanneling = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}