using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerAtDoor : MonoBehaviour
{
    private DialogueController dialogueController;
    // Start is called before the first frame update
    void Start()
    {
        dialogueController = FindAnyObjectByType<DialogueController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            dialogueController.onNextLoop();
        }
    }
}
