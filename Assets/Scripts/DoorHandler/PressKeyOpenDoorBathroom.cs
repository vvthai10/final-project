using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressKeyOpenDoorBathroom : MonoBehaviour
{
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
        Instruction.SetActive(false);
        Action = false;
    }


    public void OnKicked()
    {
        AnimeObject.GetComponent<Animator>().Play("BathRoomOpen");
        IsOpen = true;
    }

    public void OnDoorOpened()
    {
        AnimeObject.GetComponent<Animator>().Play("BathRoomOpen");
        IsOpen = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
