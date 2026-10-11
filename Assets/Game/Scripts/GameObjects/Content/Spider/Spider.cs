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
        private HealthComponent _healthComponent;
        private GroundedComponent _groundedComponent;
        
        private void Awake()
        {
            _patrolComponent = GetComponent<PatrolComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            _lookComponent = GetComponent<LookComponent>();
            _damageComponent = GetComponent<DamageComponent>();
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
            _triggerComponent.OnEntered += this.OnTrigger;
        }

        private void OnDisable()
        {
            _healthComponent.OnDied -= this.OnDied;
            _triggerComponent.OnEntered -= this.OnTrigger;
        }
        
        private void Update()
        {
            _patrolComponent.Patrol();
        }

        private void OnDied() => Destroy(this.gameObject);
        
        private void OnTrigger(Collider2D obj)
        {
            _damageComponent.Damage(obj);
            
            if (obj.TryGetComponent<Rigidbody2D>(out var body))
                _forceComponent.PushKnockback(body);
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