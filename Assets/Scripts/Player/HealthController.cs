using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerControl
{
    [RequireComponent(typeof(PlayerController))]
    public class HealthController : MonoBehaviour
    {
        private float _currentHealth = 100;
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private int regenRate = 15;
        private bool canRegen = false;

        [SerializeField] private Image redSplatterImage = null;
        [SerializeField] private Image radialBloodImage = null;
        [SerializeField] private float hurtTimer = 0.1f;
        [SerializeField] private float healCooldown = 3.0f;
        [SerializeField] private float maxHealCooldown = 3.0f;
        private bool _startCooldown = false;

        [SerializeField] private AudioClip hurtAudio = null;
        private AudioSource _healthAudioSource;
        private Animator _animator;
        private int _deathHash;

        [SerializeField] GameObject deathText;
        // Start is called before the first frame update
        void Start()
        {
            _healthAudioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
            _deathHash = Animator.StringToHash("Death");
        }

        private void Update()
        {
            if (_startCooldown)
            {
                healCooldown -= Time.deltaTime;
                if (healCooldown <= 0)
                {
                    canRegen = true;
                    _startCooldown = false;
                }
            }
            if (canRegen)
            {
                if(_currentHealth <= maxHealth - 0.01)
                {
                    _currentHealth += Time.deltaTime * regenRate;
                    UpdateHealth();
                }
                else
                {
                    _currentHealth = maxHealth;
                    healCooldown = maxHealCooldown;
                    canRegen = false;
                }
            }
        }
        void UpdateHealth()
        {
            redSplatterImage.color = new Color(
                                                redSplatterImage.color.r, 
                                                redSplatterImage.color.g, 
                                                redSplatterImage.color.b, 
                                                1 - (_currentHealth / maxHealth)
                                            );
            radialBloodImage.color = new Color(
                                                radialBloodImage.color.r,
                                                radialBloodImage.color.g,
                                                radialBloodImage.color.b,
                                                1 - (_currentHealth / maxHealth)
                                            );
        }
        IEnumerator HurtSplash()
        {
            if (!_healthAudioSource && hurtAudio)
                _healthAudioSource.PlayOneShot(hurtAudio);
            yield return new WaitForSeconds(hurtTimer);
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            canRegen = false;
            healCooldown = maxHealCooldown;
            _startCooldown = true;
            UpdateHealth();
            if (_currentHealth > 0)
            {
                StartCoroutine(HurtSplash());
            }
            else
            {
                _animator.SetTrigger(_deathHash);
                Manager.UIManager.instance.ShowUI(deathText, 5f);
            }
        }
    }

}