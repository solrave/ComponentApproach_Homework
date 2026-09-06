using System;
using UnityEngine;

namespace Game
{
    public class JumpRequest : MonoBehaviour
    {
        public interface IAction
        {
            void Invoke();
        }
        
        public interface ICondition
        {
            bool Evaluate();
        }
        
        [SerializeField] private float _cooldown;
        [SerializeField] private float _delay;
        private float _jumpTime;
        private float _lastJump;
        private bool _jumpRequested;
        private ICondition _condition;
        private IAction _action;

        public void SetCondition(ICondition condition) => _condition = condition;
        public void SetAction(IAction action) => _action = action;

        private void FixedUpdate()
        {
            if (!(Time.fixedTime >= _lastJump + _cooldown) && (Time.fixedTime < _jumpTime)) return;
            
            if  (_jumpRequested && (_condition == null || _condition.Evaluate()))
            {
                _action?.Invoke();
                _lastJump = Time.fixedTime;
                _jumpRequested = false;
            }
        }

        public void JumpUp()
        {
            if (_condition.Evaluate())
            {
                _jumpRequested = true;
                _jumpTime = Time.fixedTime + _delay;
            }
        }
    }
}