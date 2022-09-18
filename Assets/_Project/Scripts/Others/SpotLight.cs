using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

namespace ClubBusiness
{
    public class SpotLight : MonoBehaviour
    {
        private Transform _spotLightHeadTransform;

        #region ROTATION LIMITS
        private readonly Quaternion _defaultRotation = Quaternion.Euler(0f, 180f, 0f);
        private readonly Vector2 _xAxisLimits = new Vector2(-75f, 75f);
        private readonly Vector2 _yAxisLimits = new Vector2(120f, 240f);
        private readonly Vector2 _zAxisLimits = new Vector2(-30f, 20f);
        #endregion

        #region DURATION LIMITS
        private readonly float _minRotationDuration = 1f;
        private readonly float _maxRotationDuration = 3f;
        #endregion

        #region SEQUENCE
        private Sequence _randomRotationSequence;
        private Guid _randomRotationSequenceID;
        #endregion

        private void Start()
        {
            _spotLightHeadTransform = transform.GetChild(0);
            _spotLightHeadTransform.localRotation = _defaultRotation;
            StartRandomRotationSequence();
        }

        private void OnDisable()
        {
            DeleteRandomRotationSequence();
        }

        #region DOTWEEN FUNCTIONS
        private void StartRandomRotationSequence()
        {
            CreateRandomRotationSequence();
            _randomRotationSequence.Play();
        }
        private void CreateRandomRotationSequence()
        {
            if (_randomRotationSequence == null)
            {
                _randomRotationSequence = DOTween.Sequence();
                _randomRotationSequenceID = Guid.NewGuid();
                _randomRotationSequence.id = _randomRotationSequenceID;

                Vector3 randomRotation = new Vector3(Random.Range(_xAxisLimits.x, _xAxisLimits.y), Random.Range(_yAxisLimits.x, _yAxisLimits.y), Random.Range(_zAxisLimits.x, _zAxisLimits.y));
                float randomDuration = Random.Range(_minRotationDuration, _maxRotationDuration);

                _randomRotationSequence.Append(_spotLightHeadTransform.DOLocalRotate(randomRotation, randomDuration)).OnComplete(() =>
                {
                    DeleteRandomRotationSequence();
                    StartRandomRotationSequence();
                });
            }
        }
        private void DeleteRandomRotationSequence()
        {
            DOTween.Kill(_randomRotationSequenceID);
            _randomRotationSequence = null;
        }
        #endregion
    }
}
