using UnityEngine;

namespace Game
{
    public class PushComponent : MonoBehaviour
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

        [SerializeField] 
        private Cooldown _cooldown;
        
        private IAction _action;
        private ICondition _condition;
        private bool _pushRequested;

        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;

        public void Push()
        {
            if (!_cooldown.Ready) return;
            if (!(_condition == null || _condition.Evaluate())) return;
            
            _cooldown.Trigger();
            _pushRequested = true;
        }

        private void FixedUpdate()
        {
            _cooldown.Tick(Time.fixedDeltaTime);
            
            if (!_pushRequested) return;
            
            _action?.Invoke();
            _pushRequested = false;
        }
    }
}