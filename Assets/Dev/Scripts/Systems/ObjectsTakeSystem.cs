using Dev.Systems;
using GoodCat.EcsLite.Shared;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Systems
{
    public class ObjectsTakeSystem : IEcsRunSystem
    {
        [EcsInject] private DependenciesContainer _dependenciesContainer;

        private int _counter;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var bagsFilter = world.Filter<BagComponent>().Inc<TransformRefComponent>().End();
            var moveToBagObjectsFilter = world.Filter<ObjectMoveToBagEvent>().Inc<TagObjectStackComponent>()
                .Inc<TransformRefComponent>().End();

            var transformsPool = world.GetPool<TransformRefComponent>();
            var moveToBagsPool = world.GetPool<ObjectMoveToBagEvent>();
            var stackObjectsInBagPool = world.GetPool<ObjectStackInBagComponent>();
            var bagsPool = world.GetPool<BagComponent>();
            var stackDataPool = world.GetPool<StackDataComponent>();

            _counter = 0;

            foreach (var stackObjectIndex in moveToBagObjectsFilter)
            {
                var stackObjectTransformComponent = transformsPool.Get(stackObjectIndex);
                Transform stackObjectTransform = stackObjectTransformComponent.Transform;

                ObjectMoveToBagEvent moveToBagComponent = moveToBagsPool.Get(stackObjectIndex);
                int bagOwnerEntity = moveToBagComponent.BagOwnerEntity;
                int spawnZoneEntity = moveToBagComponent.SpawnZoneEntity;

                foreach (var bagIndex in bagsFilter)
                {
                    var bagComponent = bagsPool.Get(bagIndex);

                    bool isNotOwnerBag = bagComponent.OwnerEntity != bagOwnerEntity;

                    if (isNotOwnerBag) continue;

                    var bagTransformComponent = transformsPool.Get(bagIndex);
                    Transform bagTransform = bagTransformComponent.Transform;

                    int objectsInBagAmount = _dependenciesContainer.GameState.SpawnedStackObjectsInBag[bagIndex].Count;

                    Vector3 targetPosition =
                        _dependenciesContainer.GetBagStackPosition(bagTransform.position,
                            objectsInBagAmount + _counter);

                    stackObjectTransform.position = targetPosition;

                    ref var stackDataComponent = ref stackDataPool.Get(spawnZoneEntity);
                    stackDataComponent.ObjectsAmount--;


                    ref var objectStackInBag = ref stackObjectsInBagPool.Add(stackObjectIndex);
                    objectStackInBag.BagEntity = bagIndex;
                    objectStackInBag.Index = objectsInBagAmount;

                    ObjectStack objectStack = stackObjectTransform.GetComponent<ObjectStack>();

                    _dependenciesContainer.GameState.SpawnedStackObjectsInBag[bagIndex].Add(objectStack);
                    _dependenciesContainer.GameState.SpawnedStackObjectsInSpawnZones[spawnZoneEntity]
                        .Remove(objectStack);

                    moveToBagsPool.Del(stackObjectIndex);
                }

                _counter++;
            }
        }
    }
}