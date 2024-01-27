using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectOpenBathRoom : MonoBehaviour
{
    private static int count = 0;
    public AudioClip clip;
    [SerializeField] private GameObject _ghost;
    private bool flag = false;
    private DateTime _start;
    // Update is called once per frame
    void Update()
    {
        if(FindAnyObjectByType<PressKeyOpenDoorBathroom>().GetIsBathRoomOpen() && count <= 0)
        {
            count++;
            FindAnyObjectByType<DialogueController>().PlayAudio(clip);
            FindAnyObjectByType<DialogueController>().ShowMotherDeath();
            flag = true;
            _start = DateTime.Now;
        }
        if (flag)
        {
            if (!_ghost.activeInHierarchy)
            {
                if ((DateTime.Now - _start).TotalSeconds >= 60)
                {
                    _ghost.SetActive(true);
                    _ghost.GetComponent<GhostVisionField>().RenewFOVCoroutine();
                    GhostController.instance.SwitchState(GhostController.State.Idle);
                    flag = false;
                }
            }
        }
    }
}
