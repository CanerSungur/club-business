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

                if (_ai.CurrentLocation == Enums.AiLocation.Inside)
                    MakeDecision(aiStateManager);
                else
                    aiStateManager.SwitchState(aiStateManager.GetIntoClubState);
            }
        }

        // when inside, decide to drink or dance. 40-40. 10 wander 10 idle
        // if ai chooses to drink, then wander then toilet.

        // if ai chooses to dance, then wander then drink.

        private void MakeDecision(AiStateManager aiStateManager)
        {
            if (_canMakeADecision)
            {

                if (_ai.NeedDrink)
                    aiStateManager.SwitchState(aiStateManager.BuyDrinkState);
                else if (_ai.NeedDancing)
                    aiStateManager.SwitchState(aiStateManager.DanceState);
                else if (_ai.NeedToPiss)
                {
                    if (ClubManager.ToiletIsAvailable)
                        aiStateManager.SwitchState(aiStateManager.EnterToiletState);
                    else
                        aiStateManager.SwitchState(aiStateManager.GetIntoToiletQueueState);
                }
                else
                {
                    if (RNG.RollDice(10))
                        aiStateManager.SwitchState(aiStateManager.IdleState);
                    else
                    {
                        if (RNG.RollDice(10))
                            aiStateManager.SwitchState(aiStateManager.WanderState);
                        else
                        {
                            if (RNG.RollDice(50))
                                aiStateManager.SwitchState(aiStateManager.DanceState);
                            else
                                aiStateManager.SwitchState(aiStateManager.BuyDrinkState);
                        }
                    }
                }


                //    if (!ClubManager.ToiletIsAvailable)
                //    {
                //        if (QueueManager.ToiletQueue.QueueIsFull)
                //        {
                //            if (QueueManager.BarQueue.QueueIsFull)
                //            {
                //                //aiStateManager.SwitchState(aiStateManager.WanderState);
                //                if (ClubManager.DanceFloorHasCapacity)
                //                {
                //                    aiStateManager.SwitchState(aiStateManager.DanceState);
                //                }
                //                else
                //                {
                //                    if (RNG.RollDice(60))
                //                        aiStateManager.SwitchState(aiStateManager.WanderState);
                //                    else
                //                        aiStateManager.SwitchState(aiStateManager.IdleState);
                //                }
                //            }
                //            else
                //            {
                //                // we can get into queue
                //                aiStateManager.SwitchState(aiStateManager.BuyDrinkState);
                //            }
                //        }
                //        else
                //        {
                //            aiStateManager.SwitchState(aiStateManager.GetIntoToiletQueueState);
                //        }
                //    }
                //    else
                //    {
                //        aiStateManager.SwitchState(aiStateManager.GoToToiletState);
                //    }
            }
        }
    }
}
