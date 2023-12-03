using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerControl
{
    [RequireComponent(typeof(PlayerController))]
    public class _EasterEgg : MonoBehaviour
    {
        private int _idle2PushUpHash;
        private int _pushUpHash;

        private Animator _animator;
        private bool _hasAnimator;
        private InputManager _inputManager;
        // Start is called before the first frame update
        void Start()
        {
            _inputManager = GetComponent<InputManager>();
            _hasAnimator = TryGetComponent<Animator>(out _animator);

            _idle2PushUpHash = Animator.StringToHash("Idle2PushUp");
            _pushUpHash = Animator.StringToHash("PushUp");
        }

        // Update is called once per frame
        void Update()
        {
            if (!_hasAnimator) return;
            Idle2PushUp();
            PushUp();
        }

        void Idle2PushUp()
        {
            if (_inputManager.Idle2PushUp)
            {
                _animator.SetTrigger(_idle2PushUpHash);
            }
            else
            {
                _animator.ResetTrigger(_idle2PushUpHash);
            }
        }

        void PushUp()
        {
            if (_inputManager.PushUp)
            {
                _animator.SetBool(_pushUpHash, true);
            }
            else
            {
                _animator.SetBool(_pushUpHash, false);
            }
        }
    }

}