using UnityEngine;
using ZestCore.Ai;
using DG.Tweening;
using System;

namespace ClubBusiness
{
    public class BouncerGoWaitingState : BouncerBaseState
    {
        private Bouncer _bouncer;
        private Transform _target = null;
        private bool _reachedToWaitingPoint, _isMoving;

        #region SEQUENCE
        private Sequence _rotationSequence;
        private Guid _rotationSequenceID;
        private readonly Vector3 _rotation = Vector3.zero;
        #endregion

        public override void EnterState(BouncerStateManager bouncerStateManager)
        {
            if (_bouncer == null)
                _bouncer = bouncerStateManager.Bouncer;

            _reachedToWaitingPoint = _isMoving = false;
            _target = DanceFloor.BouncerWaitTransform;
        }

        public override void ExitState(BouncerStateManager bouncerStateManager)
        {

        }

        public override void UpdateState(BouncerStateManager bouncerStateManager)
        {
            if (!_reachedToWaitingPoint)
            {
                if (Operation.IsTargetReached(_bouncer.transform, _target.position, 0.05f))
                {
                    _reachedToWaitingPoint = true;
                    _bouncer.transform.position = _target.position;
                    StartRotationSequence();

                    if (_bouncer.IsWastingTime)
                        bouncerStateManager.SwitchState(bouncerStateManager.WasteTimeState);
                    else
                        bouncerStateManager.SwitchState(bouncerStateManager.WaitForFightState);
                }
                else
                {
                    if (!_isMoving)
                    {
                        _isMoving = true;
                        _bouncer.OnGoWaitingForFight?.Invoke();
                    }

                    Navigation.MoveTransform(_bouncer.transform, _target.position, _bouncer.MovementSpeed);
                    Navigation.LookAtTarget(_bouncer.transform, _target.position);
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

                _rotationSequence.Append(_bouncer.transform.DORotate(_rotation, 1f)).OnComplete(() => DeleteRotationSequence());
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
