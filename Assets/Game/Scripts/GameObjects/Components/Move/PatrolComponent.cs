using UnityEngine;

namespace Game
{
    public class PatrolComponent : MonoBehaviour
    {
        public Vector2 Direction { get; private set; }
        
        [SerializeField]
        private Transform[] _patrolPoints;
        
        [SerializeField] 
        private float _stoppingDistance;

        private int _currentIndex;
        private Transform _target;
        private bool _hasTargets;
        
        private void Awake()
        {
            _hasTargets = _patrolPoints is not null && _patrolPoints.Length > 0;
            if (_hasTargets)
            {
                foreach (var point in _patrolPoints)
                {
                    point.SetParent(null);
                }
            }
            else
            {
                Debug.LogWarning("There is no patrol points in the list!", this);
                Direction = Vector2.zero;
            }
        }

        public void UpdateDirection()
        {
            if (!_hasTargets) 
                return;
            
            _target = _patrolPoints[_currentIndex];
            
            if (!_target)
            {
                Direction = Vector2.zero;
                return;
            }

            var offset = _target.position - this.transform.position;
           
            if (offset.sqrMagnitude <= _stoppingDistance * _stoppingDistance)
            {
                _currentIndex = (_currentIndex + 1) % _patrolPoints.Length;
                Direction = Vector2.zero;
                return;
            }

            Direction = offset.normalized;
        }
    }
}