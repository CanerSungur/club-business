using System;
using UnityEngine;
using ZestGames;
using DG.Tweening;

namespace ClubBusiness
{
    public class Bodyguard : MonoBehaviour
    {
        private bool _initialized = false;
        private float _currentStamina;

        [Header("-- SETUP --")]
        [SerializeField] private Animator sleepCanvasAnimator;

        #region SCRIPT REFERENCES
        private BodyguardAnimationController _animationController;
        public BodyguardAnimationController AnimationController => _animationController == null ? _animationController = GetComponent<BodyguardAnimationController>() : _animationController;
        private BodyguardStateManager _stateManager;
        public BodyguardStateManager StateManager => _stateManager == null ? _stateManager = GetComponent<BodyguardStateManager>() : _stateManager;
        private BodyguardTrigger _trigger;
        public BodyguardTrigger Trigger => _trigger == null ? _trigger = GetComponentInChildren<BodyguardTrigger>() : _trigger;
        #endregion

        #region EVENTS
        public Action OnWaitForCustomer, OnLetIn, OnWasteTime, OnGetWarned;
        #endregion

        #region PROPERTIES
        public bool IsWastingTime { get; private set; }
        #endregion

        public void Init(Gate gate)
        {
            _initialized = true;
            _currentStamina = Gate.BodyguardStamina;

            IsWastingTime = false;

            AnimationController.Init(this);
            StateManager.Init(this);
            Trigger.Init(this);

            OnLetIn += LetCustomerIn;
            OnWaitForCustomer += WaitForCustomers;
            OnWasteTime += WasteTime;
            OnGetWarned += GetWarned;
            
            GateEvents.OnSetCurrentBodyguardStamina += UpdateStamina;
            //GateEvents.OnSetCurrentBodyguardSpeed += UpdateSpeed;

            PlayerEvents.OnBodyguardIsActive?.Invoke();

            Bounce();
        }

        private void OnDisable()
        {
            if (!_initialized) return;

            OnLetIn -= LetCustomerIn;
            OnWaitForCustomer -= WaitForCustomers;
            OnWasteTime -= WasteTime;
            OnGetWarned -= GetWarned;

            GateEvents.OnSetCurrentBodyguardStamina -= UpdateStamina;
            //GateEvents.OnSetCurrentBodyguardSpeed -= UpdateSpeed;

            transform.DOKill();
        }

        private void Bounce() => transform.DOShakeScale(1f, 0.25f);

        #region EVENT HANDLER FUNCTIONS
        private void UpdateStamina() => OnGetWarned?.Invoke();
        private void LetCustomerIn()
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
            QueueManager.GateQueue.OnAiIsWorking?.Invoke();
            sleepCanvasAnimator.SetBool("Open", false);
        }
        private void WasteTime()
        {
            //IsWastingTime = true;
            QueueManager.GateQueue.OnAiIsNotWorking?.Invoke();
            sleepCanvasAnimator.SetBool("Open", true);
        }
        private void GetWarned()
        {
            IsWastingTime = false;
            _currentStamina = Gate.BodyguardStamina;
            StateManager.SwitchState(StateManager.WaitForCustomerState);
        }
        #endregion
    }
}
