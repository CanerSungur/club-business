using UnityEngine;
using ZestCore.Ai;
using ZestGames;

namespace ClubBusiness
{
    public class AiEnterToiletState : AiBaseState
    {
        private Ai _ai;
        private Transform _target = null;
        private int _waypointIndex;
        private bool _enteredToilet, _isMoving;

        public override void EnterState(AiStateManager aiStateManager)
        {
            if (_ai == null)
                _ai = aiStateManager.Ai;

            _enteredToilet = _isMoving = false;
            _waypointIndex = 0;
            _target = CustomerWaypoints.Waypoints[_waypointIndex];
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            Debug.Log("ENTER TOILET STATE");

            if (!_enteredToilet)
            {
                if (Operation.IsTargetReached(_ai.transform, _target.position, 0.05f))
                {
                    if (_waypointIndex == CustomerWaypoints.Waypoints.Length - 1)
                    {
                        _enteredToilet = true;
                        aiStateManager.SwitchState(aiStateManager.GoToToiletState);
                    }
                    else
                    {
                        _waypointIndex++;
                        _target = CustomerWaypoints.Waypoints[_waypointIndex];
                    }
                }
                else
                {
                    Navigation.MoveTransform(_ai.transform, _target.position, _ai.RunSpeed);
                    Navigation.LookAtTarget(_ai.transform, _target.position, 10f);

                    if (!_isMoving)
                    {
                        _isMoving = true;
                        _ai.OnMove?.Invoke();
                    }
                }
            }
        }
    }
}
