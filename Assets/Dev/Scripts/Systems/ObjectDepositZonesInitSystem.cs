using System.Collections.Generic;
using Client.Systems;
using Dev.Systems;
using GoodCat.EcsLite.Shared;
using Leopotam.EcsLite;

namespace Client
{
    public class ObjectDepositZonesInitSystem : IEcsInitSystem
    {
        [EcsInject] private DependenciesContainer _dependenciesContainer;
            
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var stackDataPool = world.GetPool<StackDataComponent>();
            var stackUIPool = world.GetPool<StackUIComponent>();

            foreach (ObjectStackDepositZone stackSpawnZone in _dependenciesContainer.ObjectsStackDepositZones)
            {
                int spawnZoneEntity = world.NewEntity();
    
                stackSpawnZone.Init(spawnZoneEntity, world);

                ref StackDataComponent stackDataComponent = ref stackDataPool.Add(spawnZoneEntity);
                stackDataComponent.ObjectsAmount = 0;
                stackDataComponent.TriggerZoneIndex = spawnZoneEntity;
                
                stackUIPool.Add(spawnZoneEntity);
                
                var triggerZoneComponentPool = world.GetPool<TriggerZoneComponent>();
                var transformsPool = world.GetPool<TransformRefComponent>();
                var tagPickUpZonePool = world.GetPool<TriggerDepositZone>();

                ref var transformRefComponent = ref transformsPool.Add(spawnZoneEntity);
                transformRefComponent.Transform = stackSpawnZone.transform;

                tagPickUpZonePool.Add(spawnZoneEntity);
                triggerZoneComponentPool.Add(spawnZoneEntity);
                
                _dependenciesContainer.GameState.SpawnedStackObjectsInSpawnZones.Add(spawnZoneEntity, new List<ObjectStack>());
            }
        }
    }
}