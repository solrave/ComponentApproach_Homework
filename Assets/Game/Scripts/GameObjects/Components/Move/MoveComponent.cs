using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.5f;
    
    public bool IsMoving { get; private set; }
    
    private IAction _action;
    private ICondition _condition;
    private Vector3 _direction;
    private bool _moveRequired;
    
    public interface IAction
    {
        void Invoke(Vector2 direction);
    }

    public interface ICondition
    {
        bool Evaluate();
    }

    public void SetAction(IAction action) => _action = action;
    public void SetCondition(ICondition condition) => _condition = condition;

    public void Move(Vector2 direction)
    {
        _direction = direction;
        _moveRequired = true;
    }

    private void FixedUpdate()
    {
        InvokeMoveAction();
    }

    private void InvokeMoveAction()
    {
        IsMoving = _moveRequired && _direction != Vector3.zero &&
                   (_condition == null || _condition.Evaluate());
        
        if (IsMoving)
        {
            MoveTransform(_direction);
            _action?.Invoke(_direction);
        }
        
        _moveRequired = false;
    }

    private void MoveTransform(Vector2 direction)
    {
        this.transform.Translate((Vector3) direction * (_speed * Time.fixedDeltaTime), Space.World);
    }
}
