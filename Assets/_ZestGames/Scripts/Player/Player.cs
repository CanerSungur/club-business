using UnityEngine;
using DG.Tweening;
using System;
using ClubBusiness;

namespace ZestGames
{
    public class Player : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private LayerMask walkableLayer;
        [SerializeField] private TimerForAction timerForAction;

        #region COMPONENTS
        private Collider _collider;
        public Collider Collider => _collider == null ? _collider = GetComponent<Collider>() : _collider;
        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody => _rigidbody == null ? _rigidbody = GetComponent<Rigidbody>() : _rigidbody;
        #endregion

        #region SCRIPT REFERENCES
        private JoystickInput _inputHandler;
        public JoystickInput InputHandler => _inputHandler == null ? _inputHandler = GetComponent<JoystickInput>() : _inputHandler;
        private IPlayerMovement _playerMovement;
        public IPlayerMovement PlayerMovement => _playerMovement == null ? _playerMovement = GetComponent<IPlayerMovement>() : _playerMovement;
        private PlayerAnimationController _animationController;
        public PlayerAnimationController AnimationController => _animationController == null ? _animationController = GetComponent<PlayerAnimationController>() : _animationController;
        private PlayerCollision _collisionHandler;
        public PlayerCollision CollisionHandler => _collisionHandler == null ? _collisionHandler = GetComponent<PlayerCollision>() : _collisionHandler;
        private PlayerMoneyHandler _moneyHandler;
        public PlayerMoneyHandler MoneyHandler => _moneyHandler == null ? _moneyHandler = GetComponent<PlayerMoneyHandler>() : _moneyHandler;
        private PlayerAudio _audioHandler;
        public PlayerAudio AudioHandler => _audioHandler == null ? _audioHandler = GetComponent<PlayerAudio>() : _audioHandler;
        private PlayerStateController _stateController;
        public PlayerStateController StateController => _stateController == null ? _stateController = GetComponent<PlayerStateController>() : _stateController;
        private PlayerDrinkThrower _drinkThrower;
        public PlayerDrinkThrower DrinkThrower => _drinkThrower == null ? _drinkThrower = GetComponent<PlayerDrinkThrower>() : _drinkThrower;
        #endregion

        #region PROPERTIES
        public ToiletItem FixedToiletItem { get; set; }
        public bool IsDead { get; private set; }
        public bool IsUpgrading { get; private set; }
        public bool IsGrounded => Physics.Raycast(Collider.bounds.center, Vector3.down, Collider.bounds.extents.y + 0.01f, walkableLayer);
        public TimerForAction TimerForAction => timerForAction;
        #endregion

        #region SEQUENCE
        private Sequence _upgradeRotationSequence;
        private Guid _upgradeRotationSequenceID;
        private float _upgradeRotationDuration = 1f;
        #endregion

        private void Start()
        {
            CharacterTracker.SetPlayerTransform(transform);
            IsDead = IsUpgrading = false;
            FixedToiletItem = null;

            InputHandler.Init(this);
            PlayerMovement.Init(this);
            AnimationController.Init(this);
            CollisionHandler.Init(this);
            MoneyHandler.Init(this);
            AudioHandler.Init(this);
            StateController.Init(this);
            DrinkThrower.Init(this);
            timerForAction.Init();

            PlayerUpgradeEvents.OnOpenCanvas += HandleUpgradeStart;
            PlayerUpgradeEvents.OnCloseCanvas += HandleUpgradeEnd;
        }

        private void OnDisable()
        {
            PlayerUpgradeEvents.OnOpenCanvas -= HandleUpgradeStart;
            PlayerUpgradeEvents.OnCloseCanvas -= HandleUpgradeEnd;
        }

        #region EVENT HANDLER FUNCTIONS
        private void HandleUpgradeStart()
        {
            IsUpgrading = true;
            StartUpgradeRotationSequence();
        }
        private void HandleUpgradeEnd()
        {
            IsUpgrading = false;
            DeleteUpgradeRotationSequence();
        }
        #endregion

        private void StartUpgradeRotationSequence()
        {
            DeleteUpgradeRotationSequence();
            CreateUpgradeRotationSequence();
            _upgradeRotationSequence.Play();
        }

        #region DOTWEEN FUNCTIONS
        private void CreateUpgradeRotationSequence()
        {
            if (_upgradeRotationSequence == null)
            {
                _upgradeRotationSequence = DOTween.Sequence();
                _upgradeRotationSequenceID = Guid.NewGuid();
                _upgradeRotationSequence.id = _upgradeRotationSequenceID;

                _upgradeRotationSequence.Append(transform.DORotate(new Vector3(0f, 180f, 0f), _upgradeRotationDuration)).OnComplete(() =>
                {
                    DeleteUpgradeRotationSequence();
                });
            }
        }
        private void DeleteUpgradeRotationSequence()
        {
            DOTween.Kill(_upgradeRotationSequenceID);
            _upgradeRotationSequence = null;
        }
        #endregion
    }
}
