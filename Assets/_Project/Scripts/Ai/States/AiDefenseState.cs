using UnityEngine;
using ZestGames;
using ZestCore.Ai;

namespace ClubBusiness
{
    public class AiDefenseState : AiBaseState
    {
        private Ai _ai;
        private Ai _attackerAi;

        private bool _startedArguing, _startedFighting;

        public override void EnterState(AiStateManager aiStateManager)
        {
            aiStateManager.SwitchStateType(Enums.AiStateType.Defend);

            if (_ai == null)
                _ai = aiStateManager.Ai;

            if (!_startedArguing)
                _ai.OnStartArguing?.Invoke();
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            Debug.Log("DEFENSE STATE");

            Navigation.LookAtTarget(_ai.transform, _attackerAi.transform.position);
        }

        #region PUBLICS
        public void SetAttacker(Ai attackerAi)
        {
            _attackerAi = attackerAi;
        }
        public void StartFighting()
        {
            _startedFighting = true;
            _ai.OnIdle?.Invoke();
        }
        public void FinishFight()
        {
            _ai.StateManager.SwitchState(_ai.StateManager.WanderState);
        }
        #endregion
    }
}
