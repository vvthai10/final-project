using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PressKeyOpenDoorBathroom : MonoBehaviour
{
    [Header("VR Settings: ")]
    [SerializeField]
    private InputActionProperty interactDoor;

    public GameObject Instruction;
    public GameObject AnimeObject;
    public GameObject ThisTrigger;
    // public AudioSource DoorOpenSound;
    public bool Action = false;
    private bool IsOpen = false;

    void Start()
    {
        Instruction.SetActive(false);

    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            Instruction.SetActive(true);
            Action = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            Instruction.SetActive(false);
            Action = false;
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || (interactDoor != null && interactDoor.action.ReadValue<float>() != 0))
        {
            if (Action == true)
            {
                if(!IsOpen)
                {
                    IsOpen = true;
                    // Instruction.SetActive(false);
                    AnimeObject.GetComponent<Animator>().Play("BathRoomOpen");
                    // ThisTrigger.SetActive(false);
                    // DoorOpenSound.Play();
                    Action = false;
                }
                else
                {
                    IsOpen = false;
                    // Instruction.SetActive(false);
                    AnimeObject.GetComponent<Animator>().Play("BathRoomClose");
                    // ThisTrigger.SetActive(false);
                    // DoorOpenSound.Play();
                    Action = false;
                }
            }
        }

    }
}
