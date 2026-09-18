using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Snake
{
    public class Snake : MonoBehaviour,
                        MoveComponent.IAction,
                        MoveComponent.ICondition,
                        FlipComponent.IAction,
                        FlipComponent.ICondition
    {
        [SerializeField]
        private float _damage;

        private GameObject _target;

        private PatrolComponent _patrolComponent;
        private ChaseComponent _chaseComponent;
        private HealthComponent _healthComponent;
        private LookComponent _lookComponent;
        
        private MoveComponent _moveComponent;
        private TransformMoveComponent _transformMoveComponent;
        
        private CollisionComponent _collisionComponent;
        private TriggerComponent _triggerComponent;
        
        private FlipComponent _flipComponent;
        private RigidbodyFlipComponent _rigidbodyFlipComponent;
        
        private GroundedComponent _groundedComponent;

        private void Awake()
        {
            _patrolComponent = GetComponent<PatrolComponent>();
            _chaseComponent = GetComponent<ChaseComponent>();
            _healthComponent = GetComponent<HealthComponent>();
            _lookComponent = GetComponent<LookComponent>();
            
            _moveComponent = GetComponent<MoveComponent>();
            _transformMoveComponent = GetComponent<TransformMoveComponent>();
            
            _collisionComponent = GetComponent<CollisionComponent>();
            _triggerComponent = GetComponent<TriggerComponent>();
            
            _flipComponent = GetComponent<FlipComponent>();
            _rigidbodyFlipComponent = GetComponent<RigidbodyFlipComponent>();
            
            _groundedComponent = GetComponent<GroundedComponent>();
            _groundedComponent.SetCondition(() => _healthComponent.IsAlive);
        
            _moveComponent.SetAction(this);
            _moveComponent.SetCondition(this);
            
            _flipComponent.SetAction(this);
            _flipComponent.SetCondition(this);
        }

        private void OnEnable()
        {
            _healthComponent.OnDied += this.OnDied;
            _collisionComponent.OnEntered += this.OnCollision;
            _triggerComponent.OnEntered += this.GetChaseTarget;
            _triggerComponent.OnExited += this.ReleaseChaseTarget;
        }

        private void OnDisable()
        {
            _healthComponent.OnDied -= this.OnDied;
            _collisionComponent.OnEntered -= this.OnCollision;
            _triggerComponent.OnEntered -= this.GetChaseTarget;
            _triggerComponent.OnExited -= this.ReleaseChaseTarget;
        }
        
        private void Update()
        {
            if (_chaseComponent.HasTarget)
            {
                Debug.Log($"CHASING");
                _chaseComponent.UpdateDirection();
                if (_chaseComponent.Direction != Vector2.zero)
                    _moveComponent.Move(_chaseComponent.Direction);
            }
            else
            {
                _patrolComponent.UpdateDirection();
                if (_patrolComponent.Direction != Vector2.zero)
                    _moveComponent.Move(_patrolComponent.Direction);
            }
        }

        private void OnDied() => Destroy(this.gameObject);

        private void OnCollision(Collision2D obj)
        {
            if (obj.collider.TryGetComponent<Character>(out var character))
                _flipComponent.Flip();
            
            if (!obj.collider.TryGetComponent<HealthComponent>(out var health)
                || !health.IsAlive) return;
            
            health.TakeDamage(_damage);
        }
        
        private void GetChaseTarget(Collider2D obj)
        {
            if (obj.TryGetComponent<Character>(out var target))
            {
                _chaseComponent.SetTarget(target.gameObject);
            }
        }
        
        private void ReleaseChaseTarget(Collider2D obj)
        {
            if (obj.TryGetComponent<Character>(out var target))
            {
                _chaseComponent.SetTarget(null);
            }
        }
        
        void MoveComponent.IAction.Invoke(Vector2 direction)
        { 
            _transformMoveComponent.Move(direction);
            _lookComponent.Look(direction.x); 
        }
        
        bool MoveComponent.ICondition.Evaluate() 
            => _healthComponent.IsAlive;

        void FlipComponent.IAction.Invoke() 
            => _rigidbodyFlipComponent.Flip();
        
        bool FlipComponent.ICondition.Evaluate()
            => _healthComponent.IsAlive && 
               _groundedComponent.IsGrounded;
    }
}