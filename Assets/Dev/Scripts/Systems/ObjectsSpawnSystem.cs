using Client.Systems;
using Dev.Systems;
using GoodCat.EcsLite.Shared;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client
{
    public class ObjectsSpawnSystem : IEcsRunSystem
    {
        [EcsInject] private DependenciesContainer _dependenciesContainer;
        private Vector3 _lastPos;
        private int _rowsCount;

        public void Run(IEcsSystems systems)
        {
            //if(_dependenciesContainer.GameState.AreObjectsDepositing) return;
            
            var world = systems.GetWorld();

            var triggerZonesFilter = world.Filter<TransformRefComponent>().Inc<TagObjectStackPickUpZoneComponent>()
                .Inc<TriggerZoneComponent>().Inc<TimerComponent>().End();

            var transformsPool = world.GetPool<TransformRefComponent>();
            var timersPool = world.GetPool<TimerComponent>();
            var stackDataPool = world.GetPool<StackDataComponent>();
            
            foreach (var index in triggerZonesFilter)
            {
                int spawnedObjectsAmount = _dependenciesContainer.GameState.SpawnedStackObjectsInSpawnZones[index].Count;

                if (spawnedObjectsAmount >=
                    _dependenciesContainer.GameSettings.ObjectsStackGeneratedMaxAmount) continue;

                ref var transformRefComponent = ref transformsPool.Get(index);
                var transform = transformRefComponent.Transform;

                ref TimerComponent timerComponent = ref timersPool.Get(index);

                timerComponent.CurrentTime += Time.deltaTime;

                if (timerComponent.CurrentTime >= timerComponent.TargetTime)
                {
                    ref var stackDataComponent = ref stackDataPool.Get(index);
                    stackDataComponent.ObjectsAmount++;

                    Vector3 spawnPos = GetObjectsDepositPosition(transform.position, spawnedObjectsAmount);

                    ObjectStack objectStack = Object.Instantiate(_dependenciesContainer.ObjectStackPrefab, spawnPos,
                        Quaternion.identity);

                    _dependenciesContainer.GameState.SpawnedStackObjectsInSpawnZones[index].Add(objectStack);

                    int newObjectStackEntity = world.NewEntity();

                    objectStack.EntityId = newObjectStackEntity;
                    var objectsTagPool = world.GetPool<TagObjectStackComponent>();

                    ref TransformRefComponent transformComponent = ref transformsPool.Add(newObjectStackEntity);
                    transformComponent.Transform = objectStack.transform;

                    objectsTagPool.Add(newObjectStackEntity);

                    timerComponent.CurrentTime = 0;
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
}