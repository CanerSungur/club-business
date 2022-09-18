using UnityEngine;
using DG.Tweening;
using System;

namespace ClubBusiness
{
    public class LiquidPuddle : MonoBehaviour
    {
        private ToiletItemBreakHandler _breakHandler;

        #region FIX
        private readonly int _fixCount = 5;
        private int _currentFixedCount;
        #endregion

        #region SCALE
        private readonly Vector3 _defaultScale = new Vector3(0.9f, 1f, 0.9f);
        private Vector3 _currentScale;
        #endregion

        #region SEQUENCE
        private Sequence _scaleSequence;
        private Guid _scaleSequenceID;
        private readonly float _sequenceDuration = 0.5f;
        #endregion

        #region EVENTS
        public Action OnFix;
        #endregion

        public void Init(ToiletItemBreakHandler breakHandler)
        {
            if (_breakHandler == null)
            {
                _breakHandler = breakHandler;
            }

            EnableLiquid();
        }

        private void EnableLiquid()
        {
            _currentFixedCount = 0;
            _currentScale = Vector3.zero;
            transform.localScale = _currentScale;

            DeleteScaleSequence();
            CreateScaleSequence(_defaultScale, 3f);
            _scaleSequence.Play();
        }

        #region PUBLICS
        public void DecreasePuddleScale(int fixCount)
        {
            Vector3 targetScale = _defaultScale - (Vector3.one * (0.2f * fixCount));
            targetScale.y = 1f;

            DeleteScaleSequence();
            CreateScaleSequence(targetScale, _sequenceDuration);
            _scaleSequence.Play();
        }
        #endregion

        #region DOTWEEN FUNCTIONS
        private void CreateScaleSequence(Vector3 scale, float duration)
        {
            if (_scaleSequence == null)
            {
                _scaleSequence = DOTween.Sequence();
                _scaleSequenceID = Guid.NewGuid();
                _scaleSequence.id = _scaleSequenceID;

                _scaleSequence.Append(transform.DOScale(scale, duration)).OnComplete(() =>
                {
                    DeleteScaleSequence();
                    _currentScale = scale;
                    transform.localScale = _currentScale;
                });
            }
        }
        private void DeleteScaleSequence()
        {
            DOTween.Kill(_scaleSequenceID);
            _scaleSequence = null;
        }
        #endregion
    }
}
