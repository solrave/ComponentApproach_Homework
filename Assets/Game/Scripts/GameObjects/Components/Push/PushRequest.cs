using UnityEngine;

namespace Game
{
    public class PushRequest : MonoBehaviour
    {
        public interface IAction
        {
            void Invoke();
        }
        
        public interface ICondition
        {
            bool Evaluate();
        }

        public bool PushRequested => _pushRequested;
        
        [SerializeField] private float _cooldown;
        [SerializeField] private float _delay;
        private IAction _action;
        private ICondition _condition;
        private bool _pushRequested;
        private float _lastPushTime;
        private float _pushTime;

        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;

        public void Push()
        {
            _pushRequested = true;
            _pushTime = Time.fixedTime;
        }

        private void FixedUpdate()
        {
            if (!(Time.fixedTime >= _pushTime + _delay) &&
                !(Time.fixedTime >= _lastPushTime + _cooldown)) return;

            if (_pushRequested)
            {
                _action?.Invoke();
                _lastPushTime = Time.fixedTime;
                _pushRequested = false;
            }
        }
    }
}