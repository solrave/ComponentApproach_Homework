using UnityEngine;

namespace Game
{
    public class Platform : MonoBehaviour
    {
        private MoveComponent _moveComponent;
        private PatrolComponent _patrolComponent;
        private CollisionComponent _collisionComponent;
        private Transform _load;

        private void Awake()
        {
            _patrolComponent = GetComponent<PatrolComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            _collisionComponent = GetComponent<CollisionComponent>();
        }

        private void OnEnable()
        {
            _collisionComponent.OnEntered += this.OnEnter;
            _collisionComponent.OnExited += this.OnExit;
        }

        private void OnDisable()
        {
            _collisionComponent.OnEntered -= this.OnEnter;
            _collisionComponent.OnExited -= this.OnExit;
            
            if (_load)
                _load.SetParent(null);
            
            _load = null;
        }

        private void Update()
        {
            _patrolComponent.UpdateDirection();
            
            if (_patrolComponent.Direction != Vector2.zero)
                _moveComponent.Move(_patrolComponent.Direction);
        }
        
        private void OnEnter(Collision2D obj)
        {
            _load = obj.transform;
            _load.SetParent(this.transform);
        }
        
        private void OnExit(Collision2D obj)
        {
            if (!obj.transform) return;
            
            obj.transform.SetParent(null);
            _load = null;
        }
    }
}

