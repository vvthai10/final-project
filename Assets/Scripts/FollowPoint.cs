using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPoint : MonoBehaviour
{
    public Transform point;

    void Update()
    {
        transform.position = point.position;
    }
}
