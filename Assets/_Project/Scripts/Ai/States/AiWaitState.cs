using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class AiWaitState : AiBaseState
    {
        private Ai _ai;
        private readonly float _waitDuration = 4f;
        private float _timer;

        private AiBaseState _attemptedState;

        public override void EnterState(AiStateManager aiStateManager)
        {
            if (_ai == null)
                _ai = aiStateManager.Ai;

            _timer = _waitDuration;
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            Debug.Log("WAIT STATE");

            //if (_ai.IsLeaving) return;
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                // try to do action again.
                aiStateManager.SwitchState(_attemptedState);
            }
        }

        public void SetAttemptedState(AiBaseState state) => _attemptedState = state;
    }
}
