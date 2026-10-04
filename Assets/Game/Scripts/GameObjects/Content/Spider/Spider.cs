using Game.Damage;
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
        private LookComponent _lookComponent;
        private DamageComponent _damageComponent;
        
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
            _lookComponent = GetComponent<LookComponent>();
            _damageComponent = GetComponent<DamageComponent>();

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
        }

        private void OnEnable()
        {
            _healthComponent.OnDied += this.OnDied;
            _collisionComponent.OnEntered += this.OnCollision;
        }

        private void OnDisable()
        {
            _healthComponent.OnDied -= this.OnDied;
            _collisionComponent.OnEntered -= this.OnCollision;
        }

        private void Update()
        {
            _patrolComponent.UpdateDirection();
            
            if (_patrolComponent.Direction != Vector2.zero)
            {
                _moveComponent.Move(_patrolComponent.Direction);
            }
        }

        private void OnDied() => Destroy(this.gameObject);

        private void OnCollision(Collision2D obj)
        {
            if (obj.collider.TryGetComponent<Rigidbody2D>(out var body))
                _knockbackPushComponent.Push(body);

            _damageComponent.Damage(obj);
        }

        void MoveComponent.IAction.Invoke(Vector2 direction)
        {
            _lookComponent.Look(-direction.x);
        }

        bool MoveComponent.ICondition.Evaluate() => _healthComponent.IsAlive &&
                                                    _groundedComponent.IsGrounded;

        void KnockbackPushComponent.IAction.Invoke(Rigidbody2D target) 
            => _rigidbodyPushComponent.PushKnockback(target);

        bool KnockbackPushComponent.ICondition.Evaluate()
            => _healthComponent.IsAlive;
    }
}