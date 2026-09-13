using System;
using UnityEngine;

namespace Game
{
    public class FlipComponentView : MonoBehaviour
    {
        private int FLIP = Animator.StringToHash("BlowUp");
        
        [SerializeField]
        private AudioSource _audioSource;
        
        [SerializeField] 
        private AudioClip _flipSFX;
        
        [SerializeField]
        private ParticleSystem _flipVFX;
        
        private Animator _animator;
        private RigidbodyFlipComponent _rigidbodyFlipComponent;
        
        private void OnEnable()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigidbodyFlipComponent = GetComponentInParent<RigidbodyFlipComponent>();
            _rigidbodyFlipComponent.OnFlip += this.AnimateFlip;
        }

        private void OnDisable()
        {
            _rigidbodyFlipComponent.OnFlip -= this.AnimateFlip;
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