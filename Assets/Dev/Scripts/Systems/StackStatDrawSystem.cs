using Client.Systems;
using GoodCat.EcsLite.Shared;
using Leopotam.EcsLite;
using UnityEngine;

namespace Dev.Systems
{
    public class StackStatDrawSystem :  IEcsInitSystem, IEcsRunSystem
    {
        [EcsInject] private DependenciesContainer _dependenciesContainer;
        
        private CameraContainer MainCamera => _dependenciesContainer.MainCameraContainer;


        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var stackUIPool = world.GetPool<StackUIComponent>();
            
            foreach (var zone in _dependenciesContainer.ObjectsStackDepositZones)
            {
                InitStackUIZone(zone.EntityId, zone.transform.position);
            }
            
            foreach (var zone in _dependenciesContainer.ObjectsStackSpawnZones)
            {
                InitStackUIZone(zone.EntityId, zone.transform.position);
            }


            void InitStackUIZone(int zoneEntity, Vector3 spawnPos)
            {
                ref StackUIComponent stackUIComponent = ref stackUIPool.Get(zoneEntity);

                stackUIComponent.View = _dependenciesContainer.Factory.CreateStackUI(spawnPos);
            }
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            var triggerZonesFilter = world.Filter<TransformRefComponent>().Inc<TriggerZoneComponent>().End();

            var transformsPool = world.GetPool<TransformRefComponent>();
            var stackDataPool = world.GetPool<StackDataComponent>();
            var stackUIPool = world.GetPool<StackUIComponent>();

            foreach (var triggerZoneIndex in triggerZonesFilter)
            {
                TransformRefComponent zoneTransformComponent = transformsPool.Get(triggerZoneIndex);
                Transform zoneTransform = zoneTransformComponent.Transform;

                StackDataComponent stackDataComponent = stackDataPool.Get(triggerZoneIndex);

                ref StackUIComponent stackUIComponent = ref stackUIPool.Get(triggerZoneIndex);

                Vector3 targetPos = zoneTransform.transform.position + Vector3.up * 2;
                
                stackUIComponent.View.transform.position = targetPos;
                Vector3 directionToCam = (targetPos - MainCamera.Camera.transform.position).normalized;
                directionToCam = Vector3.ProjectOnPlane(directionToCam, Vector3.up);
                
                stackUIComponent.View.transform.rotation = Quaternion.LookRotation(directionToCam);
                stackUIComponent.View.UpdateAmount(stackDataComponent.ObjectsAmount);
            }
            
        }
    }
}