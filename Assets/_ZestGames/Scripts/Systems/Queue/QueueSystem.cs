using ClubBusiness;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZestGames
{
    public abstract class QueueSystem : MonoBehaviour
    {
        [SerializeField] private Enums.QueueType queueType;
        [Space(10)]

        [Header("-- SETUP --")]
        [SerializeField] private bool spawnWithCode = true;
        [SerializeField] private bool reFormationWhenActivated = true;
        [SerializeField] private int capacity = 5;
        [SerializeField] private QueuePoint queuePointPrefab;
        [SerializeField, Tooltip("Use this if you don't spawn queue points from script")] private QueuePoint[] queuePoints;

        private QueueSystemAnimationController _animationController;

        #region PRIVATES
        private QueueActivator _queueActivator;
        private readonly Quaternion _defaultRotation = Quaternion.Euler(90f, 0f, 0f);
        private readonly float _queueOffset = -0.75f;
        private bool _updatingQueue = false;
        #endregion

        #region PROPERTIES
        public bool QueueIsFull => EmptyQueuePoints.Count == 0 && !_updatingQueue;
        public int Capacity => capacity;
        public Enums.QueueType QueueType => queueType;
        #endregion

        #region QUEUE POINTS LIST SYSTEM
        private Dictionary<int, QueuePoint> _queuePoints = new Dictionary<int, QueuePoint>();
        private List<QueuePoint> _emptyQueuePoints;
        public List<QueuePoint> EmptyQueuePoints => _emptyQueuePoints == null ? _emptyQueuePoints = new List<QueuePoint>() : _emptyQueuePoints;
        public void AddQueuePoint(QueuePoint queuePoint)
        {
            if (!EmptyQueuePoints.Contains(queuePoint))
                EmptyQueuePoints.Add(queuePoint);
        }
        public void RemoveQueuePoint(QueuePoint queuePoint)
        {
            if (EmptyQueuePoints.Contains(queuePoint))
                EmptyQueuePoints.Remove(queuePoint);
        }
        #endregion

        #region AIS IN QUEUE LIST
        private List<Ai> _aisInQueue;
        public List<Ai> AisInQueue => _aisInQueue == null ? _aisInQueue = new List<Ai>() : _aisInQueue;
        public void AddAiInQueue(Ai ai)
        {
            if (!AisInQueue.Contains(ai))
                AisInQueue.Add(ai);
        }
        public void RemoveAiFromQueue(Ai ai)
        {
            if (AisInQueue.Contains(ai))
                AisInQueue.Remove(ai);
        }
        #endregion

        #region EVENTS
        public Action OnPlayerEntered, OnPlayerExited;
        #endregion

        private void Start()
        {
            if (TryGetComponent(out _animationController))
                _animationController.Init(this);

            _queueActivator = GetComponentInChildren<QueueActivator>();
            _queueActivator.Init(this);

            if (spawnWithCode)
                SpawnQueuePoints(capacity);
            else
                InitQueuePoints();

            PlayerEvents.OnEmptyNextInQueue += UpdateQueue;
        }

        private void OnDisable()
        {
            PlayerEvents.OnEmptyNextInQueue -= UpdateQueue;
        }

        private void SpawnQueuePoints(int count)
        {
            int queue = 0;
            for (int i = 0; i < count; i++)
            {
                QueuePoint queuePoint = Instantiate(queuePointPrefab, Vector3.zero, _defaultRotation, transform);
                _queuePoints.Add(i + 1, queuePoint);

                queuePoint.Init(this, i + 1);
                queuePoint.transform.localPosition = new Vector3(0f, 0f, queue * _queueOffset);
                AddQueuePoint(queuePoint);
                queue++;
            }
        }
        private void InitQueuePoints()
        {
            for (int i = 0; i < queuePoints.Length; i++)
            {
                _queuePoints.Add(i + 1, queuePoints[i]);
                queuePoints[i].Init(this, i + 1);
                AddQueuePoint(queuePoints[i]);
            }
        }

        #region EVENT HANDLER FUNCTIONS
        private void UpdateQueue(QueueSystem queueSystem)
        {
            if (queueSystem != this) return;

            if (reFormationWhenActivated)
            {
                _updatingQueue = true;

                Ai firstAi = AisInQueue[0];
                RemoveAiFromQueue(firstAi);
                firstAi.StateManager.GetIntoClubState.CurrentQueuePoint.QueueIsReleased();
                firstAi.StateManager.GetIntoClubState.ActivateStateAfterQueue();

                for (int i = 0; i < AisInQueue.Count; i++)
                {
                    Ai ai = AisInQueue[i];
                    //ai.StateManager.GetIntoQueueState.CurrentQueuePoint.QueueIsReleased();
                    //ai.StateManager.GetIntoQueueState.UpdateQueue(EmptyQueuePoints[0]);
                    ai.StateManager.GetIntoClubState.UpdateQueue(_queuePoints[i + 1]);
                    // go to next queue
                }

                _updatingQueue = false;
            }
            else
            {
                Ai firstAi = AisInQueue[0];
                RemoveAiFromQueue(firstAi);
                firstAi.StateManager.BuyDrinkState.ActivateStateAfterQueue();
            }
        }
        #endregion

        #region PUBLICS
        public QueuePoint GetQueue(Ai ai)
        {
            AddAiInQueue(ai);
            QueuePoint queue = EmptyQueuePoints[0];
            queue.QueueIsTaken();
            return queue;
        }
        #endregion
    }
}
