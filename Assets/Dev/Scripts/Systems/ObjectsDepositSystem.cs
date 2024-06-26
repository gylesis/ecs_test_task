using Dev.Systems;
using GoodCat.EcsLite.Shared;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Systems
{
    public class ObjectsDepositSystem : IEcsRunSystem
    {
        [EcsInject] private DependenciesContainer _dependenciesContainer;
        private Vector3 _lastPos;
        private int _rowsCount;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var depositObjectsFilter = world.Filter<ObjectMoveToDepositEvent>().Inc<TagObjectStackComponent>()
                .Inc<TransformRefComponent>().End();

            var bagsFilter = world.Filter<BagComponent>().Inc<TransformRefComponent>().End();

            var transformsPool = world.GetPool<TransformRefComponent>();
            var moveToDepositEventPool = world.GetPool<ObjectMoveToDepositEvent>();
            var stackObjectsInBagPool = world.GetPool<ObjectStackInBagComponent>();
            var bagsPool = world.GetPool<BagComponent>();

            var stackDataPool = world.GetPool<StackDataComponent>();


            foreach (var objectIndex in depositObjectsFilter)
            {
                var stackObjectTransformComponent = transformsPool.Get(objectIndex);
                Transform stackObjectTransform = stackObjectTransformComponent.Transform;

                var moveToDepositEvent = moveToDepositEventPool.Get(objectIndex);
                int bagOwnerEntity = moveToDepositEvent.BagOwnerEntity;
                int spawnZoneEntity = moveToDepositEvent.SpawnZoneEntity;

                foreach (var bagIndex in bagsFilter)
                {
                    BagComponent bagComponent = bagsPool.Get(bagIndex);

                    bool isNotOwnerBag = bagComponent.OwnerEntity != bagOwnerEntity;

                    if (isNotOwnerBag) continue;

                    int bagObjectsCount = _dependenciesContainer.GameState.SpawnedStackObjectsInBag[bagIndex].Count;

                    if (bagObjectsCount == 0)
                    {
                        Debug.Log($"Nothing to deposit");
                        continue;
                    }

                    stackObjectsInBagPool.Del(objectIndex);

                    TransformRefComponent depositZoneTransformRefComponent = transformsPool.Get(spawnZoneEntity);
                    Transform zoneTransform = depositZoneTransformRefComponent.Transform;

                    Vector3 targetPosition =
                        GetObjectsDepositPosition(zoneTransform.position, moveToDepositEvent.Index);

                    Quaternion targetRotation = zoneTransform.rotation;

                    stackObjectTransform.position = targetPosition;
                    stackObjectTransform.rotation = targetRotation;

                    ref var stackDataComponent = ref stackDataPool.Get(spawnZoneEntity);
                    stackDataComponent.ObjectsAmount++;

                    moveToDepositEventPool.Del(objectIndex);

                    ObjectStack objectStack = stackObjectTransform.GetComponent<ObjectStack>();

                    _dependenciesContainer.GameState.SpawnedStackObjectsInBag[bagIndex]
                        .Remove(objectStack);

                    _dependenciesContainer.GameState.SpawnedStackObjectsInSpawnZones[spawnZoneEntity]
                        .Add(objectStack);
                }
            }
        }


        public Vector3 GetObjectsDepositPosition(Vector3 startPoint, int number)
        {
            if (number == 0)
            {
                _lastPos = startPoint;
            }

            float offsetX = 0.55f;
            float offsetY = 0.2f;
            float offsetZ = 0.4f;

            if (number % _dependenciesContainer.GameSettings.StackWidth == 0)
            {
                _lastPos.x = startPoint.x;
                _lastPos.z += offsetZ;
                _rowsCount++;
            }
            else
            {
                _lastPos.x += offsetX;
            }

            if (_rowsCount != 0 && _rowsCount % _dependenciesContainer.GameSettings.StackLength == 0)
            {
                _rowsCount++;
                _lastPos.y += offsetY;
                _lastPos.x = startPoint.x;
                _lastPos.z = startPoint.z;
            }

            return _lastPos;
        }
    }

    public struct TriggerDepositEvent
    {
        public int OwnerBagEntity;
    }

    public struct TriggerDepositZone
    {
        public int ObjectsCount;
    }
}