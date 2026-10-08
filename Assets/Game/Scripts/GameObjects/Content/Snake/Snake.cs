using Game.Damage;
using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Snake
{
    public class Snake : MonoBehaviour,
                        MoveComponent.IAction,
                        MoveComponent.ICondition
    {
        [SerializeField]
        private float _damage;

        [SerializeField]
        private ForceComponent _forceComponent;
        
        private GameObject _target;
        private PatrolComponent _patrolComponent;
        private ChaseComponent _chaseComponent;
        private HealthComponent _healthComponent;
        private LookComponent _lookComponent;
        private MoveComponent _moveComponent;
        private CollisionComponent _collisionComponent;
        private TriggerComponent _triggerComponent;
        private GroundedComponent _groundedComponent;
        private DamageComponent _damageComponent;
        

        private void Awake()
        {
            _damageComponent = GetComponent<DamageComponent>();
            _patrolComponent = GetComponent<PatrolComponent>();
            _chaseComponent = GetComponent<ChaseComponent>();
            _healthComponent = GetComponent<HealthComponent>();
            _lookComponent = GetComponent<LookComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            _collisionComponent = GetComponentInChildren<CollisionComponent>();
            _triggerComponent = GetComponentInChildren<TriggerComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
            _groundedComponent.SetCondition(() => _healthComponent.IsAlive);
            _moveComponent.SetAction(this);
            _moveComponent.SetCondition(this);
        }

        private void OnEnable()
        {
            _healthComponent.OnDied += this.OnDied;
            _collisionComponent.OnEntered += this.OnCollision;
            _triggerComponent.OnEntered += this.SetChaseTarget;
            _triggerComponent.OnExited += this.ReleaseChaseTarget;
        }

        private void OnDisable()
        {
            _healthComponent.OnDied -= this.OnDied;
            _collisionComponent.OnEntered -= this.OnCollision;
            _triggerComponent.OnEntered -= this.SetChaseTarget;
            _triggerComponent.OnExited -= this.ReleaseChaseTarget;
        }
        
        private void Update()
        {
            if (_chaseComponent.HasTarget)
            {
                Debug.Log($"CHASING");
                _chaseComponent.UpdateDirection();
                if (_chaseComponent.Direction != Vector2.zero)
                    _moveComponent.Move(_chaseComponent.Direction);
            }
            else
            {
                _patrolComponent.UpdateDirection();
                if (_patrolComponent.Direction != Vector2.zero)
                    _moveComponent.Move(_patrolComponent.Direction);
            }
        }

        private void OnDied() 
            => Destroy(this.gameObject);

        private void OnCollision(Collision2D obj)
        {
            _forceComponent.PushKnockback(obj.rigidbody);
            _damageComponent.Damage(obj);
        }
        
        private void SetChaseTarget(Collider2D obj)
        {
            if (obj.TryGetComponent<Character>(out var target))
                _chaseComponent.SetTarget(target.gameObject);
        }
        
        private void ReleaseChaseTarget(Collider2D obj)
        {
            if (obj.TryGetComponent<Character>(out var target))
                _chaseComponent.SetTarget(null);
        }
        
        void MoveComponent.IAction.Invoke(Vector2 direction)
            { _lookComponent.Look(direction.x); }
        
        bool MoveComponent.ICondition.Evaluate() 
            => _healthComponent.IsAlive;
    }
}