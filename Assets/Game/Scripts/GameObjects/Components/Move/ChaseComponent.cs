using UnityEngine;

namespace Game
{
    public class ChaseComponent : MonoBehaviour
    {
        private GameObject _target;
        public Vector2 Direction { get; private set; }
        public bool HasTarget => _target != null;
        public void SetTarget(GameObject target) => _target = target;

        public void UpdateDirection()
        {
            if (_target == null)
            {
                Direction = Vector2.zero;
                return;
            }

            var offset = _target.transform.position - this.transform.position;
            Direction = offset.normalized;
        }
    }
}