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
        [SerializeField] private float _interactDistance = 3f;
        [Space]
        [Header("Texts Displays: ")]
        [SerializeField] private GameObject _pickupText;
        [SerializeField] private GameObject _interactRadioText;
        [SerializeField] private GameObject _newsText;
        [SerializeField] private GameObject _bookText;

        [Space]
        [Header("Canvas Displays: ")]
        [SerializeField] private GameObject newsCanvas;

        private Rigidbody _rigidbody;
        private Transform _transform;
        private Ray _cameraRay;
        private RaycastHit _raycastHit;
        private const string _flashlightName = "Flashlight";
        private const string _radioName = "Radio";
        private const string _newsName = "News";
        private const string _bookName = "Book";

        private bool pickFlashlight = false;
        private Rigidbody _rigidbodyFlashlight;
        private Transform _transformFlashlight;
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
                if (!hitInfo.rigidbody) return;
                switch (hitInfo.rigidbody.name)
                {
                    case _flashlightName:
                        {
                            if (!_pickupText) return; 
                            Manager.UIManager.instance.ShowUI(_pickupText, 0);
                            break;
                        }
                    case _radioName:
                        {
                            if (!_interactRadioText) return;
                            Manager.UIManager.instance.ShowUI(_interactRadioText, 0);
                            break;
                        }
                    case _newsName:
                        {
                            if (!_newsText) return;
                            Manager.UIManager.instance.ShowUI(_newsText, 0);
                            break;
                        }
                    case _bookName:
                        {
                            if (!_bookText) return;
                            Manager.UIManager.instance.ShowUI(_bookText, 0);
                            break;
                        }
                    default:
                        break;
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
                        pickFlashlight = true;
                        _rigidbodyFlashlight = _rigidbody;
                        _transformFlashlight = _transform;
                        break;
                    }
                    case _radioName:
                    {
                        // Do something...
                        FindObjectOfType<DialogueController>().StartConversation();
                        break;
                    }
                    case _newsName:
                    {
                        ItemCanvasManager.instance.Show();
                        break;
                    }
                    case _bookName:
                    {
                        BookCanvasManager.instance.Show();
                        break;
                    }
                    default:
                        break;
                }
            } 
        }
        private void FixedUpdate()
        {            
            if (pickFlashlight && _transformFlashlight)
            {
                Vector3 playerEuler = PlayerController.instance.GetEulerAngles();
                _transformFlashlight.position = _interactTarget.position;
                _transformFlashlight.transform.localEulerAngles = new Vector3(80f + FPSController.instance.XRotation(), playerEuler.y, playerEuler.z);
            }

        }
    }

}