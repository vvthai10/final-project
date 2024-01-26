using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInClothes : MonoBehaviour
{
    public GameObject textPrompt;
    public GameObject player;
    public Transform teleportInCloset;
    public Transform teleportOutCloset;
    private bool isPlayerInRange = false;
    private bool isInCloset = false;

    void Start()
    {
        textPrompt.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isInCloset)
        {
            player.transform.position = teleportInCloset.transform.position;
            isInCloset = true;
            float rotationDiff = -Quaternion.Angle(transform.rotation, teleportInCloset.rotation);
            rotationDiff += 180;
            player.transform.Rotate(Vector3.up, rotationDiff);
            // GhostController.instance.SwitchState(GhostController.State.Wandering);
        }
        else if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && isInCloset)
        {
            player.transform.position = teleportOutCloset.transform.position;
            isInCloset = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textPrompt.SetActive(true);
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textPrompt.SetActive(false); 
            isPlayerInRange = false;
        }
    }
}
