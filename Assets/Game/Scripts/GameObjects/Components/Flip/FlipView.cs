using UnityEngine;

namespace Game
{
    public class FlipView : MonoBehaviour
    {
        private int FLIP = Animator.StringToHash("BlowUp");
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _flipSFX;
        [SerializeField] private ParticleSystem _flipVFX;
        private Animator _animator;
        
        private void OnEnable()
        {
            _animator = GetComponentInChildren<Animator>();
        }
        
        public void Flip()
        {
            _animator.SetTrigger(FLIP);
            _flipVFX.Play();
            _audioSource.PlayOneShot(_flipSFX);
        }
    }
}