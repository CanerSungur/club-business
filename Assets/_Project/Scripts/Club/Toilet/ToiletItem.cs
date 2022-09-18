using UnityEngine;
using System;
using ZestGames;

namespace ClubBusiness
{
    public abstract class ToiletItem : MonoBehaviour
    {
        private ToiletItemBreakHandler _breakHandler;
        private ToiletCabinDoor _toiletCabinDoor;
        private Collider _collider;

        private int _usedCount;
        private readonly int _breakLimit = 2;

        #region PROPERTIES
        public Transform PointTransform { get; private set; }
        public bool PlayerIsInArea { get; set; }
        public bool IsBroken { get; private set; }
        #endregion

        #region EVENTS
        public Action OnBreak, OnFix, OnFixCompleted, OnCustomerExit;
        #endregion

        private void OnEnable()
        {
            if (_breakHandler == null)
            {
                if (transform.GetChild(transform.childCount - 1).TryGetComponent(out _toiletCabinDoor))
                    _toiletCabinDoor.Init(this);

                _collider = GetComponent<Collider>();
                _breakHandler = GetComponent<ToiletItemBreakHandler>();
                _breakHandler.Init(this);
            }

            _usedCount = 0;
            PlayerIsInArea = IsBroken = _collider.enabled = false;
            PointTransform = transform.GetChild(0).GetChild(0);
            Toilet.AddEmptyToiletItem(this);

            OnFixCompleted += FixCompleted;
        }

        private void OnDisable()
        {
            OnFixCompleted -= FixCompleted;
        }

        private void Break()
        {
            if (!IsBroken)
            {
                IsBroken = _collider.enabled = true;
                OnBreak?.Invoke();
            }
        }
        private void FixCompleted()
        {
            PlayerEvents.OnStopFixingToilet?.Invoke();
            PlayerIsInArea = IsBroken = _collider.enabled = false;
            _usedCount = 0;
            Toilet.AddEmptyToiletItem(this);
        }

        #region PUBLICS
        public void Occupy()
        {
            Toilet.RemoveEmptyToiletItem(this);
        }
        public void Release()
        {
            _usedCount++;
            if (_usedCount >= _breakLimit)
                Break();

            if (!IsBroken)
                Toilet.AddEmptyToiletItem(this);
        }
        #endregion
    }
}
