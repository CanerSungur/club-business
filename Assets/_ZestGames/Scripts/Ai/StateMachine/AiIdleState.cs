using ClubBusiness;
using UnityEngine;
using ZestCore.Utility;

namespace ZestGames
{
    public class AiIdleState : AiBaseState
    {
        private Ai _ai;

        private bool _canMakeADecision = false;
        private readonly float _decisionDelay = 3f;
        private float _timer;

        public override void EnterState(AiStateManager aiStateManager)
        {
            //Debug.Log("Entered idle state");
            aiStateManager.SwitchStateType(Enums.AiStateType.Idle);

            if (_ai == null)
                _ai = aiStateManager.Ai;

            _canMakeADecision = false;
            _timer = _decisionDelay;
            _ai.OnIdle?.Invoke();
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            if (GameManager.GameState != Enums.GameState.Started) return;

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = _decisionDelay;
                _canMakeADecision = true;
                MakeDecision(aiStateManager);
            }
        }

        private void MakeDecision(AiStateManager aiStateManager)
        {
            if (_canMakeADecision)
            {
                if (QueueManager.ExampleQueue.QueueIsFull || !_ai.CanGetIntoQueue)
                {
                    //aiStateManager.SwitchState(aiStateManager.WanderState);
                    if (ClubManager.DanceFloorHasCapacity)
                    {
                        aiStateManager.SwitchState(aiStateManager.DanceState);
                    }
                    else
                    {
                        if (RNG.RollDice(60))
                            aiStateManager.SwitchState(aiStateManager.WanderState);
                        else
                            aiStateManager.SwitchState(aiStateManager.IdleState);
                    }
                }
                else
                {
                    // we can get into queue
                    aiStateManager.SwitchState(aiStateManager.GetIntoClubState);
                    _ai.CanGetIntoQueue = false;
                }
            }
        }
    }
}
