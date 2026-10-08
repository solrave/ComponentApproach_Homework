using System;
using UnityEngine;

namespace Game
{
    public class JumpComponent : MonoBehaviour
    {
        public event Action OnJump;
        public interface IAction
        {
            void Invoke();
        }
        
        public interface ICondition
        {
            bool Evaluate();
        }
        
        [SerializeField]
        private Cooldown _cooldown;
        
        [SerializeField] 
        private float _force;
        
        private bool _jumpRequested;
        private ICondition _condition;
        private IAction _action;
        private Rigidbody2D _body;

        public void SetCondition(ICondition condition) => _condition = condition;
        public void SetAction(IAction action) => _action = action;
        
        private void OnEnable() 
            => _body = GetComponent<Rigidbody2D>();
        
        public void Jump()
        {
            if (!_cooldown.Ready) return;
            if (!(_condition == null || _condition.Evaluate())) return;
            
            _cooldown.Trigger();
            _jumpRequested = true;
        }

        private void FixedUpdate()
        {
            _cooldown.Tick(Time.fixedDeltaTime);
            
            if (!_jumpRequested) return;

            JumpUp();
            _action?.Invoke();
            _jumpRequested = false;
            OnJump?.Invoke();
        }

        private void JumpUp() =>
            _body.AddForce(Vector2.up * _force, ForceMode2D.Impulse);
    }
}