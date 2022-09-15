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

            if (other.TryGetComponent(out QueueActivator queueActivator) && !queueActivator.PlayerIsInArea && queueActivator.CanPlayerActivateQueue)
            {
                queueActivator.StartEmptyingQueue(_player);
                if (queueActivator.QueueSystem.QueueType == Enums.QueueType.Gate)
                    PlayerEvents.OnStartLettingPeopleIn?.Invoke();
                else if (queueActivator.QueueSystem.QueueType == Enums.QueueType.Bar)
                    PlayerEvents.OnStartFillingDrinks?.Invoke();
                //_player.TimerForAction.StartFilling(() => queueActivator.StartEmptyingQueue());
            }

            if (other.TryGetComponent(out ExamplePoint examplePoint) && !examplePoint.PlayerIsInArea)
            {
                examplePoint.PlayerIsInArea = true;
                _player.MoneyHandler.StartSpending(examplePoint);
            }

            if (other.TryGetComponent(out BodyguardTrigger bodyguardTrigger) && bodyguardTrigger. Bodyguard.IsWastingTime)
            {
                PlayerEvents.OnWarnWorker?.Invoke();
                bodyguardTrigger.Bodyguard.OnGetWarned?.Invoke();
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
                //_player.TimerForAction.StopFilling();
            }

            if (other.TryGetComponent(out ExamplePoint examplePoint) && examplePoint.PlayerIsInArea)
            {
                examplePoint.PlayerIsInArea = false;
                _player.MoneyHandler.StopSpending();
            }
        }
    }
}
