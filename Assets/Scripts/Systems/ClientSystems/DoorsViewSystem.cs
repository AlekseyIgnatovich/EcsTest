using System.Linq;
using UnityEngine;
using Leopotam.EcsLite;
using Shared;

namespace Client
{
    public class DoorsViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            var doorsFilter = _world.Filter<Door>().End();

            var buttons = _world.GetPool<Button>();
            var doorReferences = _world.GetPool<DoorViewReference>();

            var doorViews = GameObject.FindObjectsOfType<DoorView>();

            foreach (var entity in doorsFilter)
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
            var pressedFilter = _world.Filter<Pressed>().End();

            var doors = _world.GetPool<Door>();
            var doorReferences = _world.GetPool<DoorViewReference>();

            foreach (var entity in pressedFilter)
            {
                var door = doors.Get(entity);
                var view = doorReferences.Get(entity).doorView;

                view.UpdateProgress(door.openProgress);
            }
        }
    }
}