using PlayerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerControl
{
    [RequireComponent(typeof(HealthController))]
    public class DamageController : MonoBehaviour
    {
        [SerializeField] float damage = 35f;
        [SerializeField] HealthController _healthController;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                _healthController.TakeDamage(damage);
                other.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
    }

}