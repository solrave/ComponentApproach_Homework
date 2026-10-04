using Game.Damage;
using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Monkey
{
    public class Monkey : MonoBehaviour,
                          PushComponent.IAction,
                          PushComponent.ICondition,
                          JumpComponent.IAction,
                          JumpComponent.ICondition
    {
        [SerializeField]
        private Transform _lookTarget;
        
        private LookComponent _lookComponent;
        private JumpComponent _jumpComponent;
        private RigidbodyJumpComponent _rigidbodyJumpComponent;
        private PushComponent _pushComponent;
        private RigidbodyPushComponent _rigidbodyPushComponent;
        private GroundedComponent _groundedComponent;
        private HealthComponent _healthComponent;
        private DamageComponent _damageComponent;
        private TriggerComponent _triggerComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _lookComponent = GetComponent<LookComponent>();
            _jumpComponent = GetComponent<JumpComponent>();
            _rigidbodyJumpComponent = GetComponent<RigidbodyJumpComponent>();
            _pushComponent = GetComponent<PushComponent>();
            _rigidbodyPushComponent = GetComponent<RigidbodyPushComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
            _pushComponent.SetAction(this);
            _pushComponent.SetCondition(this);
            _jumpComponent.SetAction(this);
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
            _pushComponent.Push();
            _lookComponent.Look(_lookTarget);
        }
        
        private void OnEntered(Collider2D obj) => _damageComponent.Damage(obj);

        void PushComponent.IAction.Invoke() => _rigidbodyPushComponent.PushSideways();

        bool PushComponent.ICondition.Evaluate() => _groundedComponent.IsGrounded;

        void JumpComponent.IAction.Invoke() => _rigidbodyJumpComponent.JumpUp();

        bool JumpComponent.ICondition.Evaluate() => _groundedComponent.IsGrounded;
    }
}