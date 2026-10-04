using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] 
        private float _cooldown;
        
        [SerializeField]
        private float _delay;
        
        public bool Ready => _delayTime <= 0f && _cooldownTime <= 0f;
        private float _cooldownTime;
        private float _delayTime;

        public void Trigger()
        {
            _delayTime = _delay;
        }

        public void Tick(float time)
        {
            if (_cooldownTime > 0f)
                _cooldownTime -= time;

            if (_delayTime <= 0f)
                return;
            
            _delayTime -= time;

            if (_delayTime >= 0f)
                return;
            
            _cooldownTime = _cooldown;
        }
    }
}