using Leopotam.EcsLite;

namespace Shared
{
    public class ButtonsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _buttonsFilter;
        private EcsFilter _playersFilter;
        
        private EcsPool<Button> _buttons;
        private EcsPool<TransformCmp> _transforms;
        private EcsPool<Pressed> _pressed;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _buttonsFilter = world.Filter<Button>().End();
            _playersFilter = world.Filter<Player>().End();
            
            _buttons = world.GetPool<Button>();
            _transforms = world.GetPool<TransformCmp>();
            _pressed = world.GetPool<Pressed>();
            
            var config = systems.GetShared<Config>();

            for (int i = 0; i < config.buttons.Length; i++)
            {
                int entity = world.NewEntity();

                _buttons.Add(entity);
                _transforms.Add(entity);

                ref var button = ref _buttons.Get(entity);
                button.configId = config.buttons[i].configId;
                ref var position = ref _transforms.Get(entity);
                position.position = config.buttons[i].position;
                position.radius = config.buttons[i].radius;
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var player in _playersFilter)
            {
                ref var playerPosition = ref _transforms.Get(player);
                foreach (var button in _buttonsFilter)
                {
                    var buttonPosition = _transforms.Get(button);

                    var radius = buttonPosition.radius + playerPosition.radius;
                    var dist = UnityEngine.Vector3.Distance(playerPosition.position, buttonPosition.position);

                    if (dist <= radius)
                    {
                        if (!_pressed.Has(button))
                            _pressed.Add(button);
                    }
                    else
                    {
                        if (_pressed.Has(button))
                            _pressed.Del(button);
                    }
                }
            }
        }
    }
}