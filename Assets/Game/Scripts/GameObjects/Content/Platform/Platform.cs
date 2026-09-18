using Game.Scripts;
using UnityEngine;

namespace Game
{
    public class Platform : MonoBehaviour,
                            MoveComponent.IAction,
                            MoveComponent.ICondition
                           
    {
        private MoveComponent _moveComponent;
        private TransformMoveComponent _transformMoveComponent;
        private PatrolComponent _patrolComponent;
        private CollisionComponent _collisionComponent;
        private Transform _load;

        private void Awake()
        {
            _patrolComponent = GetComponent<PatrolComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            _transformMoveComponent = GetComponent<TransformMoveComponent>();
            _collisionComponent = GetComponent<CollisionComponent>();
            _moveComponent.SetAction(this);
            _moveComponent.SetCondition(this);
            
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
            if (ApplicationQuit.AppIsClosing) return; //??????????
            if (!isActiveAndEnabled) return; //??????????
            if (!obj.transform) return; //??????????
            
            obj.transform.SetParent(null); //??????????
            _load = null;
        }

        void MoveComponent.IAction.Invoke(Vector2 direction)
            => _transformMoveComponent.Move(direction);

        bool MoveComponent.ICondition.Evaluate()
        {
            return true;
        }
    }
}

