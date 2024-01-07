using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasManager : MonoBehaviour
{
    public static Camera[] cameras;
    public Camera[] initCameras;

    private void Awake()
    {
        cameras = initCameras;
    }

    public static void SwitchTo(string cameraName)
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
