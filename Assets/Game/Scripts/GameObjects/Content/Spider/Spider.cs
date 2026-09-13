using System;
using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Spider
{
    public class Spider : MonoBehaviour,
                          MoveComponent.IAction,
                          MoveComponent.ICondition,
                          KnockbackPushComponent.IAction,
                          KnockbackPushComponent.ICondition
    {
        [SerializeField] 
        private float _damage;
        
        private PatrolComponent _patrolComponent;
        private MoveComponent _moveComponent;
        private TransformMoveComponent _transformMoveComponent;
        private LookComponent _lookComponent;
        
        private CollisionComponent _collisionComponent;
        private TriggerComponent _triggerComponent;
        
        private HealthComponent _healthComponent;
        private GroundedComponent _groundedComponent;
        
        private KnockbackPushComponent _knockbackPushComponent;
        private RigidbodyPushComponent _rigidbodyPushComponent;

        private void Awake()
        {
            _patrolComponent = GetComponent<PatrolComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            _transformMoveComponent = GetComponent<TransformMoveComponent>();
            _lookComponent = GetComponent<LookComponent>();

            _collisionComponent = GetComponent<CollisionComponent>();
            _triggerComponent = GetComponent<TriggerComponent>();
            
            _healthComponent = GetComponent<HealthComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
            
            _knockbackPushComponent = GetComponent<KnockbackPushComponent>();
            _rigidbodyPushComponent = GetComponent<RigidbodyPushComponent>();
        
            _groundedComponent.SetCondition(() => _healthComponent.IsAlive);
        
            _moveComponent.SetAction(this);
            _moveComponent.SetCondition(this);
            
            _knockbackPushComponent.SetAction(this);
            _knockbackPushComponent.SetCondition(this);
            //
            // _pushRequest.SetAction(this);
            // _pushRequest.SetCondition(this);
        }

        private void OnEnable()
        {
            _collisionComponent.OnEntered += this.OnEntered;
        }

        private void OnDisable()
        {
            _collisionComponent.OnEntered -= this.OnEntered;
        }

        private void OnEntered(Collision2D obj)
        {
            if (obj.collider.TryGetComponent<Rigidbody2D>(out var body))
                _knockbackPushComponent.Push(body);
            
            if (!obj.collider.TryGetComponent<HealthComponent>(out var health)
                || !health.IsAlive) return;
            
            health.TakeDamage(_damage);
            
        }

        void MoveComponent.IAction.Invoke(Vector2 direction)
        {
            _transformMoveComponent.Move(direction);
            _lookComponent.Look(-direction.x);
        }

        bool MoveComponent.ICondition.Evaluate() => _healthComponent.IsAlive &&
                                                    _groundedComponent.IsGrounded;

        void KnockbackPushComponent.IAction.Invoke(Rigidbody2D target) 
            => _rigidbodyPushComponent.Push(target);

        bool KnockbackPushComponent.ICondition.Evaluate()
            => _healthComponent.IsAlive;
    }
}