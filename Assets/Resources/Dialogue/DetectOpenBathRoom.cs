using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectOpenBathRoom : MonoBehaviour
{
    private static int count = 0;
    public AudioClip clip;

    // Update is called once per frame
    void Update()
    {
        if(FindAnyObjectByType<PressKeyOpenDoorBathroom>().GetIsBathRoomOpen() && count <= 0)
        {
            count++;
            FindAnyObjectByType<DialogueController>().StopAudio();
            FindAnyObjectByType<DialogueController>().PlayAudio(clip);
        }
    }
}
