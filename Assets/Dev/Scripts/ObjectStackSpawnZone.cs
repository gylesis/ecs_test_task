using Client.Systems;
using UnityEngine;

namespace Client
{
    public class ObjectStackSpawnZone : PlayerTriggerZone
    {
        protected override void OnPlayerTriggered(GameObject player)
        {
            int playerEntity = player.GetComponent<Player>().EntityId;
            
            var triggerZoneComponentPool = _world.GetPool<TriggerZoneComponent>();
            var triggerEnterEventsPool = _world.GetPool<TriggerEventComponent>();
            var takeEventsPool = _world.GetPool<ObjectTakeEvent>();

            ref ObjectTakeEvent takeEvent = ref takeEventsPool.Add(_entityId);
            takeEvent.OwnerBagEntity = playerEntity;
            
            triggerEnterEventsPool.Add(_entityId);
                
            ref var zoneComponent = ref triggerZoneComponentPool.Get(_entityId);
            zoneComponent.TriggeredEntity = playerEntity;
        }
        
    }
}