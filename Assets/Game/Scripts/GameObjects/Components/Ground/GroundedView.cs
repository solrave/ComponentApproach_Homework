using Game;
using UnityEngine;

public class GroundedView : MonoBehaviour
{
    private int GROUNDED = Animator.StringToHash("IsGrounded");
    private int FALLING = Animator.StringToHash("IsFalling");

    private Grounded _grounded;
    private Animator _animator;

    private void OnEnable()
    {
        _grounded = GetComponent<Grounded>();
        _animator = GetComponentInChildren<Animator>();
    }
    
    private void Update()
    {
        _animator.SetBool(GROUNDED, _grounded.IsGrounded);
        _animator.SetBool(FALLING, _grounded.IsGrounded);
    }
}
