using Leopotam.EcsLite;

namespace Shared
{
    public class DoorsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private ITimeProvider _timeProvider;
        private EcsWorld _world;

        public DoorsSystem(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            var filterButtons = _world.Filter<Button>().End();
            var doors = _world.GetPool<Door>();

            foreach (var entity in filterButtons)
            {
                doors.Add(entity);
            }
        }

        public void Run(IEcsSystems systems)
        {
            var filterPressed = _world.Filter<Pressed>().End();
            var doors = _world.GetPool<Door>();

            foreach (var entity in filterPressed)
            {
                ref var door = ref doors.Get(entity);
                door.openProgress += 0.1f * _timeProvider.DeltaTime;
                if (door.openProgress > 1)
                    door.openProgress = 1;
            }
        }
    }
}
