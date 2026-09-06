using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(DamageAnimator))]
    public class HealthView : MonoBehaviour
    {
        private int DEATH = Animator.StringToHash("Death");
        private Health _health;
        private DamageAnimator _damageAnimator;
        private Animator _animator;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _damageSound;

        private void OnEnable()
        {
            _animator = GetComponentInChildren<Animator>();
            _health = GetComponentInParent<Health>();
            _damageAnimator = GetComponent<DamageAnimator>();
            _health.OnDamage += this.OnDamage;
        }

        private void OnDisable()
        {
            _health.OnDamage -= this.OnDamage;
        }

        private void OnDamage()
        {
            _damageAnimator.TakeDamage();
            _audioSource.PlayOneShot(_damageSound);
            
            if (_health.IsDead)
                _animator.SetTrigger(DEATH);
        }
    }
}