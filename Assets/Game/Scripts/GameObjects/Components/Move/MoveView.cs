using System;
using UnityEngine;

namespace Game
{
    public class MoveView : MonoBehaviour
    {
        private int MOVING = Animator.StringToHash("IsMoving");
        
        private MoveRequest _moveRequest;
        private Animator _animator;
        
        private void OnEnable()
        {
            _moveRequest = GetComponent<MoveRequest>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            _animator.SetBool(MOVING, _moveRequest.IsMoving);
        }
    }
}