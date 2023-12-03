using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    [RequireComponent(typeof(PlayerController))]

    public class FPSController : MonoBehaviour
    {
        private InputManager _inputManager;
        [SerializeField] private Transform CameraRoot;
        [SerializeField] private Transform Camera;
        [SerializeField] private float UpperLimit = -40f;
        [SerializeField] private float BottomLimit = 70f;
        [SerializeField] private float MouseSensitivity = 21.9f;

        private float _xRotation;
        private Rigidbody _playerRigidBody;

        // Start is called before the first frame update
        void Start()
        {
            _playerRigidBody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void CameraMovement()
        {
            var Mouse_X = _inputManager.Look.x;
            var Mouse_Y = _inputManager.Look.y;
            Camera.position = CameraRoot.position;

            _xRotation -= Mouse_Y * MouseSensitivity * Time.smoothDeltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

            Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            _playerRigidBody.MoveRotation(
                _playerRigidBody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0)
                );
        }

        private void LateUpdate()
        {
            CameraMovement();
        }
    }

}