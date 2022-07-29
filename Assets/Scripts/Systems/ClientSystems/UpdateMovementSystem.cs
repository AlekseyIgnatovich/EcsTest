using Leopotam.EcsLite;
using Shared;

namespace Client
{
    public class MovementViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _transformsFilter;
        private EcsPool<TransformCmp> _transforms;
        private EcsPool<PositionViewReference> _playerViews;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _transformsFilter = world.Filter<TransformCmp>().Inc<PositionViewReference>().End();
            _transforms = world.GetPool<TransformCmp>();
            _playerViews = world.GetPool<PositionViewReference>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _transformsFilter)
            {
                ref var movement = ref _transforms.Get(entity);
                ref var view = ref _playerViews.Get(entity);

                if (view.transform != null)
                    view.transform.position = movement.position;
            }
        }
    }
}