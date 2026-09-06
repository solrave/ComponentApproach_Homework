using Game;
using UnityEngine;

public class Character : MonoBehaviour,
                         MoveRequest.IAction,
                         MoveRequest.ICondition,
                         JumpRequest.IAction,
                         JumpRequest.ICondition,
                         PushRequest.IAction,
                         PushRequest.ICondition,
                         FlipRequest.IAction,
                         FlipRequest.ICondition
{
    private MoveRequest _moveRequest;
    private MoveTransform _moveTransform;
    private LookComponent _lookComponent;
    private CollisionComponent _collisionComponent;
    private Health _health;
    private Grounded _grounded;
    private JumpRequest _jumpRequest;
    private JumpAbility _jumpAbility;
    private PushRequest _pushRequest;
    private PushAbility _pushAbility;
    private PushView _pushView;
    private FlipRequest _flipRequest;
    private FlipAbility _flipAbility;
    private FlipView _flipView;

    private void Awake()
    {
        _jumpAbility = GetComponent<JumpAbility>();
        _jumpRequest = GetComponent<JumpRequest>();
        
        _moveRequest = GetComponent<MoveRequest>();
        _moveTransform = GetComponent<MoveTransform>();

        _pushRequest = GetComponent<PushRequest>();
        _pushAbility = GetComponent<PushAbility>();
        _pushView = GetComponent<PushView>();
        
        
        _flipRequest = GetComponent<FlipRequest>();
        _flipAbility = GetComponent<FlipAbility>();
        _flipView = GetComponent<FlipView>();
        
        _lookComponent = GetComponent<LookComponent>();
        _collisionComponent = GetComponent<CollisionComponent>();
        _health = GetComponent<Health>();
        _grounded = GetComponent<Grounded>();
        _grounded.SetCondition(() => _health.IsAlive);
        
        _moveRequest.SetAction(this);
        _moveRequest.SetCondition(this);
        
        _jumpRequest.SetAction(this);
        _jumpRequest.SetCondition(this);
        
        _pushRequest.SetAction(this);
        _pushRequest.SetCondition(this);
        
        _flipRequest.SetAction(this);
        _flipRequest.SetCondition(this);
    }

    void MoveRequest.IAction.Invoke(Vector3 direction)
    { 
        _moveTransform.Move(direction);
        _lookComponent.Look(direction.x);
    }
    
    bool MoveRequest.ICondition.Evaluate() => _health.IsAlive;

    void JumpRequest.IAction.Invoke() => _jumpAbility.JumpUp();
    
    bool JumpRequest.ICondition.Evaluate() => _health.IsAlive 
                                              && _grounded.IsGrounded;

    void PushRequest.IAction.Invoke()
    {
        _pushAbility.Push();
        _pushView.Push();
    }

    bool PushRequest.ICondition.Evaluate() => _health.IsAlive 
                                              && !_pushRequest.PushRequested;

    void FlipRequest.IAction.Invoke()
    {
        _flipAbility.Flip();
        _flipView.Flip();
    }

    bool FlipRequest.ICondition.Evaluate() => _health.IsAlive
                                              && _grounded.IsGrounded 
                                              && !_pushRequest.PushRequested;
}
