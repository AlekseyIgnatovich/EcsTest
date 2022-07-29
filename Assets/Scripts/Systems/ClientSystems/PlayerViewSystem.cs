using Leopotam.EcsLite;
using Shared;
using UnityEngine;

namespace Client
{
    public class PlayerViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _playerFilter;
        private EcsFilter _changedFilter;
        
        private EcsPool<PositionViewReference> _positionViews;
        private EcsPool<PlayerViewReference> _playerViews;
        private EcsPool<Player> _players;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _playerFilter = world.Filter<Player>().End();
            _changedFilter = world.Filter<StateChanged>().Inc<Player>().End();

            _positionViews = world.GetPool<PositionViewReference>();
            _playerViews = world.GetPool<PlayerViewReference>();
            _players = world.GetPool<Player>();

            var player = GameObject.FindObjectOfType<PlayerView>();
            foreach (var entity in _playerFilter)
            {
                _positionViews.Add(entity);

                ref var position = ref _positionViews.Get(entity);
                position.transform = player.transform;

                ref var animation = ref _playerViews.Add(entity);
                animation.PlayerView = player.GetComponent<PlayerView>();
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _changedFilter)
            {
                var player = _players.Get(entity);
                var view = _playerViews.Get(entity);

                view.PlayerView.SetState(player.state);
            }
        }
    }
}

