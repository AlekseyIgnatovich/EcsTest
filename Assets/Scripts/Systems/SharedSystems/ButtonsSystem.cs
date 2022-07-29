using Leopotam.EcsLite;

namespace Shared
{
    public class ButtonsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            var config = systems.GetShared<Config>();

            for (int i = 0; i < config.buttons.Length; i++)
            {
                int entity = _world.NewEntity();

                var buttons = _world.GetPool<Button>();
                buttons.Add(entity);
                var positions = _world.GetPool<Position>();
                positions.Add(entity);

                ref var button = ref buttons.Get(entity);
                button.configId = config.buttons[i].configId;
                ref var position = ref positions.Get(entity);
                position.position = config.buttons[i].position;
                position.radius = config.buttons[i].radius;
            }
        }

        public void Run(IEcsSystems systems)
        {
            var filterButtons = _world.Filter<Button>().End();
            var filterPlayer = _world.Filter<Player>().End();

            var positions = _world.GetPool<Position>();
            var pressed = _world.GetPool<Pressed>();

            foreach (var player in filterPlayer)
            {
                ref var playerPosition = ref positions.Get(player);
                foreach (var button in filterButtons)
                {
                    var buttonPosition = positions.Get(button);

                    var radius = buttonPosition.radius + playerPosition.radius;
                    var dist = UnityEngine.Vector3.Distance(playerPosition.position, buttonPosition.position);

                    if (dist <= radius)
                    {
                        if (!pressed.Has(button))
                            pressed.Add(button);
                    }
                    else
                    {
                        if (pressed.Has(button))
                            pressed.Del(button);
                    }
                }
            }
        }
    }
}