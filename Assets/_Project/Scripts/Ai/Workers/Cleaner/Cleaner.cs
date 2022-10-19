using ZestGames;
using UnityEngine;
using System;
using ZestCore.Utility;
using DG.Tweening;

namespace ClubBusiness
{
    public class Cleaner : MonoBehaviour
    {
        private bool _initialized = false;
        private float _currentStamina;

        [Header("-- SETUP --")]
        [SerializeField] private Animator sleepCanvasAnimator;
        [SerializeField] private GameObject mopObj;

        #region SCRIPT REFERENCES
        private CleanerAnimationController _animationController;
        public CleanerAnimationController AnimationController => _animationController == null ? _animationController = GetComponent<CleanerAnimationController>() : _animationController;
        private CleanerStateManager _stateManager;
        public CleanerStateManager StateManager => _stateManager == null ? _stateManager = GetComponent<CleanerStateManager>() : _stateManager;
        private CleanerTrigger _trigger;
        public CleanerTrigger Trigger => _trigger == null ? _trigger = GetComponentInChildren<CleanerTrigger>() : _trigger;
        #endregion

        #region EVENTS
        public Action OnGoWaiting, OnWait, OnGoCleaning, OnStartCleaning, OnStopCleaning, OnWasteTime, OnGetWarned;
        public Action OnStartMoving, OnStopMoving;
        #endregion

        #region PROPERTIES
        public bool IsWastingTime { get; private set; }
        public bool IsCleaning { get; private set; }
        public bool IsAtWaitingPosition { get; private set; }
        public float MovementSpeed { get; private set; }
        public float RotationSpeed { get; private set; }
        #endregion

        public void Init(Toilet toilet)
        {
            _initialized = true;
            _currentStamina = Toilet.CleanerStamina;

            IsAtWaitingPosition = true;
            IsCleaning = IsWastingTime = false;
            MovementSpeed = 2f;
            RotationSpeed = 10f;
            mopObj.SetActive(false);

            AnimationController.Init(this);
            StateManager.Init(this);
            Trigger.Init(this);
            
            OnWait += Wait;
            OnGoCleaning += GoCleaning;

            OnStartCleaning += StartCleaning;
            OnStopCleaning += StopCleaning;
            OnWasteTime += WasteTime;
            OnGetWarned += GetWarned;

            ToiletEvents.OnSetCurrentCleanerStamina += UpgradeStamina;
            ToiletUpgradeEvents.OnUpgradeCleanerSpeed += UpgradeHappened;
            ToiletUpgradeEvents.OnUpgradeCleanerStamina += UpgradeHappened;

            Bounce();
        }

        private void OnDisable()
        {
            if (!_initialized) return;

            OnWait -= Wait;
            OnGoCleaning -= GoCleaning;

            OnStartCleaning -= StartCleaning;
            OnStopCleaning -= StopCleaning;
            OnWasteTime -= WasteTime;
            OnGetWarned -= GetWarned;

            ToiletEvents.OnSetCurrentCleanerStamina -= UpgradeStamina;
            ToiletUpgradeEvents.OnUpgradeCleanerSpeed -= UpgradeHappened;
            ToiletUpgradeEvents.OnUpgradeCleanerStamina -= UpgradeHappened;

            transform.DOKill();
        }

        private void Bounce() => transform.DOShakeScale(1f, 0.25f);

        #region EVENT HANDLER FUNCTIONS
        private void UpgradeHappened()
        {
            ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.LevelUp_PS, transform.position, Quaternion.Euler(-90f, 0f, 0f));
        }
        private void UpgradeStamina() => OnGetWarned?.Invoke();
        private void GoCleaning()
        {
            IsCleaning = true;
            IsAtWaitingPosition = false;
        }
        private void StartCleaning()
        {
            mopObj.SetActive(true);
        }
        private void StopCleaning()
        {
            mopObj.SetActive(false);
            IsCleaning = false;
            _currentStamina--;

            if (_currentStamina <= 0)
                IsWastingTime = true;
            else
                IsWastingTime = false;
        }
        private void Wait()
        {
            //IsWastingTime = false;
            IsAtWaitingPosition = true;
            sleepCanvasAnimator.SetBool("Open", false);
        }
        private void WasteTime()
        {
            //IsWastingTime = true;
            sleepCanvasAnimator.SetBool("Open", true);
        }
        private void GetWarned()
        {
            sleepCanvasAnimator.SetBool("Open", false);
            IsWastingTime = false;
            _currentStamina = Toilet.CleanerStamina;

            Delayer.DoActionAfterDelay(this, 2f, () => {
                if (IsAtWaitingPosition)
                    StateManager.SwitchState(StateManager.WaitState);
                else
                    StateManager.SwitchState(StateManager.CleanState);
            });
        }
        #endregion
    }
}
