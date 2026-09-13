using System;
using Game.Scripts.GameObjects.Content.Spider;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class PatrolComponent : MonoBehaviour
    {
        [SerializeField] private Transform[] _patrolPoints;
        [SerializeField] private float _stoppingDistance;
        [SerializeField] private Spider _spider;
        private MoveComponent _moveComponent;
        private int _currentIndex;
        private Vector2 _direction;
        private Transform _target;
        private bool IsReached => _direction.sqrMagnitude <= _stoppingDistance;

        private void Awake()
        {
            _moveComponent = GetComponent<MoveComponent>();
            if (_patrolPoints is not null && _patrolPoints.Length > 0)
            {
                foreach (var point in _patrolPoints)
                {
                    point.SetParent(null);
                }
            }
            else
            {
                Debug.LogWarning("There is no patrol points in the list!");
            }
        }

        private void Update()
        {
            Patrol();
        }

        private void Patrol()
        {
            _target = _patrolPoints[_currentIndex];
            _direction = _target.position - this.transform.position;
            
                if (!IsReached)
                    _moveComponent?.Move(_direction.normalized);
                else
                    _currentIndex = (_currentIndex + 1) % _patrolPoints.Length;
        }
    }
}