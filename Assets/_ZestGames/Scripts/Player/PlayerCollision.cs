using ClubBusiness;
using UnityEngine;

namespace ZestGames
{
    public class PlayerCollision : MonoBehaviour
    {
        private Player _player;

        public void Init(Player player)
        {
            _player = player;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out UpgradeAreaBase upgradeArea) && !upgradeArea.PlayerIsInArea)
                upgradeArea.StartOpening();

            #region HIRE WORKER SECTION
            if (other.TryGetComponent(out BartenderHireArea bartenderHireArea))
                bartenderHireArea.OpenHireCanvas();
            if (other.TryGetComponent(out BodyguardHireArea bodyguardHireArea))
                bodyguardHireArea.OpenHireCanvas();
            if (other.TryGetComponent(out CleanerHireArea cleanerHireArea))
                cleanerHireArea.OpenHireCanvas();
            if (other.TryGetComponent(out BouncerHireArea bouncerHireArea))
                bouncerHireArea.OpenHireCanvas();
            #endregion

            if (other.TryGetComponent(out QueueActivator queueActivator) && !queueActivator.PlayerIsInArea && queueActivator.CanPlayerActivateQueue)
            {
                queueActivator.StartEmptyingQueue(_player);
                if (queueActivator.QueueSystem.QueueType == Enums.QueueType.Gate)
                    PlayerEvents.OnStartLettingPeopleIn?.Invoke();
                else if (queueActivator.QueueSystem.QueueType == Enums.QueueType.Bar)
                    PlayerEvents.OnStartFillingDrinks?.Invoke();
            }

            if (other.TryGetComponent(out ExamplePoint examplePoint) && !examplePoint.PlayerIsInArea)
            {
                examplePoint.PlayerIsInArea = true;
                _player.MoneyHandler.StartSpending(examplePoint);
            }

            #region WARN WORKER SECTION
            if (other.TryGetComponent(out BodyguardTrigger bodyguardTrigger) && bodyguardTrigger.Bodyguard.IsWastingTime)
            {
                PlayerEvents.OnWarnWorker?.Invoke();
                bodyguardTrigger.Bodyguard.OnGetWarned?.Invoke();
            }
            if (other.TryGetComponent(out BartenderTrigger bartenderTrigger) && bartenderTrigger.Bartender.IsWastingTime)
            {
                PlayerEvents.OnWarnWorker?.Invoke();
                bartenderTrigger.Bartender.OnGetWarned?.Invoke();
            }
            if (other.TryGetComponent(out CleanerTrigger cleanerTrigger) && cleanerTrigger.Cleaner.IsWastingTime)
            {
                PlayerEvents.OnWarnWorker?.Invoke();
                cleanerTrigger.Cleaner.OnGetWarned?.Invoke();
            }
            if (other.TryGetComponent(out BouncerTrigger bouncerTrigger) && bouncerTrigger.Bouncer.IsWastingTime)
            {
                PlayerEvents.OnWarnWorker?.Invoke();
                bouncerTrigger.Bouncer.OnGetWarned?.Invoke();
            }
            #endregion

            if (other.TryGetComponent(out ToiletItem toiletItem) && toiletItem.IsBroken && !toiletItem.PlayerIsInArea && _player.FixedToiletItem == null)
            {
                toiletItem.PlayerIsInArea = true;
                _player.FixedToiletItem = toiletItem;
                PlayerEvents.OnStartFixingToilet?.Invoke(toiletItem);
            }

            if (other.TryGetComponent(out AiTrigger aiTrigger) && aiTrigger.Ai.IsFighting && !aiTrigger.PlayerIsInTrigger)
            {
                aiTrigger.PlayerIsInTrigger = true;
                _player.TimerForAction.StartFilling(DataManager.BreakFightDuration, () => {
                    if (DanceFloor.AttackerAi && DanceFloor.DefenderAi)
                    {
                        aiTrigger.Ai.OnStopFighting?.Invoke();
                        aiTrigger.Ai.StateManager.SwitchState(aiTrigger.Ai.StateManager.LeaveClubState);
                        DanceFloor.DefenderAi.StateManager.SwitchState(DanceFloor.DefenderAi.StateManager.DanceState);

                        ClubEvents.OnEveryoneGetHappier?.Invoke();
                        ClubEvents.OnAFightEnded?.Invoke();
                    }
                });
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out UpgradeAreaBase upgradeArea) && upgradeArea.PlayerIsInArea)
                upgradeArea.StopOpening();

            if (other.TryGetComponent(out QueueActivator queueActivator) && queueActivator.PlayerIsInArea)
            {
                queueActivator.StopEmptyingQueue(_player);
                if (queueActivator.QueueSystem.QueueType == Enums.QueueType.Gate)
                    PlayerEvents.OnStopLettingPeopleIn?.Invoke();
                else if (queueActivator.QueueSystem.QueueType == Enums.QueueType.Bar)
                    PlayerEvents.OnStopFillingDrinks?.Invoke();
            }

            if (other.TryGetComponent(out ExamplePoint examplePoint) && examplePoint.PlayerIsInArea)
            {
                examplePoint.PlayerIsInArea = false;
                _player.MoneyHandler.StopSpending();
            }

            if (other.TryGetComponent(out ToiletItem toiletItem) && toiletItem.PlayerIsInArea && _player.FixedToiletItem == toiletItem)
            {
                toiletItem.PlayerIsInArea = false;
                _player.FixedToiletItem = null;
                PlayerEvents.OnStopFixingToilet?.Invoke();
            }

            if (other.TryGetComponent(out AiTrigger aiTrigger) && aiTrigger.Ai.IsFighting && aiTrigger.PlayerIsInTrigger)
            {
                aiTrigger.PlayerIsInTrigger = false;
                _player.TimerForAction.StopFilling();
            }
        }
    }
}
