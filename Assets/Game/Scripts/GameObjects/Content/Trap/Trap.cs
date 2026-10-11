using Game.Damage;
using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Trap
{
    public class Trap : MonoBehaviour
    {
        private TriggerComponent _triggerComponent;
        private HealthComponent _healthComponent;
        private DamageComponent _damageComponent;

        private void OnEnable()
        {
            _triggerComponent = GetComponentInChildren<TriggerComponent>();
            _healthComponent = GetComponent<HealthComponent>();
            _damageComponent = GetComponent<DamageComponent>();
            
            _triggerComponent.OnEntered += this.OnTriggered;
            _healthComponent.OnDied += this.Die;
        }

        private void OnDestroy()
        {
            _triggerComponent.OnEntered -= this.OnTriggered;
            _healthComponent.OnDied -= this.Die;
        }

        private void Die() => Destroy(this.gameObject);

        private void OnTriggered(Collider2D obj)
        {
            if (obj.TryGetComponent<Trap>(out var trap))
                return;

            if (_damageComponent.Damage(obj))
                Die();
        }
    }
}