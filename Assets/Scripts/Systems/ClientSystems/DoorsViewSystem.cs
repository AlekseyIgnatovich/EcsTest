using System.Linq;
using UnityEngine;
using Leopotam.EcsLite;
using Shared;

namespace Client
{
    public class DoorsViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        public void Init(IEcsSystems systems)
        {
            var doors = systems.GetWorld().Filter<Door>().End();

            var buttons = systems.GetWorld().GetPool<Button>();
            var doorReferences = systems.GetWorld().GetPool<DoorViewReference>();

            var doorViews = GameObject.FindObjectsOfType<DoorView>();

            foreach (var entity in doors)
            {
                string configId = buttons.Get(entity).configId;
                var view = doorViews.FirstOrDefault(d => d.buttonConfigId == configId);

                if (view == null)
                {
                    Debug.LogError($"Can't found view for door: {configId}");
                    continue;
                }

                doorReferences.Add(entity);
                ref var reference = ref doorReferences.Get(entity);
                reference.doorView = view;
            }
        }

        public void Run(IEcsSystems systems)
        {
            var doorEntities = systems.GetWorld().Filter<Pressed>().End();

            var doors = systems.GetWorld().GetPool<Door>();
            var doorReferences = systems.GetWorld().GetPool<DoorViewReference>();

            foreach (var entity in doorEntities)
            {
                var door = doors.Get(entity);
                var view = doorReferences.Get(entity).doorView;

                view.UpdateProgress(door.openProgress);
            }
        }
    }
}