using Leopotam.EcsLite;
using Shared;
using UnityEngine;

namespace Client
{
    public class PlayerViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            var filter = _world.Filter<Player>().End();
            var positionViews = _world.GetPool<PositionViewReference>();
            var playerViews = _world.GetPool<PlayerViewReference>();

            var player = GameObject.FindObjectOfType<PlayerView>();
            foreach (var entity in filter)
            {
                positionViews.Add(entity);

                ref var position = ref positionViews.Get(entity);
                position.transform = player.transform;

                ref var animation = ref playerViews.Add(entity);
                animation.PlayerView = player.GetComponent<PlayerView>();
            }
        }

        public void Run(IEcsSystems systems)
        {
            var filter = _world.Filter<Player>().Inc<StateChanged>().End();

            var playersPool = _world.GetPool<Player>();
            var playerViews = _world.GetPool<PlayerViewReference>();

            foreach (var entity in filter)
            {
                var player = playersPool.Get(entity);
                var view = playerViews.Get(entity);

                view.PlayerView.SetState(player.state);
            }
        }
    }
}

