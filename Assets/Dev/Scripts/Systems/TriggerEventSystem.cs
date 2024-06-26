using System.Collections.Generic;
using GoodCat.EcsLite.Shared;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Systems
{
    public class TriggerEventSystem : IEcsRunSystem
    {
        [EcsInject] private DependenciesContainer _dependenciesContainer;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var triggerZones = world.Filter<TriggerZoneComponent>().Inc<TriggerEventComponent>()
                .Inc<TransformRefComponent>().End();

            var triggerZonesPool = world.GetPool<TriggerZoneComponent>();
            var tagMoveToBagPool = world.GetPool<ObjectMoveToBagEvent>();
            var tagMoveDepositPool = world.GetPool<ObjectMoveToDepositEvent>();
            var triggerEnterEventsPool = world.GetPool<TriggerEventComponent>();

            var takeEventsPool = world.GetPool<ObjectTakeEvent>();
            var depositEventsPool = world.GetPool<TriggerDepositEvent>();

            foreach (var triggerZoneIndex in triggerZones)
            {
                ref TriggerZoneComponent triggerZoneComponent = ref triggerZonesPool.Get(triggerZoneIndex);

                int triggeredEntity = triggerZoneComponent.TriggeredEntity;

                Debug.Log($"trigger event gained");

                var isTakeEvent = takeEventsPool.Has(triggerZoneIndex);
                var isDepositEvent = depositEventsPool.Has(triggerZoneIndex);
                
                if (isTakeEvent)
                {
                    takeEventsPool.Del(triggerZoneIndex);

                    var spawnedObjectsStack =
                        _dependenciesContainer.GameState.SpawnedStackObjectsInSpawnZones[triggerZoneIndex];

                    for (var index = spawnedObjectsStack.Count - 1; index >= 0; index--)
                    {
                        var objectStack = spawnedObjectsStack[index];
                        int entityId = objectStack.EntityId;

                        ref var moveToBagComponent = ref tagMoveToBagPool.Add(entityId);
                        moveToBagComponent.BagOwnerEntity = triggeredEntity;
                        moveToBagComponent.SpawnZoneEntity = triggerZoneIndex;
                    }
                }

                if (isDepositEvent)
                {
                    int bagIndex = _dependenciesContainer.GameState.OwnersToBags[triggeredEntity];
                    
                    List<ObjectStack> objectsStackList = _dependenciesContainer.GameState.SpawnedStackObjectsInBag[bagIndex];

                    for (var index = objectsStackList.Count - 1; index >= 0; index--)
                    {
                        var objectStack = objectsStackList[index];
                        int entityId = objectStack.EntityId;

                        ref var moveToDepositEvent = ref tagMoveDepositPool.Add(entityId);
                        moveToDepositEvent.SpawnZoneEntity = triggerZoneIndex;
                        moveToDepositEvent.BagOwnerEntity = triggeredEntity;
                        moveToDepositEvent.Index = objectsStackList.Count + index;
                    }

                    depositEventsPool.Del(triggerZoneIndex);
                   
                }

                triggerEnterEventsPool.Del(triggerZoneIndex);
            }
        }
    }

    public struct BagOwnerComponent
    {
        public int BagEntity;
    }
}