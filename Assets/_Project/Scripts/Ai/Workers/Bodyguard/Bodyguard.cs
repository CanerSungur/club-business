using System;
using UnityEngine;

namespace ClubBusiness
{
    public class Bodyguard : MonoBehaviour
    {
        private bool _initialized = false;

        #region SCRIPT REFERENCES
        private BodyguardAnimationController _animationController;
        public BodyguardAnimationController AnimationController => _animationController == null ? _animationController = GetComponent<BodyguardAnimationController>() : _animationController;
        private BodyguardStateManager _stateManager;
        public BodyguardStateManager StateManager => _stateManager == null ? _stateManager = GetComponent<BodyguardStateManager>() : _stateManager;
        #endregion

        #region EVENTS
        public Action OnWaitForCustomer, OnLetIn, OnWasteTime, OnGetWarned;
        #endregion

        #region PROPERTIES
        public bool IsWastingTime { get; private set; }
        #endregion

        public void Init()
        {
            _initialized = true;

            IsWastingTime = false;

            AnimationController.Init(this);
            StateManager.Init(this);

            OnWaitForCustomer += WaitForCustomers;
            OnWasteTime += WasteTime;
            OnGetWarned += GetWarned;
        }

        private void OnDisable()
        {
            if (!_initialized) return;

            OnWaitForCustomer -= WaitForCustomers;
            OnWasteTime -= WasteTime;
            OnGetWarned -= GetWarned;
        }

        #region EVENT HANDLER FUNCTIONS
        private void WaitForCustomers()
        {
            IsWastingTime = false;
        }
        private void WasteTime()
        {
            IsWastingTime = true;
        }
        private void GetWarned()
        {
            //StateManager.SwitchState(StateManager.WaitForCustomerState);
        }
        #endregion
    }
}
