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
        private CooldownComponent _cooldown;
        
        private bool _jumpRequested;
        private ICondition _condition;
        private IAction _action;

        public void SetCondition(ICondition condition) => _condition = condition;
        public void SetAction(IAction action) => _action = action;
        
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
            
            _action?.Invoke();
            _jumpRequested = false;
            OnJump?.Invoke();
        }
    }
}