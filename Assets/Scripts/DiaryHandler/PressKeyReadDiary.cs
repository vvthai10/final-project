using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressKeyReadDiary : MonoBehaviour
{
    public GameObject Interaction;
    public GameObject Diary;
    public GameObject MainCharacter;
    private bool IsActive = false;
    private bool InRange = false;
    // Start is called before the first frame update
    void Start()
    {
        Interaction.SetActive(false);
        Diary.SetActive(false);
        gameObject.SetActive(false);
        IsActive = false;
        InRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(InRange)
        {
            if (Input.GetKeyDown(KeyCode.E) && !IsActive)
            {
                Diary.SetActive(true);
                Interaction.SetActive(false);
                MainCharacter.SetActive(false);
                IsActive = true;
                FindAnyObjectByType<DialogueController>().StopAudio();
            }

            if (Input.GetKeyDown(KeyCode.Space) && IsActive)
            {
                Diary.SetActive(false);
                Interaction.SetActive(true);
                MainCharacter.SetActive(true);
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
