using GoodCat.EcsLite.Shared;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Systems
{
    public class MainCameraMovementSystem : IEcsRunSystem
    {
        [EcsInject] private DependenciesContainer _dependenciesContainer;

        private CameraContainer MainCamera => _dependenciesContainer.MainCameraContainer;


        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var players = world.Filter<TagPlayerComponent>().Inc<TransformRefComponent>().End();
            
            var transformsPool = world.GetPool<TransformRefComponent>();

            foreach (var index in players)
            {
                TransformRefComponent transformRefComponent = transformsPool.Get(index);

                MainCamera.CameraParent.position = Vector3.Lerp(MainCamera.CameraParent.position, transformRefComponent.Transform.position, Time.deltaTime * _dependenciesContainer.GameSettings.CameraMoveSpeed);
            }
            
        }
    }
}