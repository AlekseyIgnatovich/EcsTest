using Leopotam.EcsLite;
using UnityEngine;

namespace Client {
    sealed class EcsStartup : MonoBehaviour {
        EcsWorld _world;        
        IEcsSystems _systems;

        void Start () {
            _world = new EcsWorld ();
            
           
            // Todo: отдельная система на создание игрока
            int player = _world.NewEntity ();
            var playerPool = _world.GetPool<Player>();
            playerPool.Add (player);
            var movementPool = _world.GetPool<Movement>();
            movementPool.Add (player);
            var positionPool = _world.GetPool<Position>();
            positionPool.Add (player);
            ref var pos = ref positionPool.Get(player);
            pos.radius = 0.5f;
            
            Config sharedData = new Config();
            _systems = new EcsSystems (_world, sharedData);
            _systems
                // register your systems here, for example:
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())
                
                //SharedSystems
                .Add(new ButtonsSystem())
                .Add(new DoorsSystem())
                .Add(new MovementSystem())
                //
                
                //ClientSystems
                .Add(new PlayerInputSystem())
                .Add(new PlayerInitViewSystem())
                .Add(new MovementUpdateViewSystem())
                .Add(new DoorsViewSystem())
                //.Add()
                //
                
                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
#endif
                .Init ();
        }

        void Update () {
            // process systems here.
            _systems?.Run ();
        }

        void OnDestroy () {
            if (_systems != null) {
                // list of custom worlds will be cleared
                // during IEcsSystems.Destroy(). so, you
                // need to save it here if you need.
                _systems.Destroy ();
                _systems = null;
            }
            
            // cleanup custom worlds here.
            
            // cleanup default world.
            if (_world != null) {
                _world.Destroy ();
                _world = null;
            }
        }
    }
}