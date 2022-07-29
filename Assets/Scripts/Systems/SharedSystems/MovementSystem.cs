using Leopotam.EcsLite;

namespace Shared
{
    public class MovementSystem : IEcsRunSystem
    {
        private const float Speed = 2f;

        private ITimeProvider _timeProvider;

        public MovementSystem(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }
        
        public void Run(IEcsSystems ecsSystems)
        {
            EcsWorld world = ecsSystems.GetWorld();

            var filter = world.Filter<Movement>().End();
            var movementsPool = world.GetPool<Movement>();
            var positionsPool = world.GetPool<Position>();

            foreach (var entity in filter)
            {
                ref var position = ref positionsPool.Get(entity);
                var movement = movementsPool.Get(entity);

                var distanceVector = movement.targetPosition - position.position;
                var direction = distanceVector.normalized;
                var step = Speed * _timeProvider.DeltaTime;

                if (distanceVector.magnitude >= step)
                {
                    position.position += direction * (Speed * _timeProvider.DeltaTime);
                }
                else
                {
                    position.position = movement.targetPosition;
                    movementsPool.Del(entity);
                }
            }
        }
    }
}