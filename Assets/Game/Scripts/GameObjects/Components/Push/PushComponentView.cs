using System;
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
            _rigidbodyPushComponent.OnPush += this.AnimatePush;
        }

        private void OnDisable()
        {
            _rigidbodyPushComponent.OnPush -= this.AnimatePush;
        }

        private void AnimatePush()
        {
            _animator.SetTrigger(PUSH);
            
            if (_pushVFX)
                _pushVFX.Play();
            
            _audioSource.PlayOneShot(_pushSFX);
        }
    }
}