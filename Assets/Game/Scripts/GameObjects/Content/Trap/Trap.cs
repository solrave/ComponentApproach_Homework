using Game.Damage;
using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Trap
{
    public class Trap : MonoBehaviour
    {
        private CollisionComponent _collisionComponent;
        private HealthComponent _healthComponent;
        private DamageComponent _damageComponent;

        private void OnEnable()
        {
            _collisionComponent = GetComponent<CollisionComponent>();
            _healthComponent = GetComponent<HealthComponent>();
            _damageComponent = GetComponent<DamageComponent>();
            
            _collisionComponent.OnEntered += this.OnEntered;
            _healthComponent.OnDied += this.Die;
        }

        private void OnDestroy()
        {
            _collisionComponent.OnEntered -= this.OnEntered;
            _healthComponent.OnDied -= this.Die;
        }

        private void Die() => Destroy(this.gameObject);

        private void OnEntered(Collision2D obj)
        {
            if (obj.collider.TryGetComponent<Trap>(out var trap))
                return;

            if (_damageComponent.Damage(obj))
                Die();
        }
    }
}