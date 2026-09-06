using System.Linq;
using UnityEngine;

namespace Game
{
    public class FlipAbility : MonoBehaviour
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _forceX;
        [SerializeField] private float _forceY;
        [SerializeField] private float _radius;
       
        public void Flip()
        {
            var direction = new Vector2(_forceX, _forceY);
            var targets = Physics2D.OverlapCircleAll(_firePoint.position, _radius, _layerMask);
            
            var rigidbodies = targets
                .Select(c => c.attachedRigidbody)
                .Where(rb => rb != null)
                .Distinct();
            
            foreach (var target in rigidbodies)
            {
                target.AddForceAtPosition(direction, target.position, ForceMode2D.Impulse);
            }
        }
    }
}