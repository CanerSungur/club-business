using UnityEngine;
using ZestGames;
using ZestCore.Utility;

namespace ClubBusiness
{
    public class AiGetKnockedOutState : AiBaseState
    {
        private Ai _ai;
        private float _knockedOutTimer;
        private bool _wokeUp;

        public override void EnterState(AiStateManager aiStateManager)
        {
            if (_ai == null)
                _ai = aiStateManager.Ai;

            _ai.OnGetKnockedOut?.Invoke();
            _knockedOutTimer = 10f;
            _wokeUp = false;
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            if (!_wokeUp)
            {
                _knockedOutTimer -= Time.deltaTime;
                if (_knockedOutTimer <= 0f)
                {
                    _wokeUp = true;
                    _ai.OnGetUp?.Invoke();
                    LeaveAfterGetUp();
                }
            }
        }

        private void LeaveAfterGetUp()
        {
            Delayer.DoActionAfterDelay(_ai, 12f, () => _ai.StateManager.SwitchState(_ai.StateManager.LeaveClubState));
        }
    }
}
