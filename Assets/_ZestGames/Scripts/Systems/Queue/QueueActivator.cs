using ClubBusiness;
using System.Collections;
using UnityEngine;
using ZestCore.Utility;

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
        public bool CanPlayerActivateQueue { get; private set; }
        public QueueSystem QueueSystem => _queueSystem;
        public bool CanBodyguardTakeSomeoneIn => ClubManager.ClubHasCapacity && _queueSystem.EmptyQueuePoints.Count < _queueSystem.Capacity && _queueSystem.AisInQueue.Count > 0 && _queueSystem.AisInQueue[0].StateManager.GetIntoClubState.ReachedToQueue;
        public bool CanTakeSomeoneIn => ClubManager.ClubHasCapacity && _player != null && _queueSystem.EmptyQueuePoints.Count < _queueSystem.Capacity && _queueSystem.AisInQueue.Count > 0 && _queueSystem.AisInQueue[0].StateManager.GetIntoClubState.ReachedToQueue && !_player.TimerForAction.IsFilling;
        public bool CanGiveDrink => _player != null && _queueSystem.EmptyQueuePoints.Count < _queueSystem.Capacity && _queueSystem.AisInQueue.Count > 0 && _queueSystem.AisInQueue[0].StateManager.BuyDrinkState.ReachedToQueue && !_player.TimerForAction.IsFilling;
        public bool CanBartenderGiveDrink => _queueSystem.EmptyQueuePoints.Count < _queueSystem.Capacity && _queueSystem.AisInQueue.Count > 0 && _queueSystem.AisInQueue[0].StateManager.BuyDrinkState.ReachedToQueue;
        #endregion

        public void Init(QueueSystem queueSystem)
        {
            if (_queueSystem == null)
                _queueSystem = queueSystem;

            CanPlayerActivateQueue = true;
            PlayerIsInArea = false;
            _emptyCoroutine = null;

            Delayer.DoActionAfterDelay(this, 0.5f, CheckForPlayerActivation);
            
            GateUpgradeEvents.OnUpgradeBodyguardHire += CheckForPlayerActivation;
            BarUpgradeEvents.OnUpgradeBartenderHire += CheckForPlayerActivation;
        }

        private void OnDisable()
        {
            if (_queueSystem == null) return;

            GateUpgradeEvents.OnUpgradeBodyguardHire -= CheckForPlayerActivation;
            BarUpgradeEvents.OnUpgradeBartenderHire -= CheckForPlayerActivation;
        }

        #region EVENT HANDLER FUNCTIONS
        private void CheckForPlayerActivation()
        {
            if (_queueSystem.QueueType == Enums.QueueType.Gate && Gate.BodyguardHired)
                CanPlayerActivateQueue = false;
            else if (_queueSystem.QueueType == Enums.QueueType.Bar && Bar.BartenderHired)
                CanPlayerActivateQueue = false;
        }
        #endregion

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
                        player.TimerForAction.StartFilling(DataManager.LetInDuration, () => PlayerEvents.OnEmptyNextInGateQueue?.Invoke());

                    if (!ClubManager.ClubHasCapacity)
                        Debug.Log("NO ROOM INSIDE!");
                }
                else if (_queueSystem.QueueType == Enums.QueueType.Bar)
                {
                    if (CanGiveDrink)
                    {
                        //player.TimerForAction.StartFilling(DataManager.FillDrinkDuration, () => PlayerEvents.OnEmptyNextInQueue?.Invoke(_queueSystem));
                        Ai firstAi = _queueSystem.AisInQueue[0];
                        firstAi.BarTurnIsUp();
                        player.TimerForAction.StartFilling(DataManager.FillDrinkDuration, () => PlayerEvents.OnThrowADrink?.Invoke(firstAi));
                        _queueSystem.RemoveAiFromQueue(firstAi);
                    }

                    if (_queueSystem.QueueIsFull)
                        Debug.Log("NO ROOM AT THE BAR!");
                }

                yield return _waitForBetweenActivations;
            }
        }
        #endregion
    }
}
