using Leopotam.EcsLite;
using Shared;
using UnityEngine;

namespace Client {
    sealed class EcsStartup : MonoBehaviour {
        EcsWorld _world;        
        IEcsSystems _systems;

        void Start () {
            _world = new EcsWorld ();
            
            Config sharedData = new Config();
            _systems = new EcsSystems (_world, sharedData);
            _systems

                //SharedSystems
                .Add(new PlayerSystem())
                .Add(new ButtonsSystem())
                .Add(new DoorsSystem())
                .Add(new MovementSystem())

                //ClientSystems
                .Add(new PlayerInputSystem())
                .Add(new PlayerViewSystem())
                .Add(new MovementViewSystem())
                .Add(new DoorsViewSystem())
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
#endif
                .Init ();
        }

        void Update () {
            _systems?.Run ();
        }

        void OnDestroy () {
            if (_systems != null) {
                _systems.Destroy ();
                _systems = null;
            }
            
            if (_world != null) {
                _world.Destroy ();
                _world = null;
            }
        }
    }
}