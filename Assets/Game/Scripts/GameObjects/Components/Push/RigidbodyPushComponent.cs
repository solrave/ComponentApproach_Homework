using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RigidbodyPushComponent : MonoBehaviour
    {
        public event Action OnSinglePush;
        public event Action OnSidewayPush;
        
        [SerializeField] 
        private Transform _firePoint;
        
        [SerializeField]
        private LayerMask _layerMask;
        
        [Header("Force")]
        
        [SerializeField]
        private float _forceX;
        
        [SerializeField]
        private float _forceY;
        
        [Header("Raycast settings")]
        
        [SerializeField] 
        private float _radius;
        
        [SerializeField]
        private float _distance;
        
        private Vector2 _force;
        private Vector2 _directionRight;
        private Vector2 _directionLeft;
        
        private void Awake()
        {
            _force = new Vector2(_forceX, _forceY);
            _directionRight = transform.right;
            _directionLeft = - transform.right;
        }

        public void PushAway(Rigidbody2D obj)
        {
            if (!obj) return;

            var direction = (obj.position - (Vector2)transform.position).normalized;
            obj.AddForceAtPosition(direction * _force, obj.position, ForceMode2D.Impulse);
        }

        public void PushForward()
        {
            var targets = Physics2D.OverlapCircleAll(_firePoint.position, _radius, _layerMask);
            
            var rigidbodies = targets
                .Select(c => c.attachedRigidbody)
                .Where(rb => rb != null)
                .Distinct();
            
            foreach (var target in rigidbodies)
            {
                target.AddForceAtPosition(_force, target.position, ForceMode2D.Impulse);
            }
            
            OnSinglePush?.Invoke();
        }

        public void PushSideways()
        {
            var RightHits = Physics2D.CircleCastAll(transform.position, _radius,
                                                            _directionRight, _distance);
            
            var LeftHits = Physics2D.CircleCastAll(transform.position, _radius,
                                                            _directionLeft, _distance);
            
            foreach (var hit in RightHits)
            {
                if (hit.collider.attachedRigidbody)
                {
                    hit.collider.attachedRigidbody.AddForceAtPosition(_force, hit.collider.attachedRigidbody.position,
                                                                        ForceMode2D.Impulse);
                }
            }
            
            foreach (var hit in LeftHits)
            {
                if (hit.collider.attachedRigidbody)
                {
                    hit.collider.attachedRigidbody.AddForceAtPosition(- _force,
                        hit.collider.attachedRigidbody.position, ForceMode2D.Impulse);
                }
            }
            
            OnSidewayPush?.Invoke();
        }
    }
}