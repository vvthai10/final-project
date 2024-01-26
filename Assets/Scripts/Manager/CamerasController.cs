using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasController : MonoBehaviour
{
    public static CamerasController instance;
    public Camera[] cameras;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SwitchTo("MainCamera");
    }

    public void SwitchTo(string cameraName)
    {
        foreach (var cam in cameras)
        {
            if (cam.name == cameraName)
            {
                cam.enabled = true;
            }
            else
            {
                cam.enabled = false;
            }
        }
    }

}
