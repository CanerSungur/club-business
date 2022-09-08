using UnityEngine;
using ZestGames;
using DG.Tweening;
using System;
using ZestCore.Ai;
using Random = UnityEngine.Random;

namespace ClubBusiness
{
    public class AiGoToToiletState : AiBaseState
    {
        private Ai _ai;

        private ToiletItem _currentToiletItem;
        private bool _reachedToToilet, _isMoving;
        private readonly float _pissDuration = 10f;
        private float _timer;

        #region SEQUENCE
        private Sequence _rotationSequence;
        private Guid _rotationSequenceID;
        private readonly Vector3 _rotation = Vector3.zero;
        #endregion

        public override void EnterState(AiStateManager aiStateManager)
        {
            aiStateManager.SwitchStateType(Enums.AiStateType.GoToToilet);

            if (_ai == null)
                _ai = aiStateManager.Ai;

            _reachedToToilet = _isMoving = false;
            _timer = _pissDuration;

            if (ClubManager.ToiletIsAvailable)
            {
                ToiletItem randomToilet = Toilet.EmptyToiletItems[Random.Range(0, Toilet.EmptyToiletItems.Count)];
                randomToilet.Occupy();
                _currentToiletItem = randomToilet;
            }
            else
                Debug.Log("No available toilet!");
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            if (!_reachedToToilet)
            {
                if (Operation.IsTargetReached(_ai.transform, _currentToiletItem.PointTransform.position, 0.002f))
                {
                    _reachedToToilet = true;
                    StartRotationSequence();
                    _ai.OnStartPissing?.Invoke();
                }
                else
                {
                    if (!_isMoving)
                    {
                        _isMoving = true;
                        _ai.OnMove?.Invoke();
                    }

                    Navigation.MoveTransform(_ai.transform, _currentToiletItem.PointTransform.position, _ai.RunSpeed);
                    Navigation.LookAtTarget(_ai.transform, _currentToiletItem.PointTransform.position);
                }
            }
            else
            {
                // pissing right now.
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    _currentToiletItem.Release();
                    _ai.OnStopPissing?.Invoke();
                    aiStateManager.SwitchState(aiStateManager.WanderState);

                    // activate first ai if there is a toilet queue
                    if (QueueManager.ToiletQueue.AisInQueue.Count > 0)
                    {
                        //Ai firstAiInToiletQueue = QueueManager.ToiletQueue.AisInQueue[0];
                        QueueManager.ToiletQueue.UpdateToiletQueue();
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
