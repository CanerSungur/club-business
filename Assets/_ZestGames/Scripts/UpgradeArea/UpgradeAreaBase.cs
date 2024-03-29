using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace ZestGames
{
    public abstract class UpgradeAreaBase : MonoBehaviour
    {
        private Image _fillImage;
        private readonly float _openingTime = 2f;

        public bool PlayerIsInArea { get; private set; }

        private Sequence _fillSequence, _emptySequence;
        private Guid _fillSequenceID, _emptySequenceID;

        private void OnEnable()
        {
            _fillImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            _fillImage.fillAmount = 0f;
            PlayerIsInArea = false;
        }

        public abstract void OpenUpgradeCanvas();
        public abstract void CloseUpgradeCanvas();

        #region DOTWEEN FUNCTIONS
        private void CreateFillSequence()
        {
            if (_fillSequence == null)
            {
                _fillSequence = DOTween.Sequence();
                _fillSequence.Append(DOVirtual.Float(_fillImage.fillAmount, 1f, _openingTime, r =>
                {
                    _fillImage.fillAmount = r;
                }).OnComplete(OpenUpgradeCanvas));

                _fillSequenceID = Guid.NewGuid();
                _fillSequence.id = _fillSequenceID;
            }
        }
        private void CreateEmptySequence()
        {
            if (_emptySequence == null)
            {
                _emptySequence = DOTween.Sequence();
                _emptySequence.Append(DOVirtual.Float(_fillImage.fillAmount, 0f, _openingTime * 0.5f, r => {
                    _fillImage.fillAmount = r;
                }));

                _emptySequenceID = Guid.NewGuid();
                _emptySequence.id = _emptySequenceID;
            }
        }
        private void StopFillSequence()
        {
            DOTween.Kill(_fillSequenceID);
            _fillSequence = null;
        }
        private void StopEmptySequence()
        {
            DOTween.Kill(_emptySequenceID);
            _emptySequence = null;
        }
        #endregion

        #region PUBLICS
        public void StartOpening()
        {
            PlayerIsInArea = true;

            StopEmptySequence();

            CreateFillSequence();
            _fillSequence.Play();
        }
        public void StopOpening()
        {
            PlayerIsInArea = false;
            CloseUpgradeCanvas();

            StopFillSequence();

            CreateEmptySequence();
            _emptySequence.Play();
        }
        #endregion
    }
}
