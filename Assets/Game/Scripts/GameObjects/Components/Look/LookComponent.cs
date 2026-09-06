using UnityEngine;

namespace Game
{
    public sealed class LookComponent : MonoBehaviour
    {
        private float _angle;
        public void Look(Transform target)
        {
            Vector2 direction = target.position - this.transform.position;
            this.Look(direction.x);
        }
        
        public void Look(float direction)
        {
            if (direction > 0)
            {
                _angle = 0f;
            }

            if (direction < 0)
            {
                _angle = 180f;
            }
            
            this.transform.eulerAngles = new Vector3(0, _angle, 0);
        }
    }
}