using Shared;
using UnityEngine;

namespace Client
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : MonoBehaviour
    {
        private Animator _animator;

        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetState(PlayerState type)
        {
            _animator.SetTrigger(type.ToString());
        }
    }
}
