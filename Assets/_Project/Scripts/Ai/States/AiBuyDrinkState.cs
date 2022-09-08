using DG.Tweening;
using System;
using UnityEngine;
using ZestGames;
using ZestCore.Ai;
using ZestCore.Utility;

namespace ClubBusiness
{
    public class AiBuyDrinkState : AiBaseState
    {
        private Ai _ai;
        private QueuePoint _currentQueuePoint;
        private bool _reachedToQueue;

        public QueuePoint CurrentQueuePoint => _currentQueuePoint;
        public bool ReachedToQueue => _reachedToQueue;

        #region SEQUENCE
        private Sequence _rotationSequence;
        private Guid _rotationSequenceID;
        private readonly Vector3 _rotation = new Vector3(0f, -90f, 0f);
        #endregion

        public override void EnterState(AiStateManager aiStateManager)
        {
            aiStateManager.SwitchStateType(Enums.AiStateType.BuyDrink);

            if (_ai == null)
                _ai = aiStateManager.Ai;

            _reachedToQueue = false;
            _currentQueuePoint = QueueManager.BarQueue.GetQueue(_ai);

            _ai.OnMove?.Invoke();
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

                    // start asking for drink animation
                    _ai.OnIdle?.Invoke();
                    _ai.OnStartAskingForDrink?.Invoke();
                }
                else
                {
                    Navigation.MoveTransform(_ai.transform, _currentQueuePoint.transform.position, _ai.RunSpeed);
                    Navigation.LookAtTarget(_ai.transform, _currentQueuePoint.transform.position);
                }
            }
        }

        #region PUBLICS
        public void ActivateStateAfterQueue()
        {
            // Take drink
            // Drink it
            _ai.OnStopAskingForDrink?.Invoke();
            _ai.OnDrink?.Invoke();

            //CustomerManager.RemoveCustomerOutside(_ai);
            //CustomerManager.AddCustomerInside(_ai);
            //_ai.StateManager.SwitchState(_ai.StateManager.WanderState);
        }
        public void FinishDrinking()
        {
            _currentQueuePoint.QueueIsReleased();
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
