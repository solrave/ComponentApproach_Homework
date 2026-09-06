using UnityEngine;

namespace Game
{
    public sealed class Lava : MonoBehaviour
    {
        [SerializeField]
        private TriggerComponent _trigger;

        private void OnEnable() => _trigger.OnEntered += this.OnTriggerEntered;

        private void OnDisable() => _trigger.OnEntered -= this.OnTriggerEntered;

        private void OnTriggerEntered(Collider2D col)
        {
            Health health = col.GetComponentInParent<Health>();
            if (health != null)
                health.SetZero();
        }
    }
}