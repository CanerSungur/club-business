using UnityEngine;
using ZestCore.Ai;

namespace ClubBusiness
{
    public class CleanerCleanState : CleanerBaseState
    {
        private Cleaner _cleaner;
        private ToiletItem _currentBrokenToilet = null;
        private bool _reachedToToilet, _isMoving;

        private float _timer;
        private float _cleanTime = 5f;

        public ToiletItem CurrentBrokenToilet => _currentBrokenToilet;

        public override void EnterState(CleanerStateManager cleanerStateManager)
        {
            Debug.Log("Clean State");

            if (_cleaner == null)
                _cleaner = cleanerStateManager.Cleaner;

            if (Toilet.CanCleanerFixToilet)
            {
                _currentBrokenToilet = Toilet.BrokenToiletItems[0];
                _reachedToToilet = _isMoving = false;
                _timer = _cleanTime;
            }
            else
            {
                Debug.Log("No toilets to clean.");
                _currentBrokenToilet = null;
                cleanerStateManager.SwitchState(cleanerStateManager.WaitState);
            }
        }

        public override void ExitState(CleanerStateManager cleanerStateManager)
        {

        }

        public override void UpdateState(CleanerStateManager cleanerStateManager)
        {
            if (!_reachedToToilet)
            {
                if (Operation.IsTargetReached(_cleaner.transform, _currentBrokenToilet.CleaningTransform.position, 0.1f))
                {
                    _reachedToToilet = true;

                    if (_isMoving)
                    {
                        _isMoving = false;
                        _cleaner.OnStartCleaning?.Invoke();
                    }
                }
                else
                {
                    Navigation.MoveTransform(_cleaner.transform, _currentBrokenToilet.CleaningTransform.position, _cleaner.MovementSpeed);
                    Navigation.LookAtTarget(_cleaner.transform, _currentBrokenToilet.CleaningTransform.position, _cleaner.RotationSpeed);

                    if (!_isMoving)
                    {
                        _isMoving = true;
                        _cleaner.OnGoCleaning?.Invoke();
                    }
                }
            }
            else
            {

                if (!_currentBrokenToilet.IsBroken)
                {
                    _cleaner.OnStopCleaning?.Invoke();

                    if (Toilet.CanCleanerFixToilet && !_cleaner.IsWastingTime)
                        cleanerStateManager.SwitchState(cleanerStateManager.CleanState);
                    else
                        cleanerStateManager.SwitchState(cleanerStateManager.ExitToiletState);
                }

                // cleaning
                //_timer -= Time.deltaTime;
                //if (_timer <= 0f && _cleaner.IsCleaning)
                //{
                //    _cleaner.OnStopCleaning?.Invoke();
                //    _timer = _cleanTime;

                //    if (Toilet.CanCleanerFixToilet && !_cleaner.IsWastingTime)
                //        cleanerStateManager.SwitchState(cleanerStateManager.CleanState);
                //    else
                //        cleanerStateManager.SwitchState(cleanerStateManager.GoWaitingState);
                //}
            }
        }
    }
}
