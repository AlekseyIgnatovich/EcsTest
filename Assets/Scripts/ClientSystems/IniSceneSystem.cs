using Leopotam.EcsLite;
using UnityEngine;

public class IniSceneSystem : IEcsPreInitSystem
{
    private const string PlayerTag = "Player";
    
    public void PreInit(IEcsSystems systems)
    {
        var filter = systems.GetWorld().Filter<Player>().End();
        var viewReferences = systems.GetWorld().GetPool<PositionViewReference>();

        var playerView = GameObject.FindGameObjectWithTag(PlayerTag);
        foreach (var entity in filter)
        {
            viewReferences.Add(entity);
            ref var reference = ref viewReferences.Get(entity);

            reference.transform = playerView.transform;
        }
    }
}
