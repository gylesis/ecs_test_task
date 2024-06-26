using System.Linq;
using GoodCat.EcsLite.Shared;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Systems
{
    public class PlayerMovementSystem : IEcsRunSystem
    {
        [EcsInject] private DependenciesContainer _dependenciesContainer;

        private static readonly int Carrying = Animator.StringToHash("Carrying");
        private static readonly int Speed = Animator.StringToHash("Speed");

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            EcsFilter playerFilter = world
                .Filter<PlayerComponent>().End();

            var inputs = world.GetPool<PlayerInputComponent>();
            var players = world.GetPool<PlayerComponent>();

            foreach (var playerIndex in playerFilter)
            {
                ref var playerComponent = ref players.Get(playerIndex);
                Player player = playerComponent.Player;

                var input = inputs.Get(playerIndex);

                
                player.transform.position += input.MoveDirection * Time.deltaTime *
                                             _dependenciesContainer.GameSettings.PlayerMoveSpeed;

                float moveDirectionSqrMagnitude = input.MoveDirection.sqrMagnitude;

                if (moveDirectionSqrMagnitude != 0)
                {
                    player.transform.rotation = Quaternion.Lerp(player.transform.rotation,
                        Quaternion.LookRotation(input.MoveDirection), Time.deltaTime * 12f);
                }

                player.Animator.SetFloat(Speed, moveDirectionSqrMagnitude);

                bool carryState;

                int bagEntity = _dependenciesContainer.GameState.OwnersToBags[player.EntityId];
                
                if (_dependenciesContainer.GameState.SpawnedStackObjectsInBag.ContainsKey(bagEntity))
                {
                    carryState = _dependenciesContainer.GameState.SpawnedStackObjectsInBag[bagEntity].Count > 0;
                }
                else
                {
                    carryState = false;
                }

                player.Animator.SetLayerWeight(1, carryState ? 1 : 0);
                player.Animator.SetBool(Carrying, carryState);

                inputs.Del(playerIndex);
            }
        }
    }

    public struct PlayerComponent
    {
        public Player Player;
    }
}