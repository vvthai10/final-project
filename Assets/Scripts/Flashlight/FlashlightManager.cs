using PlayerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlashlightManager : MonoBehaviour
{
    public static FlashlightManager instance;
    private bool _currentState = false; 
    private Transform _whiteLight;
    private void Awake()
    {
        instance = this;
    }

    public void Switch()
    {
        if (!_whiteLight)
            return;
        _currentState = !_currentState;
        if (_currentState)
            Manager.AudioManager.instance.PlayOneShot("SwitchFlashlightOn");
        else
            Manager.AudioManager.instance.PlayOneShot("SwitchFlashlightOff");
        _whiteLight.gameObject.SetActive(_currentState);
    }

    public bool LoadFlashlight()
    {
        GameObject flashlight = GameObject.Find("Flashlight");
        if (!flashlight) return false;
        _whiteLight = flashlight.transform.GetChild(0);
        flashlight.layer = LayerMask.NameToLayer("Default");
        _whiteLight.gameObject.layer = LayerMask.NameToLayer("Default");
       return true;
    }
}
