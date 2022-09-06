using UnityEngine;

namespace ZestGames
{
    public class SpendMoney2D : MonoBehaviour
    {
        private MoneyCanvas _moneyCanvas;
        private RectTransform _canvasRect;
        private RectTransform _rectTransform;
        private Camera _camera;
        private Vector2 _currentPosition;
        private Transform _targetTransform = null;
        private float _disableTime;

        public void Init(MoneyCanvas moneyCanvas, Transform targetTransform)
        {
            if (!_moneyCanvas)
            {
                _moneyCanvas = moneyCanvas;
                _canvasRect = _moneyCanvas.GetComponent<RectTransform>();
                _rectTransform = GetComponent<RectTransform>();
                _camera = Camera.main;
            }

            _targetTransform = targetTransform;
            _disableTime = Time.time + 0.9f;
            _currentPosition = Hud.MoneyAnchoredPosition;
            _rectTransform.anchoredPosition = _currentPosition;
        }

        private void OnDisable()
        {
            _targetTransform = null;
        }

        private void Update()
        {
            if (_targetTransform)
            {
                Vector2 travel = GetWorldPointToScreenPoint(_targetTransform) - _rectTransform.anchoredPosition;
                _rectTransform.Translate(travel * 10f * Time.deltaTime, _camera.transform);


                if (Vector2.Distance(_rectTransform.anchoredPosition, GetWorldPointToScreenPoint(_targetTransform)) < 25f)
                {
                    gameObject.SetActive(false);
                }

                if (Time.time >= _disableTime)
                    gameObject.SetActive(false);
            }
        }

        private Vector2 GetWorldPointToScreenPoint(Transform transform)
        {
            Vector2 viewportPosition = _camera.WorldToViewportPoint(transform.position);
            Vector2 phaseUnlockerScreenPosition = new Vector2(
               (viewportPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 1f),
               (viewportPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 1f));

            return phaseUnlockerScreenPosition;
        }
    }
}
