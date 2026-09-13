using Game;
using UnityEngine;

public class Character : MonoBehaviour,
                         MoveComponent.IAction,
                         MoveComponent.ICondition,
                         JumpComponent.IAction,
                         JumpComponent.ICondition,
                         DirectPushComponent.IAction,
                         DirectPushComponent.ICondition,
                         FlipComponent.IAction,
                         FlipComponent.ICondition
{
    private MoveComponent _moveComponent;
    private TransformMoveComponent _transformMoveComponent;
    private LookComponent _lookComponent;
    private CollisionComponent _collisionComponent;
    private HealthComponent _healthComponent;
    private GroundedComponent _groundedComponent;
    private JumpComponent _jumpComponent;
    private RigidbodyJumpComponent _rigidbodyJumpComponent;
    private DirectPushComponent _directPushComponent;
    private RigidbodyPushComponent _rigidbodyPushComponent;
    private FlipComponent _flipComponent;
    private RigidbodyFlipComponent _rigidbodyFlipComponent;

    private void Awake()
    {
        _rigidbodyJumpComponent = GetComponent<RigidbodyJumpComponent>();
        _jumpComponent = GetComponent<JumpComponent>();
        
        _moveComponent = GetComponent<MoveComponent>();
        _transformMoveComponent = GetComponent<TransformMoveComponent>();

        _directPushComponent = GetComponent<DirectPushComponent>();
        _rigidbodyPushComponent = GetComponent<RigidbodyPushComponent>();
        
        _flipComponent = GetComponent<FlipComponent>();
        _rigidbodyFlipComponent = GetComponent<RigidbodyFlipComponent>();
        
        _lookComponent = GetComponent<LookComponent>();
        _collisionComponent = GetComponent<CollisionComponent>();
        _healthComponent = GetComponent<HealthComponent>();
        _groundedComponent = GetComponent<GroundedComponent>();
        _groundedComponent.SetCondition(() => _healthComponent.IsAlive);
        
        _moveComponent.SetAction(this);
        _moveComponent.SetCondition(this);
        
        _jumpComponent.SetAction(this);
        _jumpComponent.SetCondition(this);
        
        _directPushComponent.SetAction(this);
        _directPushComponent.SetCondition(this);
        
        _flipComponent.SetAction(this);
        _flipComponent.SetCondition(this);
    }

    void MoveComponent.IAction.Invoke(Vector2 direction) { _transformMoveComponent.Move(direction);
                                                           _lookComponent.Look(direction.x); }
    bool MoveComponent.ICondition.Evaluate() => _healthComponent.IsAlive;

    void JumpComponent.IAction.Invoke() => _rigidbodyJumpComponent.JumpUp();
    bool JumpComponent.ICondition.Evaluate() => _healthComponent.IsAlive 
                                                && _groundedComponent.IsGrounded;
    
    void DirectPushComponent.IAction.Invoke() => _rigidbodyPushComponent.Push();
    bool DirectPushComponent.ICondition.Evaluate() => _healthComponent.IsAlive 
                                                && !_directPushComponent.PushRequested;
    
    void FlipComponent.IAction.Invoke() => _rigidbodyFlipComponent.Flip();
    bool FlipComponent.ICondition.Evaluate() => _healthComponent.IsAlive
                                                && _groundedComponent.IsGrounded 
                                                && !_directPushComponent.PushRequested;
    
}
