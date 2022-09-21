using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class AiFightState : AiBaseState
    {
        private Ai _ai;
        private Ai _targetAi;
        private float _fightTimer;

        public override void EnterState(AiStateManager aiStateManager)
        {
            if (_ai == null)
                _ai = aiStateManager.Ai;

            _fightTimer = DanceFloor.FightDuration;
            _ai.OnStopArguing?.Invoke();

            if (_ai.StateManager.CurrentStateType == Enums.AiStateType.Attack)
            {
                _ai.OnStartFighting?.Invoke();
                _targetAi = DanceFloor.DefenderAi;

                ClubEvents.OnEveryoneGetAngrier?.Invoke();
            }
            else if (_ai.StateManager.CurrentStateType == Enums.AiStateType.Defend)
            {
                _targetAi = DanceFloor.AttackerAi;
            }
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            if (_ai.StateManager.CurrentStateType == Enums.AiStateType.Attack)
            {
                _fightTimer -= Time.deltaTime;
                if (_fightTimer <= 0f)
                {
                    _ai.OnStopFighting?.Invoke();
                    _ai.StateManager.SwitchState(_ai.StateManager.LeaveClubState);
                    _targetAi.StateManager.SwitchState(_targetAi.StateManager.GetKnockedOutState);
                    //_targetAi.StateManager.SwitchState(_targetAi.StateManager.LeaveClubState);

                    ClubEvents.OnEveryoneGetAngrier?.Invoke();
                    ClubEvents.OnAFightEnded?.Invoke();
                }
            }
        }
    }
}
