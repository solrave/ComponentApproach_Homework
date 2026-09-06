using UnityEngine;

public class MoveRequest : MonoBehaviour
{
    public bool IsMoving { get; private set; }
    
    private IAction _action;
    private ICondition _condition;
    private Vector3 _direction;
    private bool _moveRequired;
    
    public interface IAction
    {
        void Invoke(Vector3 direction);
    }

    public interface ICondition
    {
        bool Evaluate();
    }

    public void SetAction(IAction action) => _action = action;
    public void SetCondition(ICondition condition) => _condition = condition;

    public void Move(Vector3 direction)
    {
        _direction = direction;
        _moveRequired = true;
    }

    private void FixedUpdate()
    {
        IsMoving = _moveRequired && _direction != Vector3.zero &&
                   (_condition == null || _condition.Evaluate());
        
        if (IsMoving)
            _action?.Invoke(_direction);
        
        _moveRequired = false;
    }
}
