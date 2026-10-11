using UnityEngine;

namespace Game
{
    public class Platform : MonoBehaviour
    {
        private MoveComponent _moveComponent;
        private PatrolComponent _patrolComponent;
        private CollisionComponent _collisionComponent;

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
        }

        private void Update()
        {
            _patrolComponent.Patrol();
            
            if (_patrolComponent.Direction != Vector2.zero)
                _moveComponent.Move(_patrolComponent.Direction);
        }
        
        private void OnEnter(Collision2D obj)
        { 
            obj.transform.SetParent(this.transform);
        }
        
        private void OnExit(Collision2D obj)
        {
            obj.transform.SetParent(null);
        }
    }
}

