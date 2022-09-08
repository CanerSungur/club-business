using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class QueueSystemAnimationController : MonoBehaviour
    {
        private QueueSystem _queueSystem;
        private Animator _animator;
        private readonly int _enableID = Animator.StringToHash("Enable");

        public void Init(QueueSystem queueSystem)
        {
            if (_animator == null)
            {
                _queueSystem = queueSystem;
                _animator = GetComponent<Animator>();
            }

            _queueSystem.OnPlayerEntered += Enable;
            _queueSystem.OnPlayerExited += Disable;
        }

        private void OnDisable()
        {
            if (_queueSystem == null) return;

            _queueSystem.OnPlayerEntered -= Enable;
            _queueSystem.OnPlayerExited -= Disable;
        }

        private void Enable() => _animator.SetBool(_enableID, true);
        private void Disable() => _animator.SetBool(_enableID, false);
    }
}
