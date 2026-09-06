using UnityEngine;

namespace Game
{
    public class PushView : MonoBehaviour
    {
        private int PUSH = Animator.StringToHash("BlowForward");
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _pushSFX;
        [SerializeField] private ParticleSystem _pushVFX;
        private Animator _animator;
        
        private void OnEnable()
        {
            _animator = GetComponentInChildren<Animator>();
        }
        
        public void Push()
        {
            _animator.SetTrigger(PUSH);
            _pushVFX.Play();
            _audioSource.PlayOneShot(_pushSFX);
        }
    }
}