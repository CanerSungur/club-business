using UnityEngine;
using ZestCore.Ai;
using DG.Tweening;
using System;
using ClubBusiness;

namespace ZestGames
{
    public class AiGetIntoQueue : AiBaseState
    {
        private Ai _ai;

        private QueuePoint _currentQueuePoint;
        private bool _reachedToQueue, _isMoving;

        public QueuePoint CurrentQueuePoint => _currentQueuePoint;
        public bool ReachedToQueue => _reachedToQueue;

        #region SEQUENCE
        private Sequence _rotationSequence;
        private Guid _rotationSequenceID;
        private readonly Vector3 _rotation = new Vector3(0f, -90f, 0f);
        #endregion

        public override void EnterState(AiStateManager aiStateManager)
        {
            //Debug.Log("Entered get into queue state.");
            aiStateManager.SwitchStateType(Enums.AiStateType.GetInQueue);

            if (_ai == null)
                _ai = aiStateManager.Ai;

            _reachedToQueue = false;
            _currentQueuePoint = QueueManager.ExampleQueue.GetQueue(_ai);

            _ai.OnMove?.Invoke();
            _isMoving = true;
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
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
                }
                else
                {
                    Navigation.MoveTransform(_ai.transform, _currentQueuePoint.transform.position, _ai.RunSpeed);
                    Navigation.LookAtTarget(_ai.transform, _currentQueuePoint.transform.position);
                }
            }
            else
            {
                // wait for your turn
                if (_isMoving)
                {
                    _ai.OnIdle?.Invoke();
                    _isMoving = false;
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
            CustomerManager.RemoveCustomerOutside(_ai);
            CustomerManager.AddCustomerInside(_ai);
            _ai.StateManager.SwitchState(_ai.StateManager.WanderState);
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
