using CharacterControl;
using Manager;
using PlayerControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class FlashlightController : MonoBehaviour
{
    public static FlashlightController instance;
    private bool _haveFlashLight = false;
    private InputManager _inputManager;
    [SerializeField] private GameObject _switchFlashModeText;

    [Header("VR Settings: ")]
    [SerializeField]
    private InputActionProperty pickUpAction;
    [SerializeField]
    private InputActionProperty switchAction;

    [SerializeField]
    private GameObject _pickUpPoint;
    [SerializeField]
    private GameObject _flashlight;
    private DateTime _lastTimeSwitch = DateTime.Now;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pickUpAction.action.ReadValue<float>() != 0)
        {
            EnableFlashLight();
        }
        if(_haveFlashLight)
        {
            Vector3 playerEuler = PlayerController.instance.GetEulerAngles();
            _flashlight.transform.position = _pickUpPoint.transform.position;
            _flashlight.transform.localEulerAngles = new Vector3(80f + FPSController.instance.XRotation(), playerEuler.y, playerEuler.z);

            if ((DateTime.Now - _lastTimeSwitch).TotalSeconds >= 1 && (_inputManager.SwitchFlash || switchAction.action.ReadValue<float>() != 0))
            {
                _lastTimeSwitch = DateTime.Now;
                Switch();
            }
        }
    }

    public void Switch()
    {
        FlashlightManager.instance.Switch();
    }

    public void EnableFlashLight()
    {
        if(FlashlightManager.instance.LoadFlashlight())
        {
            Debug.Log("Load Flashlight");
            _haveFlashLight = true;
            Manager.UIManager.instance.ShowUI(_switchFlashModeText, 1.5f);

            Vector3 playerEuler = PlayerController.instance.GetEulerAngles();
            _flashlight.transform.position = _pickUpPoint.transform.position;
            _flashlight.transform.transform.localEulerAngles = new Vector3(80f + FPSController.instance.XRotation(), playerEuler.y, playerEuler.z);
        }
    }
}

