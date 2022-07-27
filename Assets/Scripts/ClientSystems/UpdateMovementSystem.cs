using Leopotam.EcsLite;

public class UpdateViewMovementSystem  : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var filter = systems.GetWorld().Filter<Position>().Inc<PositionViewReference>().End();
        var movements = systems.GetWorld().GetPool<Position>();
        var views = systems.GetWorld().GetPool<PositionViewReference>();

        foreach (var entity in filter)
        {
            ref var movement = ref movements.Get(entity);
            ref var view = ref views.Get(entity);

            if (view.transform != null)
            {
                view.transform.position = movement.position;
            }
        }
    }
}