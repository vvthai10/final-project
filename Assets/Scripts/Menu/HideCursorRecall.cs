using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursorRecall : MonoBehaviour
{
    public GameObject FatherRitual;
    public GameObject Father;
    
    private DateTime _start;
    private bool end = false;
    [SerializeField] private GameObject _ghost;
    public void HideCursor()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Father.SetActive(false);
        FatherRitual.SetActive(false);
        _start = DateTime.Now;
        end = true;
    }
    private void Update()
    {
        if (end)
        {
            if((DateTime.Now - _start).TotalSeconds >= 50)
            {
                Debug.Log("Ghost enable");
                _ghost.SetActive(true);
                GhostController.instance.SwitchState(GhostController.State.Chasing);
                GhostController.instance._lastBite = true;
            }
        }
    }
}
