using System;
using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Trap
{
    public class Trap : MonoBehaviour
    {
        private CollisionComponent _collisionComponent;
        [SerializeField] private float _damage;

        private void OnEnable()
        {
            _collisionComponent = GetComponent<CollisionComponent>();
            _collisionComponent.OnEntered += this.OnEntered;
        }

        private void OnDestroy()
        {
            _collisionComponent.OnEntered -= this.OnEntered;
        }

        private void OnEntered(Collision2D obj)
        {
            if (obj.collider.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(_damage);
                Destroy(this.gameObject);
            }
        }

        private void Update()
        {
            
        }
    }
}