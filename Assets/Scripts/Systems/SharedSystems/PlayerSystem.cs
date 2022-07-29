using Leopotam.EcsLite;

namespace Shared
{
	public class PlayerSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsFilter _playerFilter;
        
		private EcsPool<Player> _players;
		private EcsPool<Movement> _movements;
		private EcsPool<StateChanged> _stateChangedEvents;
		
		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			int playerEntity = world.NewEntity();

			_playerFilter = world.Filter<Player>().End();

			_movements = world.GetPool<Movement>();
			_players = world.GetPool<Player>();
			_stateChangedEvents = world.GetPool<StateChanged>();
			
			ref var player = ref _players.Add(playerEntity);
			player.state = PlayerState.Idle;
			ref var position = ref world.GetPool<Position>().Add(playerEntity);
			position.radius = 0.5f;
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _playerFilter)
			{
				if (_stateChangedEvents.Has(entity))
				{
					_stateChangedEvents.Del(entity);
				}

				ref var player = ref _players.Get(entity);
				if (_movements.Has(entity) && player.state != PlayerState.Move)
				{
					_stateChangedEvents.Add(entity);
					player.state = PlayerState.Move;
				}

				if (!_movements.Has(entity) && player.state != PlayerState.Idle)
				{
					_stateChangedEvents.Add(entity);
					player.state = PlayerState.Idle;
				}
			}
		}
	}
}
