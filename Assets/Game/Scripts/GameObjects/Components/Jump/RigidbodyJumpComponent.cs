using UnityEngine;

namespace Game
{
    public class RigidbodyJumpComponent : MonoBehaviour
    {
        [SerializeField] private float _force;
        
        private Rigidbody2D _body;
        
        public void JumpUp() =>
            _body.AddForce(Vector2.up * _force, ForceMode2D.Impulse);
        
        private void OnEnable() 
            => _body = GetComponent<Rigidbody2D>();
    }
}