using Client.Systems;
using Dev.Systems;
using GoodCat.EcsLite.Shared;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client
{
    sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private DependenciesContainer dependenciesContainer;
        
        private EcsWorld _world;
        private IEcsSystems _systems;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _systems
                .Add (new PlayerInitSystem ())
                .Add (new PlayerInputPollSystem ())
                .Add (new PlayerMovementSystem ())
                .Add (new MainCameraMovementSystem ())
                .Add (new ObjectsSpawnSystem ())
                .Add (new TriggerEventSystem ())
                .Add (new ObjectsTakeSystem ())
                .Add (new ObjectsFollowBagSystem ())
                .Add (new ObjectSpawnZonesInitSystem ())
                .Add (new ObjectDepositZonesInitSystem ())
                .Add (new ObjectsDepositSystem ())
                .Add (new StackStatDrawSystem ())
              
                    
                .InjectShared(dependenciesContainer)

                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
               
                .InitShared()
                .Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                // list of custom worlds will be cleared
                // during IEcsSystems.Destroy(). so, you
                // need to save it here if you need.
                _systems.Destroy();
                _systems = null;
            }

            // cleanup custom worlds here.

            // cleanup default world.
            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
    
}