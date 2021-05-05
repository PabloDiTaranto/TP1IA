using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public float range;
    public float angle;
    public LayerMask maskLoS;
    private float distance;

    public bool IsInSight(Transform target)
    {
        Vector3 diff = (target.position - transform.position);
        distance = diff.magnitude;
        if (distance > range) return false;
        if (Vector3.Angle(transform.forward, diff) > angle / 2) return false;
        if (Physics.Raycast(transform.position, diff.normalized, distance, maskLoS)) return false;
        return true;
    }

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * range);
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * range);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * range);
    }
    #endregion
}
