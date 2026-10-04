using UnityEngine;

namespace Game
{
    public class KnockbackPushComponent : MonoBehaviour
    {
        public interface IAction
        {
            void Invoke(Rigidbody2D target);
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
        private Rigidbody2D _target;

        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;

        public void Push(Rigidbody2D target)
        {
            if (!_cooldown.Ready) return;
            if (!(_condition == null || _condition.Evaluate())) return;

            _target = target;
            _cooldown.Trigger();
            _pushRequested = true;
        }

        private void FixedUpdate()
        {
            _cooldown.Tick(Time.fixedDeltaTime);
            
            if (!_pushRequested) return;
            if (!_target) return;
            
            _action?.Invoke(_target);
            _pushRequested = false;
        }
    }
}