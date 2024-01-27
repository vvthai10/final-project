using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour {

	public Transform player;
	public Transform reciever;

	private bool playerIsOverlapping = false;

	public GameObject DoorA;
    public GameObject DoorB;

	private int cnt = 0;
	public Texture[] listDeadlySin;
	public Renderer picture;

	[SerializeField] private GameObject fatherObjects;
    private void Start()
    {
		picture.material.mainTexture = listDeadlySin[cnt];
    }

    // Update is called once per frame
    void Update () {
		if (playerIsOverlapping)
		{
			Vector3 portalToPlayer = player.position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				// Teleport him!
				float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
				rotationDiff += 180;
				player.Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
				player.position = reciever.position + positionOffset;

				playerIsOverlapping = false;
				DoorA.GetComponent<Animator>().Play("DoorClose");
				DoorB.GetComponent<Animator>().Play("DoorClose");
				cnt += 1;
                picture.material.mainTexture = listDeadlySin[cnt];
				Debug.Log("Number Of Passes: " + cnt);
				if(fatherObjects && cnt == 8)
				{
					fatherObjects.SetActive(true);
				}
            }
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = false;
		}
	}

	
}
