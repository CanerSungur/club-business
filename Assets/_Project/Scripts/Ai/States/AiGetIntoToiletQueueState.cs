using UnityEngine;
using ZestGames;
using DG.Tweening;
using System;
using ZestCore.Ai;

namespace ClubBusiness
{
    public class AiGetIntoToiletQueueState : AiBaseState
    {
        private Ai _ai;

        private QueuePoint _currentQueuePoint;
        private bool _reachedToQueue, _isMoving;

        public QueuePoint CurrentQueuePoint => _currentQueuePoint;
        public bool ReachedToQueue => _reachedToQueue;

        #region SEQUENCE
        private Sequence _rotationSequence;
        private Guid _rotationSequenceID;
        private readonly Vector3 _rotation = new Vector3(0f, 180f, 0f);
        #endregion

        public override void EnterState(AiStateManager aiStateManager)
        {
            if (_ai == null)
                _ai = aiStateManager.Ai;

            _ai.ReactionCanvas.EnablePissing();

            if (QueueManager.ToiletQueue.QueueIsFull)
            {
                // increase anger meter
                _ai.AngerHandler.GetAngrier();

                aiStateManager.WaitState.SetAttemptedState(aiStateManager.GetIntoToiletQueueState);
                aiStateManager.SwitchState(aiStateManager.WaitState);
            }
            else
            {
                aiStateManager.SwitchStateType(Enums.AiStateType.GetIntoToiletQueue);

                _reachedToQueue = _isMoving = false;
                _currentQueuePoint = QueueManager.ToiletQueue.GetQueue(_ai);

                _ai.AngerHandler.GetHappier();
            }
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            //Debug.Log("TOILET QUEUE STATE");

            if (_currentQueuePoint == null)
            {
                Debug.Log("Switching to idle, Queue point is null");
                aiStateManager.SwitchState(aiStateManager.IdleState);
                return;
            }

            if (!_reachedToQueue)
            {
                // go to queue
                if (Operation.IsTargetReached(_ai.transform, _currentQueuePoint.transform.position, 0.002f))
                {
                    _reachedToQueue = true;
                    StartRotationSequence();
                    _ai.OnIdle?.Invoke();
                    _isMoving = false;
                }
                else
                {
                    if (!_isMoving)
                    {
                        _ai.OnMove?.Invoke();
                        _isMoving = true;
                    }

                    Navigation.MoveTransform(_ai.transform, _currentQueuePoint.transform.position, _ai.RunSpeed);
                    Navigation.LookAtTarget(_ai.transform, _currentQueuePoint.transform.position);
                }
            }
        }

        #region PUBLICS
        public void UpdateQueue(QueuePoint queuePoint)
        {
            _currentQueuePoint.QueueIsReleased();
            _currentQueuePoint = queuePoint;
            _currentQueuePoint.QueueIsTaken();

            _reachedToQueue = false;
            _ai.OnMove?.Invoke();
            _isMoving = true;
        }
        public void ActivateStateAfterQueue()
        {
            _ai.StateManager.SwitchState(_ai.StateManager.GoToToiletState);
        }
        #endregion

        private void StartRotationSequence()
        {
            CreateRotationSequence();
            _rotationSequence.Play();
        }

        #region DOTWEEN FUNCTIONS
        private void CreateRotationSequence()
        {
            if (_rotationSequence == null)
            {
                _rotationSequence = DOTween.Sequence();
                _rotationSequenceID = Guid.NewGuid();
                _rotationSequence.id = _rotationSequenceID;

                _rotationSequence.Append(_ai.transform.DORotate(_rotation, 1f)).OnComplete(() => DeleteRotationSequence());
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
