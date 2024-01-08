using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    [RequireComponent(typeof(PlayerController))]

    public class FPSController : MonoBehaviour
    {

        public static FPSController instance;
        private InputManager _inputManager;
        [Header("VR Settings: ")]
        [SerializeField]
        private InputActionProperty headRotation;
        private bool _vrDetector = false;

        [Space]
        [Header("PC Settings: ")]
        [SerializeField] private Transform CameraRoot;
        [SerializeField] private Transform Camera;
        [SerializeField] private float UpperLimit = -40f;
        [SerializeField] private float BottomLimit = 50f;
        [SerializeField] private float MouseSensitivity = 18.9f;

        [Header("Fixed camera rotation towards a point")]
        public float rotationSpeed = 1f;
        private bool lockedRotating = false;
        private Transform lockedRotatingTarget;
        private Vector3 playerToTargetDirection;
        private Vector3 camToTargetDirection;
        private Quaternion playerToTargetLookRotation;
        private Quaternion camToTargetLookRotation;

        private float _xRotation;
        private Rigidbody _playerRigidBody;
        private bool freeze = false;

        private void Awake()
        {
            instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            _playerRigidBody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();
        }

        // Update is called once per frame
        void Update()
        {
            DetectVR();

            if (lockedRotating)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, playerToTargetLookRotation, Time.deltaTime * rotationSpeed);
                Camera.rotation = Quaternion.Slerp(Camera.rotation, camToTargetLookRotation, Time.deltaTime * rotationSpeed);
            }
        }

        private void CameraMovement()
        {
            float Mouse_X;
            float Mouse_Y;
            if (_vrDetector)
            {
                //  Debug.Log("Angle: " + headRotation.action.ReadValue<Quaternion>().eulerAngles);
                var mouse = headRotation.action.ReadValue<Vector2>();
                Mouse_X = mouse.y;
                Mouse_Y = mouse.x;
                Camera.position = CameraRoot.position;

                _xRotation -= Mouse_Y * MouseSensitivity * Time.smoothDeltaTime;
                _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

                Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
                _playerRigidBody.MoveRotation(
                    _playerRigidBody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0)
                    );
            }
            else
            {
                //Debug.Log("Mouse");
                //Mouse_X = _inputManager.Look.x;
                //Mouse_Y = _inputManager.Look.y;
                //Camera.position = CameraRoot.position;

                //_xRotation -= Mouse_Y * MouseSensitivity * Time.smoothDeltaTime;
                //_xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

                //Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
                //_playerRigidBody.MoveRotation(
                //    _playerRigidBody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0)
                //    );
            }
            //Debug.Log("Mouse");
            //Mouse_X = _inputManager.Look.x;
            //Mouse_Y = _inputManager.Look.y;
            //Camera.position = CameraRoot.position;

            //_xRotation -= Mouse_Y * MouseSensitivity * Time.smoothDeltaTime;
            //_xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

            //Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            //_playerRigidBody.MoveRotation(
            //    _playerRigidBody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0)
            //    );

        }

        private void LateUpdate()
        {
            if (!freeze)
            {
                CameraMovement();
            }
        }

        public float XRotation()
        {
            return _xRotation;
        }

        public void Freeze()
        {
            this.freeze = true;
        }

        public void Unfreeze()
        {
            this.freeze = false;
        }

        public void StartRotatingTowards(Transform target)
        {
            this.lockedRotating = true;
            this.lockedRotatingTarget = target;
            this.playerToTargetDirection = (target.position - transform.position).normalized;
            playerToTargetDirection.y = 0;
            this.playerToTargetLookRotation = Quaternion.LookRotation(playerToTargetDirection);
            this.camToTargetDirection = (target.position - Camera.position).normalized;
            this.camToTargetLookRotation = Quaternion.LookRotation(camToTargetDirection);
        }

        private void DetectVR()
        {
            if (headRotation != null)
            {
                _vrDetector = true;
            }
            else
            {
                _vrDetector = false;
            }
        }
    }

}