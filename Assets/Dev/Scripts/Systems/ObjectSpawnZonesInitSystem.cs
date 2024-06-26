using System.Collections.Generic;
using Client.Systems;
using Dev.Systems;
using GoodCat.EcsLite.Shared;
using Leopotam.EcsLite;

namespace Client
{
    public class ObjectSpawnZonesInitSystem : IEcsInitSystem
    {
        [EcsInject] private DependenciesContainer _dependenciesContainer;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var stackDataPool = world.GetPool<StackDataComponent>();
            var stackUIPool = world.GetPool<StackUIComponent>();
            
            foreach (var stackSpawnZone in _dependenciesContainer.ObjectsStackSpawnZones)
            {
                int spawnZoneEntity = world.NewEntity();
    
                ref StackDataComponent stackDataComponent = ref stackDataPool.Add(spawnZoneEntity);
                stackDataComponent.ObjectsAmount = 0;
                stackDataComponent.TriggerZoneIndex = spawnZoneEntity;

                stackUIPool.Add(spawnZoneEntity);

                stackSpawnZone.Init(spawnZoneEntity, world);
                
                var triggerZoneComponentPool = world.GetPool<TriggerZoneComponent>();
                var transformsPool = world.GetPool<TransformRefComponent>();
                var tagPickUpZonePool = world.GetPool<TagObjectStackPickUpZoneComponent>();
                var timersPool = world.GetPool<TimerComponent>();

                ref var transformRefComponent = ref transformsPool.Add(spawnZoneEntity);
                transformRefComponent.Transform = stackSpawnZone.transform;

                ref var timerComponent = ref timersPool.Add(spawnZoneEntity);
                timerComponent.TargetTime = _dependenciesContainer.GameSettings.ObjectStackGenerationCooldown;
                timerComponent.CurrentTime = 0;
            
                tagPickUpZonePool.Add(spawnZoneEntity);
                triggerZoneComponentPool.Add(spawnZoneEntity);
                
                _dependenciesContainer.GameState.SpawnedStackObjectsInSpawnZones.Add(spawnZoneEntity, new List<ObjectStack>());
            }
        }
    }
}