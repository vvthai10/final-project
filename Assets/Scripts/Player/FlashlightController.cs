using CharacterControl;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlashlightController : MonoBehaviour
{
    public static FlashlightController instance;
    private bool _haveFlashLight = false;
    private InputManager _inputManager;
    [SerializeField] private GameObject _switchFlashModeText;

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
        if(_haveFlashLight)
        {
            if (_inputManager.SwitchFlash)
            {
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
            _haveFlashLight = true;
            Manager.UIManager.instance.ShowUI(_switchFlashModeText, 1.5f);
        }
    }
}

