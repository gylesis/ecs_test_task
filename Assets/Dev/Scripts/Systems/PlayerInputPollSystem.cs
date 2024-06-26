using GoodCat.EcsLite.Shared;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Systems
{
    public class PlayerInputPollSystem : IEcsRunSystem
    {
        [EcsInject] private DependenciesContainer _dependenciesContainer;
        
        public void Run(IEcsSystems systems)
        {
            Joystick movementJoystick = _dependenciesContainer.JoysticksContainer.MovementJoystick;

            float horizontal = movementJoystick.Horizontal;
            float vertical = movementJoystick.Vertical;

            if (vertical == 0)
            {
                vertical = Input.GetAxis("Vertical");
            }

            if (horizontal == 0)
            {
                horizontal = Input.GetAxis("Horizontal");
            }
            
            Vector3 inputVector = new Vector3(horizontal, 0, vertical);

            
            EcsWorld world = systems.GetWorld();

            var playerFilter = world.Filter<TagPlayerComponent>().End();

            var inputs = world.GetPool<PlayerInputComponent>();

            foreach (var index in playerFilter)
            {
                ref var input = ref inputs.Add(index);

                input.MoveDirection = inputVector;
            }

        }
    }


    //public class 
}