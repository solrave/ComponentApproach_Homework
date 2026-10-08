using Game;
using UnityEngine;

public interface ICharacterForceComponent
{
    void Push();
    void Flip();
}

public class Character : MonoBehaviour,
                         MoveComponent.IAction,
                         MoveComponent.ICondition,
                         JumpComponent.ICondition,
                         ForceComponent.IPushCondition,
                         ForceComponent.IFlipCondition,
                         ICharacterForceComponent
{
    [SerializeField] 
    private ForceComponent _pushComponent;
    
    [SerializeField] 
    private ForceComponent _flipComponent;
    
    private MoveComponent _moveComponent;
    private LookComponent _lookComponent;
    private HealthComponent _healthComponent;
    private GroundedComponent _groundedComponent;
    private JumpComponent _jumpComponent;
    private ForceComponent _forceComponent;

    private void Awake()
    {
        _forceComponent = GetComponent<ForceComponent>();
        _jumpComponent = GetComponent<JumpComponent>();
        _moveComponent = GetComponent<MoveComponent>();
        _lookComponent = GetComponent<LookComponent>();
        _healthComponent = GetComponent<HealthComponent>();
        _groundedComponent = GetComponent<GroundedComponent>();
        _groundedComponent.SetCondition(() => _healthComponent.IsAlive);
        _moveComponent.SetAction(this);
        _moveComponent.SetCondition(this);
        _jumpComponent.SetCondition(this);
        _forceComponent.SetPushCondition(this);
        _forceComponent.SetFlipCondition(this);
    }
    
    void MoveComponent.IAction.Invoke(Vector2 direction)
        { _lookComponent.Look(direction.x);}
    
    bool MoveComponent.ICondition.Evaluate() 
        => _healthComponent.IsAlive;
    
    bool JumpComponent.ICondition.Evaluate() 
        => _healthComponent.IsAlive 
           && _groundedComponent.IsGrounded;
    
    bool ForceComponent.IPushCondition.Evaluate() 
        => _healthComponent.IsAlive 
           && !_forceComponent.PushAllowed;
    
    bool ForceComponent.IFlipCondition.Evaluate() 
        => _healthComponent.IsAlive
           && _groundedComponent.IsGrounded 
           && !_forceComponent.PushAllowed;

    void ICharacterForceComponent.Push()
    {
        _pushComponent.PushForward();
    }

    void ICharacterForceComponent.Flip()
    {
        _flipComponent.FlipUp();
    }
}
