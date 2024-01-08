using PlayerControl;
using UnityEngine;


namespace CharacterControl
{
    [RequireComponent(typeof(PlayerController))]
    public class PhysicsInteract : MonoBehaviour
    {
        public static PhysicsInteract instance;
        [Header("Masks: ")]
        [SerializeField] private LayerMask _interactMask;
        [SerializeField] private LayerMask _defaultMask;
        [Space]
        [Header("Player Settings: ")]
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private Transform _interactTarget;
        [Space]
        [Header("Interact Settings: ")]
        [SerializeField] private float _interactDistance = 10f;
        [Space]
        [Header("Texts Displays: ")]
        [SerializeField] private GameObject _pickupText;
        [SerializeField] private GameObject _interactRadioText;

        private Rigidbody _rigidbody;
        private Transform _transform;
        private Ray _cameraRay;
        private RaycastHit _raycastHit;
        private const string _flashlightName = "Flashlight";
        private const string _radioName = "Radio";
        private void Awake()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            _cameraRay = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(_cameraRay, out RaycastHit hitInfo, _interactDistance, _interactMask))
            {
                
                if (_pickupText && hitInfo.rigidbody.name == _flashlightName)
                {
                    Manager.UIManager.instance.ShowUI(_pickupText, 0);
                }
                if (_interactRadioText && hitInfo.rigidbody.name == _radioName)
                {
                    Manager.UIManager.instance.ShowUI(_interactRadioText, 0);
                }
            }
            _raycastHit = hitInfo; 
        }

        public void Interact()
        {
            if(_raycastHit.collider != null && _raycastHit.rigidbody)
            {
                _rigidbody = _raycastHit.rigidbody;
                _transform = _raycastHit.transform;
                _rigidbody.useGravity = false;

                switch (_rigidbody.gameObject.name)
                {
                    case _flashlightName:
                    {
                        FlashlightController.instance.EnableFlashLight();
                        break;
                    }
                    case _radioName:
                    {
                        // Do something...
                        break;
                    }
                    default:
                        break;
                }
            } 
        }
        private void FixedUpdate()
        {            
            if (_rigidbody && _rigidbody.gameObject.name == _flashlightName)
            {
                Vector3 playerEuler = PlayerController.instance.GetEulerAngles();
                _transform.position = _interactTarget.position;
                _transform.transform.localEulerAngles = new Vector3(80f + FPSController.instance.XRotation(), playerEuler.y, playerEuler.z);
            }

        }
    }

}