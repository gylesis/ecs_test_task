using GoodCat.EcsLite.Shared;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Systems
{
    public class ObjectsFollowBagSystem : IEcsRunSystem
    {
        [EcsInject] private DependenciesContainer _dependenciesContainer;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var bagsFilter = world.Filter<BagComponent>().Inc<TransformRefComponent>().End();
            var stackObjectsInBagFilter = world.Filter<ObjectStackInBagComponent>().Inc<TransformRefComponent>().End();

            var transformsPool = world.GetPool<TransformRefComponent>();
            var stackObjectsInBagPool = world.GetPool<ObjectStackInBagComponent>();

            foreach (var bagIndex in bagsFilter)
            {
                Transform bagTransform = transformsPool.Get(bagIndex).Transform;

                foreach (var stackObjIndex in stackObjectsInBagFilter)
                {
                    ObjectStackInBagComponent objectStackInBagComponent = stackObjectsInBagPool.Get(stackObjIndex);
                    int objIndex = objectStackInBagComponent.Index;
                    int stackObjBagEntity = objectStackInBagComponent.BagEntity;

                    if (bagIndex == stackObjBagEntity)
                    {
                        Transform stackObjTransform = transformsPool.Get(stackObjIndex).Transform;

                        Vector3 targetPos = _dependenciesContainer.GetBagStackPosition(bagTransform.position, objIndex);

                        int bagObjectsCount = _dependenciesContainer.GameState.SpawnedStackObjectsInBag[bagIndex].Count;
                        
                        float ratio = 1 - _dependenciesContainer.GameSettings.ObjectStackFollowSpeedCurve.Evaluate(
                            (float)objIndex / bagObjectsCount);

                        float speed = _dependenciesContainer.GameSettings.ObjectStackFollowSpeed;

                        float t = Time.deltaTime * (ratio * speed);

                        stackObjTransform.position = Vector3.Lerp(stackObjTransform.position, targetPos, t);
                        stackObjTransform.rotation = Quaternion.LookRotation(bagTransform.forward);
                    }
                }
            }
        }
    }
}