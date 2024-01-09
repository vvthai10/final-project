using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    public Transform target;
    public bool excludeX = false;
    public bool excludeY = false;
    public bool excludeZ = false;

    void Update()
    {
        Vector3 eulerAngles = target.rotation.eulerAngles;
        if (excludeX)
        {
            eulerAngles.x = 0f;
        }
        if (excludeY)
        {
            eulerAngles.y = 0f;
        }
        if (excludeZ)
        {
            eulerAngles.z = 0f;
        }
        transform.rotation = Quaternion.Euler(eulerAngles);
    }
}
