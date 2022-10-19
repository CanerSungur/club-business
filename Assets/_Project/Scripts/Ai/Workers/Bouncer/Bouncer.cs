using System;
using UnityEngine;
using ZestGames;
using DG.Tweening;
using ZestCore.Utility;

namespace ClubBusiness
{
    public class Bouncer : MonoBehaviour
    {
        private bool _initialized = false;
        private float _currentStamina;

        [Header("-- SETUP --")]
        [SerializeField] private Animator sleepCanvasAnimator;

        #region SCRIPT REFERENCES
        private BouncerAnimationController _animationController;
        public BouncerAnimationController AnimationController => _animationController == null ? _animationController = GetComponent<BouncerAnimationController>() : _animationController;
        private BouncerStateManager _stateManager;
        public BouncerStateManager StateManager => _stateManager == null ? _stateManager = GetComponent<BouncerStateManager>() : _stateManager;
        private BouncerTrigger _trigger;
        public BouncerTrigger Trigger => _trigger == null ? _trigger = GetComponentInChildren<BouncerTrigger>() : _trigger;
        #endregion

        #region EVENTS
        public Action OnWaitForFight, OnGoWaitingForFight, OnGoBreakingFight, OnBreakFight, OnWasteTime, OnGetWarned;
        #endregion

        #region PROPERTIES
        public bool IsWastingTime { get; private set; }
        public bool IsAtWaitingPosition { get; private set; }
        public float MovementSpeed { get; private set; }
        #endregion

        public void Init(DanceFloor danceFloor)
        {
            _initialized = true;
            _currentStamina = Gate.BodyguardStamina;

            IsAtWaitingPosition = true;
            IsWastingTime = false;
            MovementSpeed = 2f;

            AnimationController.Init(this);
            StateManager.Init(this);
            Trigger.Init(this);

            OnWaitForFight += WaitForFight;
            OnGoBreakingFight += GoBreakingFight;

            OnBreakFight += BreakFight;
            OnWasteTime += WasteTime;
            OnGetWarned += GetWarned;

            DanceFloorEvents.OnSetCurrentBouncerStamina += UpdateStamina;
            DanceFloorUpgradeEvents.OnUpgradeBouncerPower += UpgradeHappened;
            DanceFloorUpgradeEvents.OnUpgradeBouncerStamina += UpgradeHappened;

            Bounce();
        }

        private void OnDisable()
        {
            if (!_initialized) return;
            OnWaitForFight -= WaitForFight;
            OnGoBreakingFight -= GoBreakingFight;

            OnBreakFight -= BreakFight;
            OnWasteTime -= WasteTime;
            OnGetWarned -= GetWarned;

            DanceFloorEvents.OnSetCurrentBouncerStamina -= UpdateStamina;
            DanceFloorUpgradeEvents.OnUpgradeBouncerPower -= UpgradeHappened;
            DanceFloorUpgradeEvents.OnUpgradeBouncerStamina -= UpgradeHappened;

            transform.DOKill();
        }

        private void Bounce() => transform.DOShakeScale(1f, 0.25f);

        #region EVENT HANDLER FUNCTIONS
        private void UpgradeHappened()
        {
            ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.LevelUp_PS, transform.position, Quaternion.Euler(-90f, 0f, 0f));
        }
        private void UpdateStamina() => OnGetWarned?.Invoke();
        private void GoBreakingFight()
        {
            IsAtWaitingPosition = false;
        }
        private void BreakFight()
        {
            _currentStamina--;

            if (_currentStamina <= 0)
                IsWastingTime = true;
            else
                IsWastingTime = false;
        }
        private void WaitForFight()
        {
            IsAtWaitingPosition = true;
            sleepCanvasAnimator.SetBool("Open", false);
        }
        private void WasteTime()
        {
            sleepCanvasAnimator.SetBool("Open", true);
        }
        private void GetWarned()
        {
            sleepCanvasAnimator.SetBool("Open", false);
            IsWastingTime = false;
            _currentStamina = DanceFloor.BouncerStamina;

            //Delayer.DoActionAfterDelay(this, 2f, () => {
            //    if (IsAtWaitingPosition)
            //        StateManager.SwitchState(StateManager.WaitForFightState);
            //    else
            //        StateManager.SwitchState(StateManager.BreakFightState);
            //});
        }
        #endregion
    }
}
