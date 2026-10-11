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
        private Transform _chaseTarget;
        private bool _hasTargets;
        private MoveComponent _moveComponent;
        private Vector3 _offset = new Vector3();
        
        private void Awake()
        {
            _moveComponent = GetComponent<MoveComponent>();
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

        public void Patrol()
        {
            if (!_hasTargets) 
                return;
            
            _target = _patrolPoints[_currentIndex];
            
            if (!_target)
            {
                Direction = Vector2.zero;
                return;
            }
            
            if (_chaseTarget)
                _offset = _chaseTarget.position - this.transform.position;
            else
                _offset = _target.position - this.transform.position;

           
            if (_offset.sqrMagnitude <= _stoppingDistance * _stoppingDistance)
            {
                _currentIndex = (_currentIndex + 1) % _patrolPoints.Length;
                Direction = Vector2.zero;
                return;
            }

            Direction = _offset.normalized;
            _moveComponent.Move(Direction);
        }

        public void SetChaseTarget(Transform target)
                    => _chaseTarget = target;

        public void ReleaseChaseTarget() 
                    => _chaseTarget = null;
    }
}