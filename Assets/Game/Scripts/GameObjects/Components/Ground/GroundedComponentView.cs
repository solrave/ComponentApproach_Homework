using Game;
using UnityEngine;

public class GroundedComponentView : MonoBehaviour
{
    private int GROUNDED = Animator.StringToHash("IsGrounded");
    private int FALLING = Animator.StringToHash("IsFalling");

    private GroundedComponent _groundedComponent;
    private Animator _animator;

    private void OnEnable()
    {
        _groundedComponent = GetComponentInParent<GroundedComponent>();
        _animator = GetComponentInChildren<Animator>();
    }
    
    private void Update()
    {
        _animator.SetBool(GROUNDED, _groundedComponent.IsGrounded);
        _animator.SetBool(FALLING, _groundedComponent.IsGrounded);
    }
}
