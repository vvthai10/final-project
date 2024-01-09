using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PressKeyOpenDoor : MonoBehaviour
{
    [Header("VR Settings: ")]
    [SerializeField]
    private InputActionProperty interactDoor;

    public GameObject Instruction;
    public GameObject AnimeObject;
    public GameObject OtherAnimeObject;
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
        if (collision.CompareTag("Player"))
        {
            Instruction.SetActive(false);
            Action = false;
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Instruction.SetActive(true);
    //    }
    //}


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
                    if(AnimeObject) {
                        AnimeObject.GetComponent<Animator>().Play("DoorOpen");
                    }
                    if (OtherAnimeObject) {
                        OtherAnimeObject.GetComponent<Animator>().Play("DoorOpen");
                    }
                    // ThisTrigger.SetActive(false);
                    // DoorOpenSound.Play();
                    Action = false;
                }
                else
                {
                    IsOpen = false;
                    // Instruction.SetActive(false);
                    if(AnimeObject) {
                        AnimeObject.GetComponent<Animator>().Play("DoorClose");
                    }
                    if (OtherAnimeObject) {
                        OtherAnimeObject.GetComponent<Animator>().Play("DoorClose");
                    }
                    // ThisTrigger.SetActive(false);
                    // DoorOpenSound.Play();
                    Action = false;
                }
            }
        }

    }
}