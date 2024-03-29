using ClubBusiness;
using System;
using UnityEngine;

namespace ZestGames
{
    public static class EventManager { }

    public static class GameEvents
    {
        public static Action OnGameStart, OnLevelSuccess, OnLevelFail, OnChangePhase;
        public static Action<Enums.GameEnd> OnGameEnd, OnChangeScene;
    }

    public static class PlayerEvents
    {
        public static Action OnMove, OnIdle, OnDie, OnWin, OnLose, OnCheer, OnStopSpendingMoney, OnWarnWorker;
        public static Action OnSetCurrentMovementSpeed, OnSetCurrentMoneyValue;
        public static Action OnOpenedUpgradeCanvas, OnClosedUpgradeCanvas;
        public static Action OnEmptyNextInGateQueue;
        public static Action<Ai> OnEmptyNextInBarQueue, OnThrowADrink;
        public static Action OnStartLettingPeopleIn, OnStopLettingPeopleIn, OnStartFillingDrinks, OnStopFillingDrinks, OnStartBreakingUpFight, OnStopBreakingUpFight;
        public static Action OnBodyguardIsActive, OnCleanerIsActive, OnBartenderIsActive;
        public static Action<ToiletItem> OnStartFixingToilet;
        public static Action OnStopFixingToilet;
    }

    public static class PlayerUpgradeEvents
    {
        public static Action OnOpenCanvas, OnCloseCanvas, OnUpdateUpgradeTexts, OnUpgradeMovementSpeed, OnUpgradeMoneyValue;
    }

    public static class UiEvents
    {
        public static Action<int> OnUpdateLevelText;
        public static Action<float> OnUpdateCollectableText;
        public static Action<string, FeedBackUi.Colors> OnGiveFeedBack;
    }

    public static class CollectableEvents
    {
        public static Action<float> OnCollect, OnSpend;
    }

    public static class InputEvents
    {
        public static Action OnTapHappened, OnTouchStarted, OnTouchStopped;
    }

    public static class AudioEvents
    {
        public static Action OnPlayCollectMoney, OnPlaySpendMoney;
    }

    public static class ClubEvents
    {
        public static Action OnAFightEnded, OnEveryoneGetAngrier, OnEveryoneGetHappier;
    }

    public static class GateEvents
    {
        public static Action OnBodyguardHired, OnSetCurrentBodyguardStamina, OnSetCurrentBodyguardSpeed;
    }
    public static class GateUpgradeEvents
    {
        public static Action OnOpenCanvas, OnCloseCanvas, OnUpdateUpgradeTexts, OnUpgradeBodyguardHire, OnUpgradeBodyguardStamina, OnUpgradeBodyguardSpeed;
        public static Action OnOpenHireCanvas, OnCloseHireCanvas;
    }

    public static class BarEvents
    {
        public static Action OnBartenderHired, OnSetCurrentBartenderStamina, OnSetCurrentBartenderSpeed;
    }
    public static class BarUpgradeEvents
    {
        public static Action OnOpenCanvas, OnCloseCanvas, OnUpdateUpgradeTexts, OnUpgradeBartenderHire, OnUpgradeBartenderStamina, OnUpgradeBartenderSpeed;
        public static Action OnOpenHireCanvas, OnCloseHireCanvas;
    }

    public static class ToiletEvents
    {
        public static Action OnCleanerHired, OnSetCurrentCleanerStamina, OnSetCurrentCleanerSpeed, OnSetCurrentToiletDuration;
    }
    public static class ToiletUpgradeEvents
    {
        public static Action OnOpenCanvas, OnCloseCanvas, OnUpdateUpgradeTexts, OnUpgradeCleanerHire, OnUpgradeCleanerStamina, OnUpgradeCleanerSpeed, OnUpgradeToiletDuration;
        public static Action OnOpenHireCanvas, OnCloseHireCanvas;
    }

    public static class DanceFloorEvents
    {
        public static Action OnBouncerHired, OnSetCurrentBouncerStamina, OnSetCurrentBouncerPower;

    }
    public static class DanceFloorUpgradeEvents
    {
        public static Action OnOpenCanvas, OnCloseCanvas, OnUpdateUpgradeTexts, OnUpgradeBouncerHire, OnUpgradeBouncerStamina, OnUpgradeBouncerPower;
        public static Action OnOpenHireCanvas, OnCloseHireCanvas;
    }
}
