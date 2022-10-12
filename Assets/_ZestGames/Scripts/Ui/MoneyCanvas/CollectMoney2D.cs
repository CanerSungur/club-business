using UnityEngine;
using DG.Tweening;
using System;

namespace ZestGames
{
    public class CollectMoney2D : MonoBehaviour
    {
        private Camera _camera;
        private RectTransform _canvasRect;

        private MoneyCanvas _moneyCanvas;
        private RectTransform _rectTransform;

        #region SEQUENCE
        private Sequence _collectSequence;
        private Guid _collectSequenceID;
        #endregion

        public void Init(MoneyCanvas moneyCanvas, Transform spawnTransform)
        {
            if (!_moneyCanvas)
            {
                _moneyCanvas = moneyCanvas;
                _camera = Camera.main;
                _canvasRect = _moneyCanvas.GetComponent<RectTransform>();
            }

            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.localScale = Vector3.one;
            _rectTransform.anchoredPosition = GetWorldPointToScreenPoint(spawnTransform);
            //_rectTransform.anchoredPosition = _moneyCanvas.MiddlePointRectTransform.anchoredPosition;

            //_rectTransform.DOAnchorPos(Hud.MoneyAnchoredPosition, 1f).OnComplete(() =>
            //{
            //    //AudioEvents.OnPlayCollectMoney?.Invoke();
            //    CollectableEvents.OnCollect?.Invoke(DataManager.MoneyValue);
            //    gameObject.SetActive(false);
            //});

            StartCollectSequence();
        }

        private Vector2 GetWorldPointToScreenPoint(Transform transform)
        {
            Vector2 viewportPosition = _camera.WorldToViewportPoint(transform.position);
            Vector2 phaseUnlockerScreenPosition = new Vector2(
               (viewportPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 1f),
               (viewportPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 1f));

            return phaseUnlockerScreenPosition;
        }

        #region DOTWEEN FUNCTIONS
        private void StartCollectSequence()
        {
            CreateCollectSequence();
            _collectSequence = null;
        }
        private void CreateCollectSequence()
        {
            if (_collectSequence == null)
            {
                _collectSequence = DOTween.Sequence();
                _collectSequenceID = Guid.NewGuid();
                _collectSequence.id = _collectSequenceID;

                _collectSequence.Append(_rectTransform.DOAnchorPos(Hud.MoneyAnchoredPosition, 1f))
                    .Join(_rectTransform.DOScale(Vector3.one * 1.2f, 1f)).OnComplete(() =>
                    {
                        AudioEvents.OnPlayCollectMoney?.Invoke();
                        CollectableEvents.OnCollect?.Invoke(DataManager.MoneyValue);
                        DeleteCollectSequence();
                        gameObject.SetActive(false);
                    });
            }
        }
        private void DeleteCollectSequence()
        {
            DOTween.Kill(_collectSequenceID);
            _collectSequence = null;
        }
        #endregion
    }
}
