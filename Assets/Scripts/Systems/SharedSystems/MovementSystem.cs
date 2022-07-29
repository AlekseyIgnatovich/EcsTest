using Leopotam.EcsLite;

namespace Shared
{
    public class MovementSystem : IEcsRunSystem
    {
        private const float Speed = 2f;

        private ITimeProvider _timeProvider;
        private EcsWorld _ecsWorld;

        public MovementSystem(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }
        
        public void Run(IEcsSystems ecsSystems)
        {
            _ecsWorld = ecsSystems.GetWorld();

            var movementsFilter = _ecsWorld.Filter<Movement>().End();
            var movements = _ecsWorld.GetPool<Movement>();
            var positions = _ecsWorld.GetPool<Position>();

            foreach (var entity in movementsFilter)
            {
                ref var position = ref positions.Get(entity);
                var movement = movements.Get(entity);

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
                    movements.Del(entity);
                }
            }
        }
    }
}