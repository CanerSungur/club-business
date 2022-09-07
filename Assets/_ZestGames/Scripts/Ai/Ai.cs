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
        //private IAiMovement movement;
        //public IAiMovement Movement => movement == null ? movement = GetComponent<IAiMovement>() : movement;
        #endregion

        [Header("-- MOVEMENT SETUP --")]
        [SerializeField] private float runSpeed = 3f;
        [SerializeField] private float walkSpeed = 1f;
        private float _currentMovementSpeed;

        [Header("-- GROUNDED SETUP --")]
        [SerializeField, Tooltip("Select layers that you want this object to be grounded.")] private LayerMask groundLayerMask;
        [SerializeField, Tooltip("Height that this object will be considered grounded when above groundable layers.")] private float groundedHeightLimit = 0.05f;

        private Enums.AiLocation _currentLocation;

        #region CONTROLS
        public bool CanMove => GameManager.GameState == Enums.GameState.Started;
        public bool IsGrounded => Physics.Raycast(Collider.bounds.center, Vector3.down, Collider.bounds.extents.y + groundedHeightLimit, groundLayerMask);
        #endregion

        #region PROPERTIES
        public bool IsDead { get; private set; }
        public Transform Target { get; private set; }
        public bool IsInQueue { get; private set; }
        public bool QueueIsUp { get; set; }
        public bool CanGetIntoQueue { get; set; }
        #endregion

        #region GETTERS
        public float WalkSpeed => walkSpeed;
        public float RunSpeed => runSpeed;
        public float CurrentMovementSpeed => _currentMovementSpeed;
        public Enums.AiLocation CurrentLocation => _currentLocation;
        #endregion

        #region EVENTS
        public Action OnIdle, OnMove, OnDie, OnWin, OnLose;
        public Action<Transform> OnSetTarget;
        #endregion

        public void Init(CustomerManager customerManager, Enums.AiLocation location)
        {
            _currentLocation = location;
            if (_currentLocation == Enums.AiLocation.Outside)
            {
                CustomerManager.AddCustomerOutside(this);
                CanGetIntoQueue = true;
            }
            else if (_currentLocation == Enums.AiLocation.Inside)
            {
                CustomerManager.AddCustomerInside(this);
                CanGetIntoQueue = false;
            }

            IsDead = IsInQueue = false;
            Target = null;

            CharacterTracker.AddAi(this);

            StateManager.Init(this);
            AnimationController.Init(this);
            //Movement.Init(this);

            OnSetTarget += SetTarget;
            OnDie += Die;
        }

        private void OnDisable()
        {
            CharacterTracker.RemoveAi(this);
            CustomerManager.RemoveCustomerInside(this);
            CustomerManager.RemoveCustomerOutside(this);

            OnSetTarget -= SetTarget;
            OnDie -= Die;
        }
        private void Die()
        {
            IsDead = true;
            OnDie?.Invoke();
            CharacterTracker.RemoveAi(this);
            Delayer.DoActionAfterDelay(this, 5f, () => gameObject.SetActive(false));
        }

        public void SetTarget(Transform transform)
        {
            if (!CanMove) return;

            Target = transform;
            OnMove?.Invoke();
        }
    }
}
