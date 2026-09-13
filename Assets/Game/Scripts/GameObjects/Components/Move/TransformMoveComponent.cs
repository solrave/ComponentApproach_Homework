using NUnit.Framework;
using UnityEngine;

namespace Game
{
    public sealed class TransformMoveComponent : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 4.5f;

        public void Move(Vector2 direction)
        {
            this.transform.Translate((Vector3) direction * (_speed * Time.fixedDeltaTime), Space.World);
        }
    }
}