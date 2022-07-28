using Leopotam.EcsLite;

public class PlayerSystem : IEcsInitSystem
{
	public void Init(IEcsSystems systems)
	{
		var world = systems.GetWorld();
		int player = world.NewEntity();

		world.GetPool<Player>().Add(player);
		world.GetPool<Movement>().Add(player);
		ref var position = ref world.GetPool<Position>().Add(player);
		position.radius = 0.5f;
	}
}
