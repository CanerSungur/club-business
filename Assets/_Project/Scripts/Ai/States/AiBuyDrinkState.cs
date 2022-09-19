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
        private bool _reachedToQueue, _tookDrink, _turnIsUp;

        private float _waitTimer;

        #region PROPERTIES
        public bool ReachedToQueue => _reachedToQueue;
        public QueuePoint CurrentQueuePoint => _currentQueuePoint;
        #endregion

        #region SEQUENCE
        private Sequence _sittingSequence;
        private Guid _sittingSequenceID;
        private readonly Vector3 _rotation = new Vector3(0f, -90f, 0f);
        private readonly float _sitHeight = 1.25f;
        private float _currentHeight;
        #endregion

        public override void EnterState(AiStateManager aiStateManager)
        {
            if (_ai == null)
                _ai = aiStateManager.Ai;

            _reachedToQueue = _tookDrink = _turnIsUp = false;
            _waitTimer = CustomerManager.CustomerWaitDuration;
            _ai.ReactionCanvas.EnableDrinking();

            if (QueueManager.BarQueue.QueueIsFull)
            {
                // increase anger meter
                _ai.AngerHandler.GetAngrier();
                // continue waiting
                aiStateManager.WaitState.SetAttemptedState(aiStateManager.BuyDrinkState);
                aiStateManager.SwitchState(aiStateManager.WaitState);
            }
            else
            {
                aiStateManager.SwitchStateType(Enums.AiStateType.BuyDrink);

                _currentQueuePoint = QueueManager.BarQueue.GetQueue(_ai);

                _ai.OnMove?.Invoke();

                _ai.AngerHandler.GetHappier();
            }
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            Debug.Log("DRINK STATE");

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
                    StartSittingSequence();

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
            else
            {
                if (!_tookDrink)
                {
                    _waitTimer -= Time.deltaTime;
                    if (_waitTimer <= 0f && !_turnIsUp)
                    {
                        _ai.AngerHandler.GetAngrier();
                        _waitTimer = CustomerManager.CustomerWaitDuration;
                    }
                }
            }
        }

        #region PUBLICS
        public void ActivateStateAfterQueue()
        {
            // Take drink
            _ai.MoneyHandler.StartSpawningDrinkingMoney();
            // Drink it
            _ai.OnStopAskingForDrink?.Invoke();
            _ai.OnDrink?.Invoke();
            _tookDrink = true;

            //CustomerManager.RemoveCustomerOutside(_ai);
            //CustomerManager.AddCustomerInside(_ai);
            //_ai.StateManager.SwitchState(_ai.StateManager.WanderState);
        }
        public void FinishDrinking()
        {
            _currentQueuePoint.QueueIsReleased();
            _ai.StateManager.SwitchState(_ai.StateManager.WanderState);
        }
        public void TurnIsUp() => _turnIsUp = true;
        #endregion

        private void StartSittingSequence()
        {
            _ai.Rigidbody.isKinematic = true;
            CreateSittingSequence();
            _sittingSequence.Play();
        }

        #region DOTWEEN FUNCTIONS
        private void CreateSittingSequence()
        {
            if (_sittingSequence == null)
            {
                _sittingSequence = DOTween.Sequence();
                _sittingSequenceID = Guid.NewGuid();
                _sittingSequence.id = _sittingSequenceID;

                _sittingSequence.Append(_ai.transform.DORotate(_rotation, 1f))
                    .Join(_ai.transform.DOLocalMoveY(_sitHeight, 1f)).OnComplete(() => DeleteSittingSequence());
            }
        }
        private void DeleteSittingSequence()
        {
            DOTween.Kill(_sittingSequenceID);
            _sittingSequence = null;
        }
        #endregion
    }
}
