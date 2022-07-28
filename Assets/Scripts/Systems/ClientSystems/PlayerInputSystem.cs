using Leopotam.EcsLite;
using UnityEngine;

public class PlayerInputSystem : IEcsRunSystem
{
	private const string HorizontalAxisName = "Horizontal";
	private const string VerticalAxisName = "Vertical";

	public void Run(IEcsSystems systems)
	{
		var filter = systems.GetWorld().Filter<Movement>().Inc<Player>().End();
		var playerInputPool = systems.GetWorld().GetPool<Movement>();

		foreach (var entity in filter)
		{
			ref var playerInputComponent = ref playerInputPool.Get(entity);

			var horizontal = Input.GetAxis(HorizontalAxisName);
			var vertical = Input.GetAxis(VerticalAxisName);

			playerInputComponent.moveInput = new Vector3(horizontal, 0, vertical);
		}
	}
}