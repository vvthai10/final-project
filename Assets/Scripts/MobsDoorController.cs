using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MobsDoorController : MonoBehaviour
{
    public UnityEvent onDoorKicked;
    public UnityEvent onDoorOpened;
    public Transform insidePoint;
    public Transform outsidePoint;
}
