using CharacterControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Manager
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;
        [SerializeField] private PlayerInput playerInput;
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Run { get; private set; }
        public bool Interact { get; private set; }

        public bool SwitchFlash { get; private set; }
        public bool Idle2PushUp { get; private set; }
        public bool PushUp { get; private set; }

        private InputActionMap _currentMap;
        private InputAction _moveAction;
        private InputAction _runAction;
        private InputAction _lookAction;
        private InputAction _interactAction;
        private InputAction _flashAction;

        private InputAction _idle2PushUpAction;
        private InputAction _pushUpAction;

        private void Awake()
        {
            HideCursor();
            instance = this;
            _currentMap = playerInput.currentActionMap;
            _moveAction = _currentMap.FindAction("Move");
            _runAction = _currentMap.FindAction("Run");
            _lookAction = _currentMap.FindAction("Look");
            _interactAction = _currentMap.FindAction("Interact"); 
            _flashAction = _currentMap.FindAction("SwitchFlash");
            _idle2PushUpAction = _currentMap.FindAction("Idle2PushUp");
            _pushUpAction = _currentMap.FindAction("PushUp");

            _moveAction.performed += onMove;
            _moveAction.canceled += onMove;

            _lookAction.performed += onLook;
            _lookAction.canceled += onLook;

            _runAction.performed += onRun;
            _runAction.canceled += onRun;


            _interactAction.performed += _ => PhysicsInteract.instance.Interact();

            _flashAction.performed += _ => FlashlightController.instance.Switch();

            _idle2PushUpAction.performed += onIdleToPushUp;
            _idle2PushUpAction.canceled += onIdleToPushUp;
            _pushUpAction.performed += onPushUp;
            _pushUpAction.canceled += onPushUp;
        }
        private void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void onMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }
        private void onLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }
        private void onRun(InputAction.CallbackContext context)
        {
            Run = context.ReadValueAsButton();
        }

        private void onIdleToPushUp(InputAction.CallbackContext context)
        {
            Idle2PushUp = context.ReadValueAsButton();
        }

        private void onPushUp(InputAction.CallbackContext context)
        {
            PushUp = context.ReadValueAsButton();
        }



        private void OnEnable()
        {
            _currentMap.Enable();
        }
        private void OnDisable()
        {
            _currentMap.Disable();
        }


    }
}
