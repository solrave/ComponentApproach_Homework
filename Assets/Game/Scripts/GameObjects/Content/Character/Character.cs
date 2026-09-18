using Game;
using UnityEngine;

public class Character : MonoBehaviour,
                         MoveComponent.IAction,
                         MoveComponent.ICondition,
                         JumpComponent.IAction,
                         JumpComponent.ICondition,
                         PushComponent.IAction,
                         PushComponent.ICondition,
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
    private PushComponent _pushComponent;
    private RigidbodyPushComponent _rigidbodyPushComponent;
    private FlipComponent _flipComponent;
    private RigidbodyFlipComponent _rigidbodyFlipComponent;

    private void Awake()
    {
        _rigidbodyJumpComponent = GetComponent<RigidbodyJumpComponent>();
        _jumpComponent = GetComponent<JumpComponent>();
        
        _moveComponent = GetComponent<MoveComponent>();
        _transformMoveComponent = GetComponent<TransformMoveComponent>();

        _pushComponent = GetComponent<PushComponent>();
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
        
        _pushComponent.SetAction(this);
        _pushComponent.SetCondition(this);
        
        _flipComponent.SetAction(this);
        _flipComponent.SetCondition(this);
    }

    void MoveComponent.IAction.Invoke(Vector2 direction) { _transformMoveComponent.Move(direction);
                                                           _lookComponent.Look(direction.x); }
    bool MoveComponent.ICondition.Evaluate() => _healthComponent.IsAlive;

    void JumpComponent.IAction.Invoke() => _rigidbodyJumpComponent.JumpUp();
    bool JumpComponent.ICondition.Evaluate() => _healthComponent.IsAlive 
                                                && _groundedComponent.IsGrounded;
    
    void PushComponent.IAction.Invoke() => _rigidbodyPushComponent.PushForward();
    bool PushComponent.ICondition.Evaluate() => _healthComponent.IsAlive 
                                                && !_pushComponent.PushRequested;
    
    void FlipComponent.IAction.Invoke() => _rigidbodyFlipComponent.Flip();
    bool FlipComponent.ICondition.Evaluate() => _healthComponent.IsAlive
                                                && _groundedComponent.IsGrounded 
                                                && !_pushComponent.PushRequested;
}
