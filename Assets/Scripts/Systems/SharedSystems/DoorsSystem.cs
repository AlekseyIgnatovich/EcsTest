using Leopotam.EcsLite;

namespace Shared
{
    public class DoorsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private ITimeProvider _timeProvider;

        public DoorsSystem(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public void Init(IEcsSystems systems)
        {
            var filterButtons = systems.GetWorld().Filter<Button>().End();
            var doors = systems.GetWorld().GetPool<Door>();

            foreach (var button in filterButtons)
            {
                doors.Add(button);
            }
        }

        public void Run(IEcsSystems systems)
        {
            var filterButtons = systems.GetWorld().Filter<Pressed>().End();
            var doors = systems.GetWorld().GetPool<Door>();

            foreach (var entity in filterButtons)
            {
                ref var door = ref doors.Get(entity);
                door.openProgress += 0.1f * _timeProvider.DeltaTime;
                if (door.openProgress > 1)
                    door.openProgress = 1;
            }
        }
    }
}
