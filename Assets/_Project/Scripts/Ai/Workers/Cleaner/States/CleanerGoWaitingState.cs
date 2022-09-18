using UnityEngine;
using ZestCore.Ai;
using DG.Tweening;
using System;

namespace ClubBusiness
{
    public class CleanerGoWaitingState : CleanerBaseState
    {
        private Cleaner _cleaner;
        private bool _reachedToWaitingPosition, _isMoving;

        #region SEQUENCE
        private Sequence _rotationSequence;
        private Guid _rotationSequenceID;
        private readonly Vector3 _rotation = new Vector3(0f, 180f, 0f);
        #endregion

        public override void EnterState(CleanerStateManager cleanerStateManager)
        {
            if (_cleaner == null)
                _cleaner = cleanerStateManager.Cleaner;

            _reachedToWaitingPosition = _isMoving = false;
        }

        public override void ExitState(CleanerStateManager cleanerStateManager)
        {
            
        }

        public override void UpdateState(CleanerStateManager cleanerStateManager)
        {
            if (!_reachedToWaitingPosition)
            {
                if (Operation.IsTargetReached(_cleaner.transform, Toilet.CleanerWaitTransform.position, 0.1f))
                {
                    _reachedToWaitingPosition = true;
                    _cleaner.transform.position = Toilet.CleanerWaitTransform.position;
                    StartRotationSequence();

                    if (_isMoving)
                    {
                        _isMoving = false;

                        if (_cleaner.IsWastingTime)
                            cleanerStateManager.SwitchState(cleanerStateManager.WasteTimeState);
                        else
                            cleanerStateManager.SwitchState(cleanerStateManager.WaitState);
                    }
                }
                else
                {
                    Navigation.MoveTransform(_cleaner.transform, Toilet.CleanerWaitTransform.position, _cleaner.MovementSpeed);
                    Navigation.LookAtTarget(_cleaner.transform, Toilet.CleanerWaitTransform.position);

                    if (!_isMoving)
                    {
                        _isMoving = true;
                        _cleaner.OnGoWaiting?.Invoke();
                    }
                }
            }
        }

        #region DOTWEEN FUNCTIONS
        private void StartRotationSequence()
        {
            CreateRotationSequence();
            _rotationSequence.Play();
        }
        private void CreateRotationSequence()
        {
            if (_rotationSequence == null)
            {
                _rotationSequence = DOTween.Sequence();
                _rotationSequenceID = Guid.NewGuid();
                _rotationSequence.id = _rotationSequenceID;

                _rotationSequence.Append(_cleaner.transform.DORotate(_rotation, 1f)).OnComplete(() => DeleteRotationSequence());
            }
        }
        private void DeleteRotationSequence()
        {
            DOTween.Kill(_rotationSequenceID);
            _rotationSequence = null;
        }
        #endregion
    }
}
