using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class MovementSystem : IEcsRunSystem
{
    private const float Speed = 2f;
    
    public void Run(IEcsSystems ecsSystems)
    {
        var filter = ecsSystems.GetWorld().Filter<Movement>().Inc<Position>().End();
        var movementsPool = ecsSystems.GetWorld().GetPool<Movement>();
        var positionsPool = ecsSystems.GetWorld().GetPool<Position>();

        foreach (var entity in filter)
        {
            ref var movement = ref movementsPool.Get(entity);
            ref var position = ref positionsPool.Get(entity);

            position.position += movement.moveInput * (Speed * Workaround.Deltatime());
        }
    }
}