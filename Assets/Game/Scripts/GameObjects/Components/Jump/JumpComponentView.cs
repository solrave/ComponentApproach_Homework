using UnityEngine;

namespace Game
{
    public class JumpComponentView : MonoBehaviour
    {
        private int JUMP = Animator.StringToHash("Jump");
        
        [SerializeField] 
        private AudioSource _audioSource;
        
        [SerializeField] 
        private AudioClip _jumpSFX;
        
        private Animator _animator;
        private JumpComponent _jumpComponent;
        
        private void OnEnable()
        {
            _animator = GetComponentInChildren<Animator>();
            _jumpComponent = GetComponentInParent<JumpComponent>();
            _jumpComponent.OnJump += this.AnimateJump;
        }

        private void OnDisable()
        {
            _jumpComponent.OnJump -= this.AnimateJump;
        }

        private void AnimateJump()
        {
            _animator.SetTrigger(JUMP);
            _audioSource.PlayOneShot(_jumpSFX);
        }
    }
}