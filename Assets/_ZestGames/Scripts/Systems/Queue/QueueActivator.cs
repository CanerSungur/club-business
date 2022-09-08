using ClubBusiness;
using System.Collections;
using UnityEngine;

namespace ZestGames
{
    public class QueueActivator : MonoBehaviour
    {
        private QueueSystem _queueSystem;
        private Player _player;

        private readonly WaitForSeconds _waitForBetweenActivations = new WaitForSeconds(0.5f);
        private IEnumerator _emptyCoroutine;

        #region PROPERTIES
        public bool PlayerIsInArea { get; private set; }
        public QueueSystem QueueSystem => _queueSystem;
        public bool CanTakeSomeoneIn => ClubManager.ClubHasCapacity && _player != null && _queueSystem.EmptyQueuePoints.Count < _queueSystem.Capacity && _queueSystem.AisInQueue[0].StateManager.GetIntoClubState.ReachedToQueue && !_player.TimerForAction.IsFilling;
        public bool CanGiveDrink => _player != null && _queueSystem.EmptyQueuePoints.Count < _queueSystem.Capacity && _queueSystem.AisInQueue[0].StateManager.BuyDrinkState.ReachedToQueue && !_player.TimerForAction.IsFilling;
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
            _queueSystem.OnPlayerEntered?.Invoke();
        }
        public void StopEmptyingQueue(Player player)
        {
            PlayerIsInArea = false;
            player.TimerForAction.StopFilling();
            StopEmptyingCoroutine();
            _queueSystem.OnPlayerExited?.Invoke();
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
                if (_queueSystem.QueueType == Enums.QueueType.Gate)
                {
                    if (CanTakeSomeoneIn)
                        player.TimerForAction.StartFilling(DataManager.LetInDuration, () => PlayerEvents.OnEmptyNextInQueue?.Invoke(_queueSystem));

                    if (!ClubManager.ClubHasCapacity)
                        Debug.Log("NO ROOM INSIDE!");
                }
                else if (_queueSystem.QueueType == Enums.QueueType.Bar)
                {
                    if (CanGiveDrink)
                        player.TimerForAction.StartFilling(DataManager.FillDrinkDuration, () => PlayerEvents.OnEmptyNextInQueue?.Invoke(_queueSystem));

                    if (_queueSystem.QueueIsFull)
                        Debug.Log("NO ROOM AT THE BAR!");
                }

                yield return _waitForBetweenActivations;
            }
        }
        #endregion
    }
}
