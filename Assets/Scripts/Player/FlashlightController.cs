using CharacterControl;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlashlightController : MonoBehaviour
{
    public static FlashlightController instance;
    private bool _haveFlashLight = false;
    private bool _currentState = false;
    private Transform _whiteLight;
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
        if(_haveFlashLight && _whiteLight)
        {
            if (_inputManager.SwitchFlash)
            {
                Switch();
            }
        }
    }

    public void Switch()
    {
        if (!(_haveFlashLight && _whiteLight))
            return;
        _whiteLight.gameObject.SetActive(!_currentState);
        _currentState = !_currentState;
    }

    public void EnableFlashLight()
    {
        _haveFlashLight = true;
        GameObject flashlight = GameObject.Find("Flashlight");
        if (!flashlight) return;
        _whiteLight = flashlight.transform.GetChild(0);

        flashlight.layer = LayerMask.NameToLayer("Default");
        _whiteLight.gameObject.layer = LayerMask.NameToLayer("Default");
        Manager.TextUIManager.instance.ShowUI(_switchFlashModeText, 1.5f);
    }
}

