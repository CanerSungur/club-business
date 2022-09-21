using ZestGames;
using ZestCore.Ai;

namespace ClubBusiness
{
    public class AiPickFightState : AiBaseState
    {
        private Ai _ai;
        private Ai _targetAi;
        private bool _reachedTarget, _isMoving;

        public override void EnterState(AiStateManager aiStateManager)
        {
            aiStateManager.SwitchStateType(Enums.AiStateType.Attack);

            if (_ai == null)
                _ai = aiStateManager.Ai;

            _targetAi = DanceFloor.DefenderAi;
            _reachedTarget = _isMoving = false;
            _ai.OnStopDancing?.Invoke();
            _ai.OnMove?.Invoke();
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            if (!_reachedTarget)
            {
                if (Operation.IsTargetReached(_ai.transform, _targetAi.transform.position, 2f))
                {
                    _reachedTarget = true;
                    _ai.OnIdle?.Invoke();

                    aiStateManager.SwitchState(aiStateManager.ArgueState);
                    _targetAi.StateManager.SwitchState(_targetAi.StateManager.ArgueState);
                }
                else
                {
                    if (!_isMoving)
                    {
                        _isMoving = true;
                        _ai.OnMove?.Invoke();
                    }

                    Navigation.MoveTransform(_ai.transform, _targetAi.transform.position, _ai.RunSpeed);
                    Navigation.LookAtTarget(_ai.transform, _targetAi.transform.position);
                }
            }
        }
    }
}
