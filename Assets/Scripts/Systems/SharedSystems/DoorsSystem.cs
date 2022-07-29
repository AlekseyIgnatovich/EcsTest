using Leopotam.EcsLite;

namespace Shared
{
    public class DoorsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private ITimeProvider _timeProvider;

        private EcsFilter _buttonsFilter;
        private EcsFilter _pressedFilter;
        
        private EcsPool<Door> _doors;
        private EcsPool<Pressed> _pressed;

        public DoorsSystem(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            _buttonsFilter = world.Filter<Button>().End();
            _pressedFilter = world.Filter<Pressed>().End();
                
            _doors = world.GetPool<Door>();

            foreach (var entity in _buttonsFilter)
            {
                _doors.Add(entity);
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _pressedFilter)
            {
                ref var door = ref _doors.Get(entity);
                door.openProgress += 0.1f * _timeProvider.DeltaTime;
                if (door.openProgress > 1)
                    door.openProgress = 1;
            }
        }
    }
}
