using Leopotam.EcsLite;
using Shared;
using UnityEngine;

namespace Client
{
	public class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly LayerMask _inputMask = 1 << LayerMask.NameToLayer("Ground");

		private Camera _camera;

		private EcsFilter _playerFilter;
		private EcsPool<Movement> _movements;
		
		public void Init(IEcsSystems systems)
		{
			_camera = Camera.main;
			
			var world = systems.GetWorld();
			_playerFilter = world.Filter<Player>().End();
			_movements = world.GetPool<Movement>();
		}

		public void Run(IEcsSystems systems)
		{
			if (!Input.GetMouseButtonDown(0)) return;

			RaycastHit hit;
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, 100, _inputMask))
			{
				foreach (var entity in _playerFilter)
				{
					if (!_movements.Has(entity))
						_movements.Add(entity);

					ref var movement = ref _movements.Get(entity);
					movement.targetPosition = new Vector3(hit.point.x, 0, hit.point.z);
				}
			}
		}
	}
}