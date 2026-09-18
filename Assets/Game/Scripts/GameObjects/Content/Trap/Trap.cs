using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Trap
{
    public class Trap : MonoBehaviour
    {
        private CollisionComponent _collisionComponent;
        
        [SerializeField] 
        private float _damage;

        private HealthComponent _healthComponent;

        private void OnEnable()
        {
            _collisionComponent = GetComponent<CollisionComponent>();
            _healthComponent = GetComponent<HealthComponent>();
            
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
            if (!obj.collider.TryGetComponent<HealthComponent>(out var health)
                || !health.IsAlive) return;
            
            if (obj.collider.TryGetComponent<Trap>(out var trap))
                return;
            
            health.TakeDamage(_damage);
            Die();
        }
    }
}