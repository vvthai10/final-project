using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PressKeyOpenDoor : MonoBehaviour
{
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
        Instruction.SetActive(false);
        Action = false;
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
                    if(AnimeObject) {
                        AnimeObject.GetComponent<Animator>().Play("DoorOpen");
                    }
                    if (OtherAnimeObject) {
                        OtherAnimeObject.GetComponent<Animator>().Play("DoorOpen");
                    }
                    // ThisTrigger.SetActive(false);
                    // DoorOpenSound.Play();
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
                }
            }
        }

    }
}