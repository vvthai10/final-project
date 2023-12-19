using Manager;
using PlayerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


namespace CharacterControl
{
    [RequireComponent(typeof(PlayerController))]
    public class PhysicsPickUp : MonoBehaviour
    {
        public static PhysicsPickUp instance;
        [SerializeField] private LayerMask _pickUpMask;
        [SerializeField] private LayerMask _defaultMask;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private Transform _pickUpTarget;
        [Space]
        [SerializeField] private float _pickUpDistance;

        private Rigidbody _rigidbody;
        private Transform _transform;
        private InputManager _inputManager;
        RaycastHit _raycastHit;
        [SerializeField] private GameObject _pickupText;
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
            Ray cameraRay = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, _pickUpDistance, _pickUpMask))
            {
                
                if (_pickupText && hitInfo.rigidbody.name == "Flashlight")
                {
                    Manager.UIManager.instance.ShowUI(_pickupText, 0);
                }
            }
            _raycastHit = hitInfo; 
        }
        public void PickUp()
        {
            if(_raycastHit.collider != null)
            {
                _rigidbody = _raycastHit.rigidbody;
                _transform = _raycastHit.transform;
                _rigidbody.useGravity = false;
                if (_rigidbody && _rigidbody.gameObject.name == "Flashlight")
                {
                    FlashlightController.instance.EnableFlashLight();
                }
            } 
        }
        private void FixedUpdate()
        {            
            if (_rigidbody)
            {
                Vector3 playerEuler = PlayerController.instance.GetEulerAngles();
                _transform.position = _pickUpTarget.position;
                _transform.transform.localEulerAngles = new Vector3(80f + FPSController.instance.XRotation(), playerEuler.y, playerEuler.z);
            }

        }
    }

}