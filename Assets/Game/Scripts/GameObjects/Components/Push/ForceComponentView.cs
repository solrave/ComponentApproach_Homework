using UnityEngine;

namespace Game
{
    public class ForceComponentView : MonoBehaviour
    {
        private int PUSH = Animator.StringToHash("BlowForward");
        private int FLIP = Animator.StringToHash("BlowUp");
        
        [Header("PUSH MODEL")] 
        [SerializeField]
        private ForceComponent _pushComponent;
        
        [Header("PUSH VIEW")]
        [SerializeField] 
        private AudioClip _pushSFX;
        [SerializeField]
        private ParticleSystem _pushVFX;
        
        [Header("FLIP MODEL")] 
        [SerializeField]
        private ForceComponent _flipComponent;
        
        [Header("FLIP VIEW")]
        [SerializeField] 
        private AudioClip _flipSFX;
        [SerializeField]
        private ParticleSystem _flipVFX;
        
        [SerializeField]
        private AudioSource _audioSource;
        
        private Animator _animator;
        
        private void OnEnable()
        {
            _animator = GetComponentInChildren<Animator>();
            _pushComponent.OnSinglePush += this.AnimateSinglePush;
            _pushComponent.OnSidewayPush += this.AnimateSidewayPush;
            _flipComponent.OnFlip += this.AnimateFlip;
        }

        private void OnDisable()
        {
            _pushComponent.OnSinglePush -= this.AnimateSinglePush;
            _pushComponent.OnSidewayPush -= this.AnimateSidewayPush;
            _flipComponent.OnFlip -= this.AnimateFlip;
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

            Vector2 basePos = _pushComponent.transform.position;
            
            var vfxPositionRight = new Vector2(basePos.x + 0.5f, basePos.y);
            var vfxPositionLeft = new Vector2(basePos.x - 0.5f, basePos.y);
            
            var rightPuff = Instantiate(_pushVFX, vfxPositionRight, Quaternion.identity);
            var leftPuff = Instantiate(_pushVFX, vfxPositionLeft, Quaternion.Euler(0f, 180f, 0f));
            
            rightPuff.Play();
            leftPuff.Play();
            
            Destroy(rightPuff.gameObject, rightPuff.main.duration);
            Destroy(leftPuff.gameObject, leftPuff.main.duration);
        }
        
         private void AnimateFlip()
        {
            _animator.SetTrigger(FLIP);
            
            if (_flipVFX)
                _flipVFX.Play();
            
            _audioSource.PlayOneShot(_flipSFX);
        }
    }
}