using UnityEngine;
using ZestGames;
using ZestCore.Ai;

namespace ClubBusiness
{
    public class AiLeaveClubState : AiBaseState
    {
        private Ai _ai;
        private Vector3 _exitPosition;
        private bool _reached;

        public override void EnterState(AiStateManager aiStateManager)
        {
            if (_ai == null)
                _ai = aiStateManager.Ai;

            if (_ai.StateManager.CurrentStateType ==  Enums.AiStateType.BuyDrink)
            {
                QueueManager.BarQueue.RemoveAiFromQueue(_ai);
                _ai.StateManager.BuyDrinkState.CurrentQueuePoint.QueueIsReleased();
            }
            else if (_ai.StateManager.CurrentStateType == Enums.AiStateType.GetIntoToiletQueue)
            {
                QueueManager.ToiletQueue.RemoveAiFromQueue(_ai);
                _ai.StateManager.GetIntoToiletQueueState.CurrentQueuePoint.QueueIsReleased();
            }
            

            aiStateManager.SwitchStateType(Enums.AiStateType.Leaving);
            _reached = false;
            _exitPosition = ClubManager.ExitTransform.position;

            


            _ai.OnMove?.Invoke();
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            if (!_reached)
            {
                if (Operation.IsTargetReached(_ai.transform, _exitPosition, 1f))
                {
                    _reached = true;
                    _ai.LeftClub();
                }
                else
                {
                    Navigation.MoveTransform(_ai.transform, _exitPosition, _ai.RunSpeed);
                    Navigation.LookAtTarget(_ai.transform, _exitPosition);
                }
            }
        }
    }
}
