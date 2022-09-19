using UnityEngine;
using ZestCore.Ai;
using ZestGames;

namespace ClubBusiness
{
    public class AiExitToiletState : AiBaseState
    {
        private Ai _ai;
        private bool _exitedToilet, _isMoving;
        private Transform _target = null;
        private int _waypointIndex;

        public override void EnterState(AiStateManager aiStateManager)
        {
            if (_ai == null)
                _ai = aiStateManager.Ai;

            _exitedToilet = _isMoving = false;
            _waypointIndex = CustomerWaypoints.Waypoints.Length - 1;
            _target = CustomerWaypoints.Waypoints[_waypointIndex];
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            Debug.Log("EXIT TOILET STATE");

            if (!_exitedToilet)
            {
                if (Operation.IsTargetReached(_ai.transform, _target.position, 0.1f))
                {
                    if (_waypointIndex == 0)
                    {
                        _exitedToilet = true;

                        if (_isMoving)
                        {
                            _isMoving = false;
                            aiStateManager.SwitchState(aiStateManager.WanderState);
                        }
                    }
                    else
                    {
                        _waypointIndex--;
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
