using UnityEngine;
using DG.Tweening;
using System;
using ZestGames;

namespace ClubBusiness
{
    public class Beer : MonoBehaviour
    {
        private Transform _firstInitTransform;
        private Ai _targetAi;

        #region SEQUENCE
        private Sequence _throwSequence;
        private Guid _throwSequenceID;
        #endregion

        public void Init(Ai targetAi)
        {
            transform.position = Bar.BeerSpawnTransform.position;
            _targetAi = targetAi;
            StartThrowSequence(_targetAi);
        }

        private void OnDisable()
        {
            _targetAi = null;
        }

        #region DOTWEEN FUNCTIONS
        private void StartThrowSequence(Ai ai)
        {
            CreateThrowSequence(ai);
            _throwSequence.Play();
        }
        private void CreateThrowSequence(Ai ai)
        {
            if (_throwSequence == null)
            {
                _throwSequence = DOTween.Sequence();
                _throwSequenceID = Guid.NewGuid();
                _throwSequence.id = _throwSequenceID;

                _throwSequence.Append(transform.DOJump(ai.transform.position + Vector3.up, 2, 1, 1f))
                    .Join(transform.DOShakeScale(1f, 0.75f)).OnComplete(() => {
                        DeleteThrowSequence();
                        PlayerEvents.OnEmptyNextInBarQueue?.Invoke(ai);
                        gameObject.SetActive(false);
                    });
            }
        }
        private void DeleteThrowSequence()
        {
            DOTween.Kill(_throwSequenceID);
            _throwSequence = null;
        }
        #endregion
    }
}
