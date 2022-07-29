using Leopotam.EcsLite;

namespace Shared
{
    public class MovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float Speed = 2f;

        private ITimeProvider _timeProvider;
        
        private EcsFilter _movementsFilter;
        
        private EcsPool<Movement> _movements;
        private EcsPool<Position> _position;

        public MovementSystem(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _movementsFilter = world.Filter<Movement>().End();
            _movements = world.GetPool<Movement>();
            _position = world.GetPool<Position>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _movementsFilter)
            {
                ref var position = ref _position.Get(entity);
                var movement = _movements.Get(entity);

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
                    _movements.Del(entity);
                }
            }
        }
    }
}