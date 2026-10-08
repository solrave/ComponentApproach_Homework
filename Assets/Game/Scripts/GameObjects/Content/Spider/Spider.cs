using Game.Damage;
using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Spider
{
    public class Spider : MonoBehaviour,
                          MoveComponent.IAction,
                          MoveComponent.ICondition,
                          ForceComponent.IPushCondition
    {
        [SerializeField] 
        private float _damage;

        [SerializeField] 
        private ForceComponent _forceComponent;

        private TriggerComponent _triggerComponent;
        private PatrolComponent _patrolComponent;
        private MoveComponent _moveComponent;
        private LookComponent _lookComponent;
        private DamageComponent _damageComponent;
        private CollisionComponent _collisionComponent;
        private HealthComponent _healthComponent;
        private GroundedComponent _groundedComponent;
        
        private void Awake()
        {
            _patrolComponent = GetComponent<PatrolComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            _lookComponent = GetComponent<LookComponent>();
            _damageComponent = GetComponent<DamageComponent>();
            _collisionComponent = GetComponentInChildren<CollisionComponent>();
            _triggerComponent = GetComponentInChildren<TriggerComponent>();
            _healthComponent = GetComponent<HealthComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
            _groundedComponent.SetCondition(() => _healthComponent.IsAlive);
            _moveComponent.SetAction(this);
            _moveComponent.SetCondition(this);
            _forceComponent.SetPushCondition(this);
        }

        private void OnEnable()
        {
            _healthComponent.OnDied += this.OnDied;
            _collisionComponent.OnEntered += this.OnCollision;
            _triggerComponent.OnEntered += this.OnTrigger;
        }

        private void OnDisable()
        {
            _healthComponent.OnDied -= this.OnDied;
            _collisionComponent.OnEntered -= this.OnCollision;
            _triggerComponent.OnEntered -= this.OnTrigger;
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
            if (obj.gameObject.layer != LayerMask.NameToLayer("Character"))
                return;
            
            if (obj.collider.TryGetComponent<Rigidbody2D>(out var body))
                _forceComponent.PushKnockback(body);

            _damageComponent.Damage(obj);
        }
        
        private void OnTrigger(Collider2D obj)
        {
            _damageComponent.Damage(obj);
            _forceComponent.PushKnockback(obj.attachedRigidbody);
        }

        void MoveComponent.IAction.Invoke(Vector2 direction)
            { _lookComponent.Look(-direction.x); }

        bool MoveComponent.ICondition.Evaluate() 
            => _healthComponent.IsAlive &&
               _groundedComponent.IsGrounded;

        bool ForceComponent.IPushCondition.Evaluate()
            => _healthComponent.IsAlive &&
               _groundedComponent.IsGrounded;
    }
}