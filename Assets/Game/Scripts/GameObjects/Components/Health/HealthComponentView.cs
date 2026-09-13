using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(DamageEffectAnimator))]
    public class HealthComponentView : MonoBehaviour
    {
        private int DEATH = Animator.StringToHash("Death");
        private HealthComponent _healthComponent;
        private DamageEffectAnimator _damageEffectAnimator;
        private Animator _animator;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _damageSound;

        private void OnEnable()
        {
            _animator = GetComponentInChildren<Animator>();
            _healthComponent = GetComponentInParent<HealthComponent>();
            _damageEffectAnimator = GetComponent<DamageEffectAnimator>();
            _healthComponent.OnDamage += this.OnDamage;
        }

        private void OnDisable()
        {
            _healthComponent.OnDamage -= this.OnDamage;
        }

        private void OnDamage()
        {
            _damageEffectAnimator.TakeDamage();
            _audioSource.PlayOneShot(_damageSound);
            
            if (_healthComponent.IsDead)
                _animator.SetTrigger(DEATH);
        }
    }
}