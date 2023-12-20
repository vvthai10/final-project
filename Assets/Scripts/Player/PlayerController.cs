using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;
        private Rigidbody _playerRigidBody;
        private InputManager _inputManager;
        private Animator _animator;

        private bool _hasAnimator;
        private int _xVelHash;
        private int _yVelHash;

        private const float _walkSpeed = 2f;
        private const float _runSpeed = 6f;
        private Vector2 _currentVelocity;
        private List<Vector3> _climbDirections;

        [Header("Player Animation Blend Speed: ")]
        [SerializeField] private float AnimBlendSpeed = 8.9f;

        [Header("Player Step Climb: ")]
        [SerializeField] GameObject stepRayLower;
        [SerializeField] GameObject stepRayUpper;
        [SerializeField][Range(0.02f, 0.2f)] float stepSmooth = 0.1f;

        private void Awake()
        {
            instance = this;
            _climbDirections = new List<Vector3>()                
            { 
                transform.TransformDirection(Vector3.forward),
                transform.TransformDirection(1.5f, 0, 1),
                transform.TransformDirection(-1.5f, 0, 1)
            };
        }
        // Start is called before the first frame update
        void Start()
        {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _playerRigidBody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();
            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
        }
        // Update is called once per frame
        void Update()
        {

        }
        private void FixedUpdate()
        {
            Move();
            StepClimb();
        }

        private void Move()
        {
            if(!_hasAnimator) return;
            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            if (_inputManager.Move == Vector2.zero)
            {
                targetSpeed = 0f;
            }
            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
            float xVecDiff = _currentVelocity.x - _playerRigidBody.velocity.x;
            float zVecDiff = _currentVelocity.y - _playerRigidBody.velocity.z;
            _playerRigidBody.AddForce(
                transform.TransformVector(new Vector3(xVecDiff, 0, zVecDiff)),
                ForceMode.VelocityChange
                );

            // set animator
            _animator.SetFloat(_xVelHash, _currentVelocity.x); 
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
        }

        private void StepClimb()
        {
            RaycastHit hitLower, hitUpper;
            foreach (var direction in _climbDirections)
            {
                if (Physics.Raycast(
                        stepRayLower.transform.position,
                        direction,
                        out hitLower,
                        0.1f)
                )
                {
                    if (!Physics.Raycast(
                            stepRayUpper.transform.position,
                            direction,
                            out hitUpper,
                            0.2f)
                        )
                    {
                        _playerRigidBody.position -= new Vector3(0f, -stepSmooth, 0f);
                    }
                }
            }
        }
        public Vector3 GetEulerAngles()
        {
            return transform.localRotation.eulerAngles;
        }
    }

}