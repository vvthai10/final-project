using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour {

	public Transform playerCamera;
	public Transform portal;
	public Transform otherPortal;
    public GameObject triggerPortal;

    // Update is called once per frame
    void Update () {
        Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;

        // Invert the Z-component to achieve backward movement when the main camera moves forward
        // playerOffsetFromPortal.z = -playerOffsetFromPortal.z;
        Vector3 valUpdate = portal.position + playerOffsetFromPortal;
        /*if(CheckPlayerInTrigger.onTrigger == "DoorA")
        {
            if (valUpdate.z <= -15.2f)
            {
                valUpdate.z = -15.2f;
            }
            if (valUpdate.x <= 11.7f)
            {
                valUpdate.Set(11.72f, valUpdate.y, valUpdate.z);
            }
            else if (valUpdate.x >= 12.3f)
            {
                valUpdate.Set(12.28f, valUpdate.y, valUpdate.z);
            }
            transform.position = valUpdate;
        }
        else if (CheckPlayerInTrigger.onTrigger == "DoorB")
        {
            if (valUpdate.z <= -3.5f)
            {
                valUpdate.z = -3.5f;
            }
            transform.position = valUpdate;
        }*/
        // transform.position = valUpdate;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);

        Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
        // Flip the camera direction by 180 degrees around the y-axis
        newCameraDirection = Quaternion.Euler(0f, 180f, 0f) * newCameraDirection;

        // transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
