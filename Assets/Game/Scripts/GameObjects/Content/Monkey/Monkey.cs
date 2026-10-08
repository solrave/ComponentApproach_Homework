using Game.Damage;
using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Monkey
{
    public class Monkey : MonoBehaviour,
                          ForceComponent.IPushCondition,
                          JumpComponent.ICondition
    {
        [SerializeField]
        private Transform _lookTarget;
        
        private LookComponent _lookComponent;
        private JumpComponent _jumpComponent;
        private ForceComponent _forceComponent;
        private GroundedComponent _groundedComponent;
        private HealthComponent _healthComponent;
        private DamageComponent _damageComponent;
        private TriggerComponent _triggerComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _lookComponent = GetComponent<LookComponent>();
            _jumpComponent = GetComponent<JumpComponent>();
            _forceComponent = GetComponent<ForceComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
            _forceComponent.SetPushCondition(this);
            _jumpComponent.SetCondition(this);
            _groundedComponent.SetCondition(() => _healthComponent.IsAlive);
            _damageComponent = GetComponent<DamageComponent>();
            _triggerComponent = GetComponentInChildren<TriggerComponent>();
        }

        private void OnEnable()
        {
            _triggerComponent.OnEntered += this.OnEntered;
        }

        private void OnDisable()
        {
            _triggerComponent.OnEntered -= this.OnEntered;
        }
        
        private void Update()
        {
            _jumpComponent.Jump();
            _forceComponent.PushSideways();
            _lookComponent.Look(_lookTarget);
        }
        
        private void OnEntered(Collider2D obj) => _damageComponent.Damage(obj);

        bool ForceComponent.IPushCondition.Evaluate() => _groundedComponent.IsGrounded;
        
        bool JumpComponent.ICondition.Evaluate() => _groundedComponent.IsGrounded;
    }
}