using System.Collections;
using UnityEngine;

public class GhostVisionField : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;

    public Transform fovOrigin;
    public Transform target;


    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    

    private bool fovRoutineBool = true;
    public Vector3 lastSeenPosition;

    public void RenewFOVCoroutine()
    {
        StopAllCoroutines();
        StartCoroutine(FOVRoutine());
    }

    public void StopFOVRoutine()
    {
        this.fovRoutineBool = false;
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);


        while (fovRoutineBool)
        {
            yield return wait;
            FOVCheck();
        }
    }

    public bool CanSeePlayer()
    {
        return canSeePlayer;
    }

    private void FOVCheck()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        //Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (distanceToTarget <= radius)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                if (!Physics.Raycast(fovOrigin.transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    lastSeenPosition = target.position;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    public Vector3 LastSeenPosition()
    {
        return lastSeenPosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        Gizmos.DrawWireSphere(transform.position, radius);
        Vector3 viewAngle1 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + viewAngle1 * radius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngle2 * radius);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}

