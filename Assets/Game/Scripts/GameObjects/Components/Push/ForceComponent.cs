using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class ForceComponent : MonoBehaviour
    {
        public interface IPushCondition
        {
            bool Evaluate();
        }
        public interface IFlipCondition
        {
            bool Evaluate();
        }
        
        
        public event Action OnFlip;
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

        public bool PushAllowed => _pushAllowed;

        [SerializeField] 
        private Cooldown _cooldown;
        
        private IPushCondition _pushCondition;
        private IFlipCondition _flipCondition;
        private bool _pushAllowed;
        private bool _flipAllowed;
        
        public void SetPushCondition(IPushCondition pushCondition) => _pushCondition = pushCondition;
        public void SetFlipCondition(IFlipCondition flipCondition) => _flipCondition = flipCondition;
        
        private void Awake()
        {
            _force = new Vector2(_forceX, _forceY);
            _directionRight = transform.right;
            _directionLeft = - transform.right;
        }

        public void TryAllowPush()
        {
            if (!_cooldown.Ready) return;
            if (!(_pushCondition == null || _pushCondition.Evaluate())) return;
            _pushAllowed = true;
        }
        
        public void TryAllowFlip()
        {
            if (!_cooldown.Ready) return;
            if (!(_flipCondition == null || _flipCondition.Evaluate())) return;
            _flipAllowed = true;
        }

        private void FixedUpdate()
        {
            _cooldown.Tick(Time.fixedDeltaTime);
            TryAllowFlip();
            TryAllowPush();
        }
        
        public void FlipUp()
        {
            if (!_flipAllowed) return;
            _cooldown.Trigger();
            
            var targets = Physics2D.OverlapCircleAll(_firePoint.position, _radius, _layerMask);
            
            var rigidbodies = targets
                .Select(c => c.attachedRigidbody)
                .Where(rb => rb != null)
                .Distinct();
            
            foreach (var target in rigidbodies)
            {
               // var direction = target.position - (Vector2)transform.position;
                target.AddForceAtPosition( _force, target.position, ForceMode2D.Impulse);
            }
            
            OnFlip?.Invoke();
            _flipAllowed = false;
        }

        public void PushKnockback(Rigidbody2D obj)
        {
            if (!_pushAllowed) return;
            if (!obj) return;

            _cooldown.Trigger();
            var direction = (obj.position - (Vector2)transform.position).normalized;
            obj.AddForceAtPosition(direction * _force, obj.position, ForceMode2D.Impulse);
            _pushAllowed = false;
        }

        public void PushForward()
        {
            if (!_pushAllowed) return;  
            
            _cooldown.Trigger();
            var targets = Physics2D.OverlapCircleAll(_firePoint.position, _radius, _layerMask);
            
            var rigidbodies = targets
                .Select(c => c.attachedRigidbody)
                .Where(rb => rb != null)
                .Distinct();
            
            foreach (var target in rigidbodies)
            {
                var direction = target.position - (Vector2)transform.position;
                target.AddForceAtPosition(direction * _force, target.position, ForceMode2D.Impulse);
            }
            
            OnSinglePush?.Invoke();
            _pushAllowed = false;
        }

        public void PushSideways()
        {
            if (!_pushAllowed) return;
            Debug.Log("Monkey Push!");
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
            _pushAllowed = false;
        }

        private void OnDrawGizmos()
        {
            if (_firePoint == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_firePoint.position, _radius);
        }
    }
}