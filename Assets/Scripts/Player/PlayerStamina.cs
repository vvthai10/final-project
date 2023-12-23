using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerStamina : MonoBehaviour
    {
        public static PlayerStamina instace;
        const float _maxStamina = 100f;
        const float _runConsumeAmount = 25f;
        float _stamina;
        bool _canRegen;
        public bool AbleToRun { get; set; }

        [Header ("Stamina Settings: ")]
        [SerializeField] private float regenRate = 50f;
        [SerializeField] private float cooldown;
        [SerializeField] private float maxCooldown = 5.0f;

        [Space]
        [Header("Breathing SFXs: ")]
        [SerializeField] private AudioClip breathAudio;
        [SerializeField] private AudioClip outOfBreathAudio;
        [SerializeField] private AudioClip runningBreathAudio;
        private AudioSource _audioSource;
        private string _currentPlayingAudio = "Breath";
        private void Awake()
        {
            instace = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            _stamina = _maxStamina;
            _canRegen = true;
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            Debug.Log("Stamina: " + _stamina);
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
                _canRegen = true;
            if (_canRegen && _stamina < _maxStamina)
                _stamina += Time.deltaTime * regenRate;
            if (_stamina > 0)
            {
                AbleToRun = true;
            }
            Breathing();
        }

        private void FixedUpdate()
        {
            if (PlayerControl.PlayerController.instance.IsRunning)
            {
                _stamina -= _runConsumeAmount * Time.fixedDeltaTime;
                cooldown = maxCooldown;
                _canRegen = false;
                if (_stamina <= 0)
                {
                    _stamina = 0;
                    AbleToRun = false;
                    _audioSource.Stop();
                    _audioSource.PlayOneShot(outOfBreathAudio);
                    _currentPlayingAudio = outOfBreathAudio.name;
                }
                
            }
        }

        private void Breathing()
        {
            if (!_audioSource) return;
            if (
                    PlayerController.instance.IsRunning &&
                    _currentPlayingAudio == "Breath"
                )
            {
                _currentPlayingAudio = runningBreathAudio.name;
                _audioSource.PlayOneShot(runningBreathAudio);
            }
            else if (
                !PlayerController.instance.IsRunning && 
                _currentPlayingAudio == "Running Breath"
                )
            {
                _audioSource.Stop();
            }

            if (!_audioSource.isPlaying && breathAudio)
            {
                _currentPlayingAudio = breathAudio.name;
                _audioSource.PlayOneShot(breathAudio);
            }
        }
    }
}