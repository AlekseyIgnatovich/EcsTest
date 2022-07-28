using Leopotam.EcsLite;
using UnityEngine;

public class PlayerViewSystem : IEcsInitSystem
{
    private const string PlayerTag = "Player";
    
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        
        var filter = world.Filter<Player>().End();
        var viewReferences = world.GetPool<PositionViewReference>();

        var playerView = GameObject.FindGameObjectWithTag(PlayerTag);
        foreach (var entity in filter)
        {
            viewReferences.Add(entity);
            ref var reference = ref viewReferences.Get(entity);

            reference.transform = playerView.transform;
        }
    }
}

