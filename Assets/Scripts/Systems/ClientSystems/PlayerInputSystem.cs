using Leopotam.EcsLite;
using Shared;
using UnityEngine;

namespace Client
{
	public class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly LayerMask _inputMask = ~LayerMask.NameToLayer("Ground");

		private EcsWorld _world;
		private Camera _camera;

		public void Init(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_camera = Camera.main;
		}

		public void Run(IEcsSystems systems)
		{
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit hit;
				Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit, 100, _inputMask))
				{
					var filter = _world.Filter<Player>().End();
					var movements = _world.GetPool<Movement>();

					foreach (var entity in filter)
					{
						if (!movements.Has(entity))
						{
							movements.Add(entity);
						}

						ref var movement = ref movements.Get(entity);
						movement.targetPosition = new Vector3(hit.point.x, 0, hit.point.z);
					}
				}
			}
		}
	}
}