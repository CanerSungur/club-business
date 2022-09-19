using UnityEngine;
using ZestCore.Ai;

namespace ClubBusiness
{
    public class CleanerEnterToiletState : CleanerBaseState
    {
        private Cleaner _cleaner;
        private Transform _target = null;
        private int _waypointIndex;
        private bool _enteredToilet, _isMoving;

        public override void EnterState(CleanerStateManager cleanerStateManager)
        {
            if (_cleaner == null)
                _cleaner = cleanerStateManager.Cleaner;

            _enteredToilet = _isMoving = false;
        }

        public override void ExitState(CleanerStateManager cleanerStateManager)
        {
            
        }

        public override void UpdateState(CleanerStateManager cleanerStateManager)
        {
            if (!_enteredToilet)
            {
                if (_target == null)
                    _target = GetClosestWaypoint();

                if (Operation.IsTargetReached(_cleaner.transform, _target.position, 0.05f))
                {
                    if (_waypointIndex == CleanerWaypoints.Waypoints.Length - 1)
                    {
                        _enteredToilet = true;
                        cleanerStateManager.SwitchState(cleanerStateManager.CleanState);
                    }
                    else
                    {
                        _waypointIndex++;
                        _target = CleanerWaypoints.Waypoints[_waypointIndex];
                    }
                }
                else
                {
                    Navigation.MoveTransform(_cleaner.transform, _target.position, _cleaner.MovementSpeed);
                    Navigation.LookAtTarget(_cleaner.transform, _target.position, _cleaner.RotationSpeed);

                    if (!_isMoving)
                    {
                        _isMoving = true;
                        _cleaner.OnGoCleaning?.Invoke();
                    }
                }
            }
        }

        private Transform GetClosestWaypoint()
        {
            float distance = float.PositiveInfinity;
            Transform closestWaypoint = null;
            for (int i = 0; i < CleanerWaypoints.Waypoints.Length; i++)
            {
                if ((_cleaner.transform.position - CleanerWaypoints.Waypoints[i].position).magnitude < distance)
                {
                    closestWaypoint = CleanerWaypoints.Waypoints[i];
                    _waypointIndex = i;
                }
            }
            return closestWaypoint;
        }
    }
}
