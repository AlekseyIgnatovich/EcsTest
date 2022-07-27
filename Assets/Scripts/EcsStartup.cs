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
            
            
            _systems = new EcsSystems (_world);
            _systems
                // register your systems here, for example:
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())
                
                //SharedSystems
                .Add(new MovementSystem())
                //
                
                //ClientSystems
                .Add(new PlayerInputSystem())
                .Add(new IniSceneSystem())
                .Add(new UpdateViewMovementSystem())
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