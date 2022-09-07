using ClubBusiness;
using System.Collections;
using UnityEngine;

namespace ZestGames
{
    public class QueueActivator : MonoBehaviour
    {
        private QueueSystem _queueSystem;
        private Player _player;

        private readonly float _queueActivatorDelay = 1f;
        private readonly WaitForSeconds _waitForBetweenActivations = new WaitForSeconds(0.5f);
        private IEnumerator _emptyCoroutine;

        #region PROPERTIES
        public bool PlayerIsInArea { get; private set; }
        public bool CanTakeSomeoneIn => ClubManager.ClubHasCapacity && _player != null && _queueSystem.EmptyQueuePoints.Count < _queueSystem.Capacity && _queueSystem.AisInQueue[0].StateManager.GetIntoClubState.ReachedToQueue && !_player.TimerForAction.IsFilling;
        #endregion

        public void Init(QueueSystem queueSystem)
        {
            if (_queueSystem == null)
                _queueSystem = queueSystem;

            PlayerIsInArea = false;
            _emptyCoroutine = null;
        }

        #region PUBLICS
        public void StartEmptyingQueue(Player player)
        {
            if (_player == null)
                _player = player;

            PlayerIsInArea = true;

            if (_queueSystem.EmptyQueuePoints.Count == _queueSystem.Capacity) // queue is empty
                Debug.Log("Line is empty");

            StartEmptyingCoroutine(player);
        }
        public void StopEmptyingQueue(Player player)
        {
            PlayerIsInArea = false;
            player.TimerForAction.StopFilling(_queueActivatorDelay);
            StopEmptyingCoroutine();
        }
        #endregion

        #region COROUTINE FUNCTIONS
        private void StartEmptyingCoroutine(Player player)
        {
            _emptyCoroutine = EmptyQueueCoroutine(player);
            StartCoroutine(_emptyCoroutine);
        }
        private void StopEmptyingCoroutine()
        {
            if (_emptyCoroutine != null)
                StopCoroutine(_emptyCoroutine);
        }
        private IEnumerator EmptyQueueCoroutine(Player player)
        {
            while (true)
            {
                if (CanTakeSomeoneIn)
                    player.TimerForAction.StartFilling(_queueActivatorDelay, () => PlayerEvents.OnEmptyNextInQueue?.Invoke());

                if (!ClubManager.ClubHasCapacity)
                    Debug.Log("NO ROOM INSIDE!");

                yield return _waitForBetweenActivations;
            }
        }
        #endregion
    }
}
