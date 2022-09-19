using UnityEngine;
using ZestGames;
using ZestCore.Ai;

namespace ClubBusiness
{
    public class AiLeaveClubState : AiBaseState
    {
        private Ai _ai;
        private Transform _target;
        private int _waypointIndex;
        private bool _reached, _isMoving;

        public override void EnterState(AiStateManager aiStateManager)
        {
            if (_ai == null)
                _ai = aiStateManager.Ai;

            _ai.AnimationController.Animator.Rebind();

            if (_ai.StateManager.CurrentStateType ==  Enums.AiStateType.BuyDrink)
            {
                QueueManager.BarQueue.RemoveAiFromQueue(_ai);
                _ai.StateManager.BuyDrinkState.CurrentQueuePoint.QueueIsReleased();
                _ai.Rigidbody.isKinematic = false;
            }
            else if (_ai.StateManager.CurrentStateType == Enums.AiStateType.GetIntoToiletQueue)
            {
                QueueManager.ToiletQueue.RemoveAiFromQueue(_ai);
                _ai.StateManager.GetIntoToiletQueueState.CurrentQueuePoint.QueueIsReleased();
            }

            aiStateManager.SwitchStateType(Enums.AiStateType.Leaving);
            _reached = false;
            _waypointIndex = 0;
            _target = ClubManager.ExitTransforms[_waypointIndex];

            _ai.OnIdle?.Invoke();
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            Debug.Log("LEAVE CLUB STATE");

            if (!_reached)
            {
                if (Operation.IsTargetReached(_ai.transform, _target.position, 0.05f))
                {
                    if (_waypointIndex == ClubManager.ExitTransforms.Length - 1)
                    {
                        _reached = true;
                        _ai.LeftClub();
                    }
                    else
                    {
                        _waypointIndex++;
                        _target = ClubManager.ExitTransforms[_waypointIndex];
                    }
                }
                else
                {
                    if (!_isMoving)
                    {
                        _isMoving = true;
                        _ai.OnMove?.Invoke();
                    }
                    Debug.Log("Moving");
                    Navigation.MoveTransform(_ai.transform, _target.position, _ai.RunSpeed);
                    Navigation.LookAtTarget(_ai.transform, _target.position, 10f);
                }
            }
        }
    }
}
