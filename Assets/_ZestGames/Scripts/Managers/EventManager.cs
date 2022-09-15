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
        public static Action OnStartLettingPeopleIn, OnStopLettingPeopleIn, OnStartFillingDrinks, OnStopFillingDrinks, OnStartCleaning, OnStopCleaning, OnStartBreakingUpFight, OnStopBreakingUpFight;
        public static Action OnBodyguardIsActive, OnCleanerIsActive, OnBartenderIsActive;
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

    public static class GateEvents
    {
        public static Action OnBodyguardHired, OnSetCurrentBodyguardStamina, OnSetCurrentBodyguardSpeed;
    }
    public static class GateUpgradeEvents
    {
        public static Action OnOpenCanvas, OnCloseCanvas, OnUpdateUpgradeTexts, OnUpgradeBodyguardHire, OnUpgradeBodyguardStamina, OnUpgradeBodyguardSpeed;
    }
}
