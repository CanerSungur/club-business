using UnityEngine;
using System;
using ZestGames;
using DG.Tweening;

namespace ClubBusiness
{
    public class Bartender : MonoBehaviour
    {
        private bool _initialized = false;
        private float _currentStamina;

        [Header("-- SETUP --")]
        [SerializeField] private Animator sleepCanvasAnimator;

        #region SCRIPT REFERENCES
        private BartenderAnimationController _animationController;
        public BartenderAnimationController AnimationController => _animationController == null ? _animationController = GetComponent<BartenderAnimationController>() : _animationController;
        private BartenderStateManager _stateManager;
        public BartenderStateManager StateManager => _stateManager == null ? _stateManager = GetComponent<BartenderStateManager>() : _stateManager;
        private BartenderTrigger _trigger;
        public BartenderTrigger Trigger => _trigger == null ? _trigger = GetComponentInChildren<BartenderTrigger>() : _trigger;
        #endregion

        #region EVENTS
        public Action OnWaitForCustomer, OnWasteTime, OnStartPouringDrink, OnStopPouringDrink, OnGetWarned, OnFinishDrink;
        #endregion

        #region PROPERTIES
        public bool IsWastingTime { get; private set; }
        public bool IsPouringDrink { get; set; }
        #endregion

        public void Init(Bar bar)
        {
            _initialized = true;
            _currentStamina = 20;

            IsWastingTime = IsPouringDrink = false;

            AnimationController.Init(this);
            StateManager.Init(this);
            Trigger.Init(this);

            OnFinishDrink += DrinkFinished;
            OnWaitForCustomer += WaitForCustomers;
            OnWasteTime += WasteTime;
            OnGetWarned += GetWarned;

            BarEvents.OnSetCurrentBartenderStamina += UpdateStamina;
            BarUpgradeEvents.OnUpgradeBartenderSpeed += UpgradeHappened;
            BarUpgradeEvents.OnUpgradeBartenderStamina += UpgradeHappened;

            Bounce();
        }

        private void OnDisable()
        {
            if (!_initialized) return;

            OnFinishDrink -= DrinkFinished;
            OnWaitForCustomer -= WaitForCustomers;
            OnWasteTime -= WasteTime;
            OnGetWarned -= GetWarned;

            BarEvents.OnSetCurrentBartenderStamina -= UpdateStamina;
            BarUpgradeEvents.OnUpgradeBartenderSpeed -= UpgradeHappened;
            BarUpgradeEvents.OnUpgradeBartenderStamina -= UpgradeHappened;

            transform.DOKill();
        }

        private void Bounce()
        {
            transform.DOShakeScale(1f, 0.25f);
        }

        #region EVENT HANDLER FUNCTIONS
        private void UpgradeHappened()
        {
            ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.LevelUp_PS, transform.position, Quaternion.Euler(-90f, 0f, 0f));
        }
        private void UpdateStamina() => OnGetWarned?.Invoke();
        private void DrinkFinished()
        {
            _currentStamina--;

            if (_currentStamina <= 0)
                IsWastingTime = true;
            else
                IsWastingTime = false;
        }
        private void WaitForCustomers()
        {
            //IsWastingTime = false;
            QueueManager.BarQueue.OnAiIsWorking?.Invoke();
            sleepCanvasAnimator.SetBool("Open", false);
        }
        private void WasteTime()
        {
            //IsWastingTime = true;
            QueueManager.BarQueue.OnAiIsNotWorking?.Invoke();
            sleepCanvasAnimator.SetBool("Open", true);
        }
        private void GetWarned()
        {
            IsWastingTime = false;
            _currentStamina = 20;
            StateManager.SwitchState(StateManager.WaitForCustomerState);
        }
        #endregion
    }
}
