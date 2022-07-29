using Leopotam.EcsLite;
using Shared;

namespace Client
{
    public class MovementViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _positionsFilter;
        private EcsPool<Position> _positions;
        private EcsPool<PositionViewReference> _playerViews;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _positionsFilter = world.Filter<Position>().Inc<PositionViewReference>().End();
            _positions = world.GetPool<Position>();
            _playerViews = world.GetPool<PositionViewReference>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _positionsFilter)
            {
                ref var movement = ref _positions.Get(entity);
                ref var view = ref _playerViews.Get(entity);

                if (view.transform != null)
                    view.transform.position = movement.position;
            }
        }
    }
}