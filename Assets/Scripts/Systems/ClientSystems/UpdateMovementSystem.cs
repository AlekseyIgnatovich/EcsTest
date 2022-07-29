using Leopotam.EcsLite;
using Shared;

namespace Client
{
    public class MovementViewSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var positionsFilter = world.Filter<Position>().Inc<PositionViewReference>().End();
            var positions = world.GetPool<Position>();
            var views = world.GetPool<PositionViewReference>();

            foreach (var entity in positionsFilter)
            {
                ref var movement = ref positions.Get(entity);
                ref var view = ref views.Get(entity);

                if (view.transform != null)
                    view.transform.position = movement.position;
            }
        }
    }
}