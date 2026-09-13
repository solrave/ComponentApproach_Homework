using UnityEngine;

namespace Game
{
    public class FlipComponent : MonoBehaviour
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
        
        [SerializeField] 
        private CooldownComponent _cooldown;
        
        private IAction _action;
        private ICondition _condition;
        private bool _flipRequested;

        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;

        public void Flip()
        {
            if (!_cooldown.Ready) return;
            if (!(_condition == null || _condition.Evaluate())) return;

            _cooldown.Trigger();
            _flipRequested = true;
        }

        private void FixedUpdate()
        {
            _cooldown.Tick(Time.fixedDeltaTime);
            
            if (!_flipRequested) return;
            
            _action?.Invoke();
            _flipRequested = false;
        }
    }
}