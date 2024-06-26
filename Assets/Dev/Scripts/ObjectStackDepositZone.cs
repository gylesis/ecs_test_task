using Client.Systems;
using UnityEngine;

namespace Client
{
    public class ObjectStackDepositZone : PlayerTriggerZone
    {
        protected override void OnPlayerTriggered(GameObject player)
        {
            int playerEntity = player.GetComponent<Player>().EntityId;
            
            var triggerZoneComponentPool = _world.GetPool<TriggerZoneComponent>();
            var triggerEnterEventsPool = _world.GetPool<TriggerEventComponent>();
            var depositEventPool = _world.GetPool<TriggerDepositEvent>();

            triggerEnterEventsPool.Add(_entityId);
            ref TriggerDepositEvent depositEvent = ref depositEventPool.Add(_entityId);
            depositEvent.OwnerBagEntity = playerEntity;
            
            ref var zoneComponent = ref triggerZoneComponentPool.Get(_entityId);
            zoneComponent.TriggeredEntity = playerEntity;
        }
    }
    
}