using UnityEngine;

namespace Game
{
    public class FlipRequest : MonoBehaviour
    {
        public interface IAction
        {
            void Invoke();
        }
        
        public interface ICondition
        {
            bool Evaluate();
        }
        
        public bool FlipRequested => _flipRequested;
        
        [SerializeField] private float _cooldown;
        [SerializeField] private float _delay;
        private IAction _action;
        private ICondition _condition;
        private bool _flipRequested;
        private float _lastFlipTime;
        private float _flipTime;

        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;

        public void Flip()
        {
            _flipRequested = true;
            _flipTime = Time.fixedTime;
        }

        private void FixedUpdate()
        {
            if (!(Time.fixedTime >= _flipTime + _delay) &&
                !(Time.fixedTime >= _lastFlipTime + _cooldown)) return;

            if (_flipRequested)
            {
                _action?.Invoke();
                _lastFlipTime = Time.fixedTime;
                _flipRequested = false;
            }
        }
    }
}