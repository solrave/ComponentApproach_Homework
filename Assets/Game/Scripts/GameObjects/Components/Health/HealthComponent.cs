using System;
using UnityEngine;

namespace Game
{
    public sealed class HealthComponent : MonoBehaviour
    {
        public event Action<float> OnHealthChanged;
        public event Action OnDied;
        public event Action OnDamage;
        
        public bool IsAlive => _currentHealth > 0;
        public bool IsDead => _currentHealth <= 0;
        
        [SerializeField]
        private float _maxHealth;

        private float _currentHealth;
        
        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (!IsAlive || damage <= 0)
                return;

            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            OnHealthChanged?.Invoke(_currentHealth);
            OnDamage?.Invoke();
            
            if (_currentHealth <= 0)
                OnDied?.Invoke();
        }

        public void SetZero() => 
            this.TakeDamage(this._currentHealth);
    }
}