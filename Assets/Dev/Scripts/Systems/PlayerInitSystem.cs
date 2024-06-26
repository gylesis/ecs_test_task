using System.Collections.Generic;
using GoodCat.EcsLite.Shared;
using Leopotam.EcsLite;

namespace Client.Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        [EcsInject] private DependenciesContainer _dependenciesContainer;
        
        public void Init(IEcsSystems systems)
        {
            Player player = _dependenciesContainer.SpawnedPlayer;

            EcsWorld world = systems.GetWorld();
            
            var transformsPool = world.GetPool<TransformRefComponent>();
            var tagBagComponentPool = world.GetPool<BagComponent>();
            var playerTagsPool = world.GetPool<TagPlayerComponent>();
            var bagOwnersPool = world.GetPool<BagOwnerComponent>();
            var players = world.GetPool<PlayerComponent>();

            int playerEntity = world.NewEntity();
            ref var playerTransformComponent = ref transformsPool.Add(playerEntity);
            playerTransformComponent.Transform = player.transform;
            playerTagsPool.Add(playerEntity);
            player.EntityId = playerEntity;
            ref PlayerComponent playerComponent = ref players.Add(playerEntity);
            playerComponent.Player = player;
            
            int playerBagEntity = world.NewEntity();
            ref var bagComponent = ref tagBagComponentPool.Add(playerBagEntity);
            bagComponent.OwnerEntity = playerEntity;
            
            ref var bagTransformComponent = ref transformsPool.Add(playerBagEntity);
            bagTransformComponent.Transform = player.BagTransform;

            ref var bagOwnerComponent = ref bagOwnersPool.Add(playerEntity);
            bagOwnerComponent.BagEntity = playerEntity;

            _dependenciesContainer.GameState.SpawnedStackObjectsInBag[playerBagEntity] = new List<ObjectStack>();
            
            _dependenciesContainer.GameState.OwnersToBags.Add(playerEntity, playerBagEntity);
        }
    }
}