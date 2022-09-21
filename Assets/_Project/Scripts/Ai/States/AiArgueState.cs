using ZestGames;
using ZestCore.Ai;
using UnityEngine;

namespace ClubBusiness
{
    public class AiArgueState : AiBaseState
    {
        private Ai _ai;
        private Ai _targetAi;
        private bool _shouldRotate;
        private float _argueTimer;

        public override void EnterState(AiStateManager aiStateManager)
        {
            if (_ai == null)
                _ai = aiStateManager.Ai;

            if (_ai.StateManager.CurrentStateType == Enums.AiStateType.Attack)
            {
                _shouldRotate = false;
                _targetAi = DanceFloor.DefenderAi;
            }
            else
            {
                _ai.StateManager.SwitchStateType(Enums.AiStateType.Defend);
                _targetAi = DanceFloor.AttackerAi;
                _shouldRotate = true;
            }

            _ai.OnStartArguing?.Invoke(_ai.StateManager.CurrentStateType);
            _argueTimer = DanceFloor.ArgueDuration;
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            if (_shouldRotate)
            {
                Navigation.LookAtTarget(_ai.transform, _targetAi.transform.position);
            }

            if (_ai.StateManager.CurrentStateType == Enums.AiStateType.Attack)
            {
                _argueTimer -= Time.deltaTime;
                if (_argueTimer <= 0f)
                {
                    _ai.StateManager.SwitchState(_ai.StateManager.FightState);
                    _targetAi.StateManager.SwitchState(_targetAi.StateManager.FightState);
                }
            }
        }
    }
}
