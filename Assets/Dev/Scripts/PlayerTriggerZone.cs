using Leopotam.EcsLite;
using UnityEngine;

namespace Client
{
    public class PlayerTriggerZone : TriggerZone
    {
        protected EcsWorld _world;

        protected int _entityId = -1;

        public int EntityId => _entityId;
        
        public void Init(int entityId, EcsWorld ecsWorld)
        {
            _world = ecsWorld;
            _entityId = entityId;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerTriggered(other.gameObject);

                base.OnTriggerEnter(other);
            }
        }

        protected virtual void OnPlayerTriggered(GameObject player) { }
    }
}