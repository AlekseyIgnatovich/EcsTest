using System.Linq;
using UnityEngine;
using Leopotam.EcsLite;
using Shared;

namespace Client
{
    public class DoorsViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _pressedFilter;

        private EcsPool<Door> _doors;
        private EcsPool<DoorViewReference> _doorReferences;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _pressedFilter = world.Filter<Pressed>().End();
            _doors = world.GetPool<Door>();
            _doorReferences = world.GetPool<DoorViewReference>();

            var doorsFilter = world.Filter<Door>().End();

            var buttons = world.GetPool<Button>();
            var doorReferences = world.GetPool<DoorViewReference>();

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
            foreach (var entity in _pressedFilter)
            {
                var door = _doors.Get(entity);
                var view = _doorReferences.Get(entity).doorView;

                view.UpdateProgress(door.openProgress);
            }
        }
    }
}