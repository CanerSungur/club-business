using UnityEngine;
using ZestCore.Ai;

namespace ClubBusiness
{
    public class CleanerExitToiletState : CleanerBaseState
    {
        private Cleaner _cleaner;
        private bool _exitedToilet, _isMoving;

        //#region SEQUENCE
        //private Sequence _rotationSequence;
        //private Guid _rotationSequenceID;
        //private readonly Vector3 _rotation = new Vector3(0f, 180f, 0f);
        //#endregion

        public override void EnterState(CleanerStateManager cleanerStateManager)
        {
            if (_cleaner == null)
                _cleaner = cleanerStateManager.Cleaner;

            _exitedToilet = _isMoving = false;
        }

        public override void ExitState(CleanerStateManager cleanerStateManager)
        {

        }

        public override void UpdateState(CleanerStateManager cleanerStateManager)
        {
            if (!_exitedToilet)
            {
                if (Operation.IsTargetReached(_cleaner.transform, Toilet.ExitTransform.position, 0.1f))
                {
                    _exitedToilet = true;
                    _cleaner.transform.position = Toilet.ExitTransform.position;
                    //StartRotationSequence();

                    if (_isMoving)
                    {
                        _isMoving = false;

                        cleanerStateManager.SwitchState(cleanerStateManager.GoWaitingState);

                        //if (_cleaner.IsWastingTime)
                        //    cleanerStateManager.SwitchState(cleanerStateManager.WasteTimeState);
                        //else
                        //    cleanerStateManager.SwitchState(cleanerStateManager.WaitState);
                    }
                }
                else
                {
                    Navigation.MoveTransform(_cleaner.transform, Toilet.ExitTransform.position, _cleaner.MovementSpeed);
                    Navigation.LookAtTarget(_cleaner.transform, Toilet.ExitTransform.position, _cleaner.RotationSpeed);

                    if (!_isMoving)
                    {
                        _isMoving = true;
                        _cleaner.OnGoWaiting?.Invoke();
                    }
                }
            }
        }

        //#region DOTWEEN FUNCTIONS
        //private void StartRotationSequence()
        //{
        //    CreateRotationSequence();
        //    _rotationSequence.Play();
        //}
        //private void CreateRotationSequence()
        //{
        //    if (_rotationSequence == null)
        //    {
        //        _rotationSequence = DOTween.Sequence();
        //        _rotationSequenceID = Guid.NewGuid();
        //        _rotationSequence.id = _rotationSequenceID;

        //        _rotationSequence.Append(_cleaner.transform.DORotate(_rotation, 1f)).OnComplete(() => DeleteRotationSequence());
        //    }
        //}
        //private void DeleteRotationSequence()
        //{
        //    DOTween.Kill(_rotationSequenceID);
        //    _rotationSequence = null;
        //}
        //#endregion

        //private Cleaner _cleaner;
        //private Transform _target = null;
        //private int _waypointIndex;
        //private bool _exitedToilet;

        //public override void EnterState(CleanerStateManager cleanerStateManager)
        //{
        //    if (_cleaner == null)
        //        _cleaner = cleanerStateManager.Cleaner;

        //    _exitedToilet = false;
        //}

        //public override void ExitState(CleanerStateManager cleanerStateManager)
        //{
            
        //}

        //public override void UpdateState(CleanerStateManager cleanerStateManager)
        //{
        //    if (!_exitedToilet)
        //    {
        //        if (_target == null)
        //            _target = GetClosestWaypoint();

        //        if (Operation.IsTargetReached(_cleaner.transform, _target.position, 0.05f))
        //        {
        //            if (_waypointIndex == 0)
        //            {
        //                _exitedToilet = true;

        //                if (_cleaner.IsWastingTime)
        //                    cleanerStateManager.SwitchState(cleanerStateManager.WasteTimeState);
        //                else
        //                    cleanerStateManager.SwitchState(cleanerStateManager.WaitState);
        //            }
        //            else
        //            {
        //                _waypointIndex--;
        //                _target = Waypoints.CleanerWaypoints[_waypointIndex];
        //            }

        //        }
        //        else
        //        {
        //            Navigation.MoveTransform(_cleaner.transform, _target.position, _cleaner.MovementSpeed);
        //            Navigation.LookAtTarget(_cleaner.transform, _target.position);
        //        }
        //    }
        //}

        //private Transform GetClosestWaypoint()
        //{
        //    float distance = float.PositiveInfinity;
        //    Transform closestWaypoint = null;
        //    for (int i = 0; i < Waypoints.CleanerWaypoints.Length; i++)
        //    {
        //        if ((_cleaner.transform.position - Waypoints.CleanerWaypoints[i].position).magnitude < distance)
        //        {
        //            closestWaypoint = Waypoints.CleanerWaypoints[i];
        //            _waypointIndex = i;
        //        }
        //    }
        //    return closestWaypoint;
        //}
    }
}
