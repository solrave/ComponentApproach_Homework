using UnityEngine;

namespace Game
{
    public class MoveComponentView : MonoBehaviour
    {
        private int MOVING = Animator.StringToHash("IsMoving");
        
        private MoveComponent _moveComponent;
        private Animator _animator;
        
        private void OnEnable()
        {
            _moveComponent = GetComponentInParent<MoveComponent>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            _animator.SetBool(MOVING, _moveComponent.IsMoving);
        }
    }
}