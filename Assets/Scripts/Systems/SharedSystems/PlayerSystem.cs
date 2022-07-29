using Leopotam.EcsLite;

namespace Shared
{
	public class PlayerSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;

		public void Init(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			int playerEntity = _world.NewEntity();

			ref var player = ref _world.GetPool<Player>().Add(playerEntity);
			player.state = PlayerState.Idle;
			ref var position = ref _world.GetPool<Position>().Add(playerEntity);
			position.radius = 0.5f;
		}

		public void Run(IEcsSystems systems)
		{
			var filter = _world.Filter<Player>().End();
			var movements = _world.GetPool<Movement>();
			var players = _world.GetPool<Player>();
			var stateChangedPool = _world.GetPool<StateChanged>();

			foreach (var entity in filter)
			{
				if (stateChangedPool.Has(entity))
				{
					stateChangedPool.Del(entity);
				}

				ref var player = ref players.Get(entity);
				if (movements.Has(entity) && player.state != PlayerState.Move)
				{
					stateChangedPool.Add(entity);
					player.state = PlayerState.Move;
				}

				if (!movements.Has(entity) && player.state != PlayerState.Idle)
				{
					stateChangedPool.Add(entity);
					player.state = PlayerState.Idle;
				}
			}
		}
	}
}
