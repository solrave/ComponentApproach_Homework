using System;
using UnityEngine;

namespace Game
{
    public sealed class Health : MonoBehaviour
    {
        [SerializeField]
        private float _maxHealth;

        public float CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0;
        public bool IsDead => CurrentHealth <= 0;

        public event Action<float> OnHealthChanged;
        public event Action OnDied;
        public event Action OnDamage;

        private void Awake()
        {
            CurrentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (!IsAlive || damage <= 0)
                return;

            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            OnHealthChanged?.Invoke(CurrentHealth);
            OnDamage?.Invoke();
            
            if (CurrentHealth <= 0)
                OnDied?.Invoke();
        }

        public void SetZero() => 
            this.TakeDamage(this.CurrentHealth);
    }
}