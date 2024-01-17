using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasManager : MonoBehaviour
{
    private static Camera[] cameras; 
    public Camera[] initialCameras;

    private void Start()
    {
        cameras = initialCameras;
        SwitchTo("MainCamera");
    }

    public static void SwitchTo(string cameraName)
    {
        foreach (var camera in cameras)
        {
            if (camera.name == cameraName)
            {
                camera.enabled = true;
            }
            else
            {
                camera.enabled = false;
            }
        }
    }
}
