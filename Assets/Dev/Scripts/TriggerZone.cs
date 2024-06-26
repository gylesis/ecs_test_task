using System;
using UnityEngine;

namespace Client
{
    [RequireComponent(typeof(Collider))]
    public class TriggerZone : MonoBehaviour
    {
        [SerializeField] private Collider _collider;

        public event Action<Collider> TriggerEnter;
        
        private void Reset()
        {
            _collider = GetComponent<Collider>();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            TriggerEnter?.Invoke(other);
        }
    }
}