using UnityEngine;
using System;
using ZestCore.Utility;
using ClubBusiness;

namespace ZestGames
{
    public class Ai : MonoBehaviour
    {
        #region COMPONENTS
        private Collider coll;
        public Collider Collider => coll == null ? coll = GetComponent<Collider>() : coll;
        private Rigidbody rb;
        public Rigidbody Rigidbody => rb == null ? rb = GetComponent<Rigidbody>() : rb;
        #endregion

        #region SCRIPT REFERENCES
        private AiStateManager _stateManager;
        public AiStateManager StateManager => _stateManager == null ? _stateManager = GetComponent<AiStateManager>() : _stateManager;
        private AiAnimationController animationController;
        public AiAnimationController AnimationController => animationController == null ? animationController = GetComponent<AiAnimationController>() : animationController;
        private AiCollision collision;
        public AiCollision Collision => collision == null ? collision = GetComponent<AiCollision>() : collision;
        private AiAppearanceHandler _appereanceHandler;
        public AiAppearanceHandler AppereanceHandler => _appereanceHandler == null ? _appereanceHandler = GetComponent<AiAppearanceHandler>() : _appereanceHandler;
        private AiEffectHandler _effectHandler;
        public AiEffectHandler EffectHandler => _effectHandler == null ? _effectHandler = GetComponent<AiEffectHandler>() : _effectHandler;
        private AiAngerHandler _angerHandler;
        public AiAngerHandler AngerHandler => _angerHandler == null ? _angerHandler = GetComponent<AiAngerHandler>() : _angerHandler;
        private ReactionCanvas _reactionCanvas;
        public ReactionCanvas ReactionCanvas => _reactionCanvas == null ? _reactionCanvas = GetComponentInChildren<ReactionCanvas>() : _reactionCanvas;
        private AiMoneyHandler _moneyHandler;
        public AiMoneyHandler MoneyHandler => _moneyHandler == null ? _moneyHandler = GetComponent<AiMoneyHandler>() : _moneyHandler;
        private AiTrigger _trigger;
        public AiTrigger Trigger => _trigger == null ? _trigger = GetComponentInChildren<AiTrigger>() : _trigger;
        #endregion

        [Header("-- SETUP --")]
        [SerializeField] private Enums.AiGender gender;
        private Enums.AiLocation _currentLocation;

        [Header("-- MOVEMENT SETUP --")]
        [SerializeField] private float runSpeed = 3f;
        [SerializeField] private float walkSpeed = 1f;
        private float _currentMovementSpeed;

        [Header("-- GROUNDED SETUP --")]
        [SerializeField, Tooltip("Select layers that you want this object to be grounded.")] private LayerMask groundLayerMask;
        [SerializeField, Tooltip("Height that this object will be considered grounded when above groundable layers.")] private float groundedHeightLimit = 0.05f;

        #region CONTROLS
        public bool CanMove => GameManager.GameState == Enums.GameState.Started;
        public bool IsGrounded => Physics.Raycast(Collider.bounds.center, Vector3.down, Collider.bounds.extents.y + groundedHeightLimit, groundLayerMask);
        #endregion

        #region PROPERTIES
        public bool IsLeaving { get; set; }
        public bool IsFighting { get; private set; }
        public bool IsDead { get; private set; }
        public Transform Target { get; private set; }
        public bool IsDancing { get; private set; }
        public bool NeedDrink { get; private set; }
        public bool NeedToPiss { get; private set; }
        public bool NeedDancing { get; private set; }
        #endregion

        #region GETTERS
        public float WalkSpeed => walkSpeed;
        public float RunSpeed => runSpeed;
        public float CurrentMovementSpeed => _currentMovementSpeed;
        public Enums.AiLocation CurrentLocation => _currentLocation;
        public Enums.AiGender Gender => gender;
        #endregion

        #region EVENTS
        public Action OnIdle, OnMove, OnDie, OnWin, OnLose, OnStartDancing, OnStopDancing, OnDrink, OnStartAskingForDrink, OnStopAskingForDrink, OnStartPissing, OnStopPissing, OnStartWaitingForToilet, OnStopWaitingForToilet, OnStandUp;
        public Action OnStopArguing, OnStartFighting, OnStopFighting, OnGetKnockedOut, OnGetUp;
        public Action<Enums.AiStateType> OnStartArguing;
        public Action<Enums.AiMood> OnMoodChange;
        public Action<Transform> OnSetTarget;
        #endregion

        public void Init(CustomerManager customerManager, Enums.AiLocation location)
        {
            _currentLocation = location;
            if (_currentLocation == Enums.AiLocation.Outside)
            {
                CustomerManager.AddCustomerOutside(this);
            }
            else if (_currentLocation == Enums.AiLocation.Inside)
            {
                CustomerManager.AddCustomerInside(this);
            }

            IsLeaving = IsDead = IsDancing = NeedDrink = NeedToPiss = false;
            Target = null;

            CharacterTracker.AddAi(this);

            StateManager.Init(this);
            AnimationController.Init(this);
            AppereanceHandler.Init(this);
            EffectHandler.Init(this);
            AngerHandler.Init(this);
            ReactionCanvas.Init(this);
            MoneyHandler.Init(this);
            Trigger.Init(this);

            OnSetTarget += SetTarget;
            OnStartDancing += StartDancing;
            OnStopDancing += StopDancing;
            OnDie += Die;

            OnDrink += Drink;
            OnStartPissing += GetIntoToilet;
            OnStopPissing += GetOutOfToilet;
            OnStartFighting += StartFighting;
            OnStopFighting += StopFighting;
        }

        private void OnDisable()
        {
            CharacterTracker.RemoveAi(this);
            CustomerManager.RemoveCustomerInside(this);
            CustomerManager.RemoveCustomerOutside(this);

            OnSetTarget -= SetTarget;
            OnStartDancing -= StartDancing;
            OnStopDancing -= StopDancing;
            OnDie -= Die;

            OnDrink -= Drink;
            OnStartPissing -= GetIntoToilet;
            OnStopPissing -= GetOutOfToilet;
            OnStartFighting -= StartFighting;
            OnStopFighting -= StopFighting;
        }

        #region EVENT HANDLER FUNCTIONS
        private void StartFighting()
        {
            IsFighting = true;
        }
        private void StopFighting()
        {
            IsFighting = false;
        }
        private void Drink()
        {
            NeedToPiss = true;
            NeedDrink = false;
        }
        private void GetIntoToilet()
        {
            NeedToPiss = false;
        }
        private void GetOutOfToilet()
        {
            NeedDancing = true;
        }
        private void Die()
        {
            IsDead = true;
            OnDie?.Invoke();
            CharacterTracker.RemoveAi(this);
            Delayer.DoActionAfterDelay(this, 5f, () => gameObject.SetActive(false));
        }
        private void StartDancing()
        {
            IsDancing = true;
            NeedDancing = false;
        }
        private void StopDancing()
        {
            IsDancing = false;
            NeedDrink = true;
        }
        #endregion

        #region PUBLICS
        public void SetTarget(Transform transform)
        {
            if (!CanMove) return;

            Target = transform;
            OnMove?.Invoke();
        }
        public void BarTurnIsUp()
        {
            StateManager.BuyDrinkState.TurnIsUp();
        }
        public void LeftClub()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}
