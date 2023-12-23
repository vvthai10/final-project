using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerFootstep : MonoBehaviour
    {
        private AudioSource _audioSource;
        // Start is called before the first frame update
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            if (_audioSource && !_audioSource.isPlaying)
            {
                if (
                    PlayerControl.PlayerController.instance.IsRunning &&
                    PlayerStamina.instace.AbleToRun
                    )
                {
                    _audioSource.pitch = 0.4f;
                    _audioSource.PlayOneShot(_audioSource.clip);
                }
                else if (PlayerControl.PlayerController.instance.IsWalking)
                {
                    _audioSource.pitch = 0.2f;
                    _audioSource.PlayOneShot(_audioSource.clip);
                }
            }

        }
    }

}