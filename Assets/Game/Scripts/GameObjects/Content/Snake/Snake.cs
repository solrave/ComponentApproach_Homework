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

        [SerializeField]
        private TriggerComponent _chaseTrigger;
        
        [SerializeField] 
        private TriggerComponent _hitTrigger;
        
        private GameObject _target;
        private PatrolComponent _patrolComponent;
        private ChaseComponent _chaseComponent;
        private HealthComponent _healthComponent;
        private LookComponent _lookComponent;
        private MoveComponent _moveComponent;
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
            _groundedComponent = GetComponent<GroundedComponent>();
            _groundedComponent.SetCondition(() => _healthComponent.IsAlive);
            _moveComponent.SetAction(this);
            _moveComponent.SetCondition(this);
        }

        private void OnEnable()
        {
            _healthComponent.OnDied += this.OnDied;
            _chaseTrigger.OnEntered += this.SetChaseTarget;
            _chaseTrigger.OnExited += this.ReleaseChaseTarget;
            _hitTrigger.OnEntered += this.OnCollision;
        }

        private void OnDisable()
        {
            _healthComponent.OnDied -= this.OnDied;
            _chaseTrigger.OnEntered -= this.SetChaseTarget;
            _chaseTrigger.OnExited -= this.ReleaseChaseTarget;
            _hitTrigger.OnEntered -= this.OnCollision;
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

        private void OnCollision(Collider2D obj)
        {
            _forceComponent.PushKnockback(obj.attachedRigidbody);
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