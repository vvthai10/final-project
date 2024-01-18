using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractChest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Interaction;
    public PadlockSystem PadlockSystem;
    private bool IsActive = false;
    private bool InRange = false;
    void Start()
    {
        Interaction.SetActive(false);
        IsActive = false;
        InRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(InRange)
        {
            if(Input.GetKeyDown(KeyCode.E) && !IsActive)
            {
                PadlockSystem.OpenPadlock();
                Interaction.SetActive(false);
                IsActive = true;
                FindAnyObjectByType<DialogueController>().StopAudio();
            } 
            if(Input.GetKeyDown(KeyCode.Space) && IsActive)
            {
                PadlockSystem.ClosePadlock();
                Interaction.SetActive(true);
                IsActive = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Interaction.SetActive(true);
            InRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Interaction.SetActive(false);
            InRange = false;
        }
    }
}
