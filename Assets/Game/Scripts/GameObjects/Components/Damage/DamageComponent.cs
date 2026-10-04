using UnityEngine;

namespace Game.Damage
{
    public class DamageComponent : MonoBehaviour
    {
        [SerializeField] 
        private float _damage;

        public bool Damage(Collision2D obj)
        {
            if (!obj.collider.TryGetComponent<HealthComponent>(out var health)
                || !health.IsAlive) return false;
            
            health.TakeDamage(_damage);
            return true;
        }
        
        public bool Damage(Collider2D obj)
        {
            if (!obj.TryGetComponent<HealthComponent>(out var health)
                || !health.IsAlive) return false;
            
            health.TakeDamage(_damage);
            return true;
        }
    }
}