using UnityEngine;

namespace Game
{
    public class PushComponentView : MonoBehaviour
    {
        private int PUSH = Animator.StringToHash("BlowForward");
        
        [SerializeField]
        private AudioSource _audioSource;
        
        [SerializeField] 
        private AudioClip _pushSFX;
        
        [SerializeField]
        private ParticleSystem _pushVFX;
        
        private Animator _animator;
        private RigidbodyPushComponent _rigidbodyPushComponent;
        
        private void OnEnable()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigidbodyPushComponent = GetComponentInParent<RigidbodyPushComponent>();
            _rigidbodyPushComponent.OnSinglePush += this.AnimateSinglePush;
            _rigidbodyPushComponent.OnSidewayPush += this.AnimateSidewayPush;
        }

        private void OnDisable()
        {
            _rigidbodyPushComponent.OnSinglePush -= this.AnimateSinglePush;
            _rigidbodyPushComponent.OnSidewayPush -= this.AnimateSidewayPush;
        }

        private void AnimateSinglePush()
        {
            _animator.SetTrigger(PUSH);
            
            if (_pushVFX)
                _pushVFX.Play();
            
            _audioSource.PlayOneShot(_pushSFX);
        }
        
        private void AnimateSidewayPush()
        {
            if (!_pushVFX) return;
            
            Vector2 basePos = _rigidbodyPushComponent.transform.position;
            
            var vfxPositionRight = new Vector2(basePos.x + 0.5f, basePos.y);
            var vfxPositionLeft = new Vector2(basePos.x - 0.5f, basePos.y);
            
            var rightPuff = Instantiate(_pushVFX, vfxPositionRight, Quaternion.identity);
            var leftPuff = Instantiate(_pushVFX, vfxPositionLeft, Quaternion.Euler(0f, 180f, 0f));
            
            rightPuff.Play();
            leftPuff.Play();
            
            Destroy(rightPuff.gameObject, rightPuff.main.duration);
            Destroy(leftPuff.gameObject, leftPuff.main.duration);
        }
    }
}