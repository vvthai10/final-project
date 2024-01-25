using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressKeyOpenDoorBathroom : MonoBehaviour
{
    public GameObject Instruction;
    public GameObject AnimeObject;
    public GameObject ThisTrigger;
    public GameObject PadLock;
    public GameObject PadLockCanvas;
    public GameObject MainCharacter;
    // public AudioSource DoorOpenSound;
    public bool Action = false;
    private bool IsOpen = false;
    private bool IsLock = true;
    private bool IsCanvasActive = false;

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


    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space) && IsCanvasActive)
        {
            FindAnyObjectByType<PassCode>().Delete();
            IsCanvasActive = false;
            MainCharacter.SetActive(true);
            PadLockCanvas.SetActive(false);
            return;
        } 

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(IsLock && Action)
            {
                PadLockCanvas.SetActive(true);
                IsCanvasActive = true;
                MainCharacter.SetActive(false);
                return;
            }

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

    public void Unlock()
    {
        IsLock = false;
        IsCanvasActive = false;
        PadLockCanvas.SetActive(false);
        PadLock.SetActive(false);
        MainCharacter.SetActive(true);
    }
}
