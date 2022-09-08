using UnityEngine;
using ZestGames;
using DG.Tweening;
using System;

namespace ClubBusiness
{
    public class PlayerStateController : MonoBehaviour
    {
        private Player _player;
        private Transform _playerMeshTransform;
        private bool _startRotating;
        private Quaternion _targetRotation;

        #region SEQUENCE
        private Sequence _rotateSequence;
        private Guid _rotateSequenceID;
        private readonly float _rotateDuration = 1.5f;
        #endregion

        public void Init(Player player)
        {
            if (_player == null)
            {
                _player = player;
                _playerMeshTransform = transform.GetChild(0);
            }

            _startRotating = false;

            PlayerEvents.OnStartLettingPeopleIn += StartLettingPeopleIn;
            PlayerEvents.OnStopLettingPeopleIn += StopLettingPeopleIn;
            PlayerEvents.OnStartFillingDrinks += StartFillingDrinks;
            PlayerEvents.OnStopFillingDrinks += StopFillingDrinks;
        }

        private void OnDisable()
        {
            if (_player == null) return;

            PlayerEvents.OnStartLettingPeopleIn -= StartLettingPeopleIn;
            PlayerEvents.OnStopLettingPeopleIn -= StopLettingPeopleIn;
            PlayerEvents.OnStartFillingDrinks -= StartFillingDrinks;
            PlayerEvents.OnStopFillingDrinks -= StopFillingDrinks;
        }

        private void Update()
        {
            if (_startRotating)
            {
                _playerMeshTransform.rotation = Quaternion.Lerp(_playerMeshTransform.rotation, _targetRotation , 2f * Time.deltaTime);
            }
        }

        #region EVENT HANDLER FUNCTIONS
        private void StartLettingPeopleIn()
        {
            _startRotating = true;
            _targetRotation = Quaternion.Euler(0f, 90f, 0f);
            //StartRotateSequence(new Vector3(0f, 90f, 0f), _rotateDuration);
        }
        private void StopLettingPeopleIn()
        {
            _startRotating = false;
            StartRotateSequence(Vector3.zero, _rotateDuration * 0.25f);
        }
        private void StartFillingDrinks()
        {
            _startRotating = true;
            _targetRotation = Quaternion.Euler(0f, 90f, 0f);
        }
        private void StopFillingDrinks()
        {
            _startRotating = false;
            StartRotateSequence(Vector3.zero, _rotateDuration * 0.25f);
        }
        #endregion

        #region DOTWEEN FUNCTIONS
        private void StartRotateSequence(Vector3 rotation, float duration)
        {
            _rotateSequence.Pause();
            DeleteRotateSequence();
            CreateRotateSequence(rotation, duration);
            _rotateSequence.Play();
        }
        private void CreateRotateSequence(Vector3 rotation, float duration)
        {
            if (_rotateSequence == null)
            {
                _rotateSequence = DOTween.Sequence();
                _rotateSequenceID = Guid.NewGuid();
                _rotateSequence.id = _rotateSequenceID;

                _rotateSequence.Append(_playerMeshTransform.DOLocalRotate(rotation, duration)).OnComplete(() => {
                    DeleteRotateSequence();
                });
            }
        }
        private void DeleteRotateSequence()
        {
            DOTween.Kill(_rotateSequenceID);
            _rotateSequence = null;
        }
        #endregion
    }
}