using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Grounded), typeof(Rigidbody2D))]
    public sealed class ExtraGravityComponent : MonoBehaviour
    {
        [SerializeField]
        private float _gravity = -7f;
        
        private Grounded _grounded;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _grounded = this.GetComponent<Grounded>();
            _rigidbody = this.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!_grounded.IsGrounded)
                _rigidbody.linearVelocity += new Vector2(0, _gravity * Time.fixedDeltaTime);
        }
    }
}