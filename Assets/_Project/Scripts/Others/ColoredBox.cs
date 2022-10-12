using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using ZestGames;

namespace ClubBusiness
{
    public class ColoredBox : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private List<Ai> _aisOnThis = new List<Ai>();
        private bool _playerIsOnThis;

        [Header("-- SETUP --")]
        [SerializeField] private Material[] lightMaterials;

        private readonly Color _lightOff = new Color(1f, 1f, 1f, 0.001f);
        private readonly Color _lightOn = new Color(1f, 1f, 1f, 1f);
        private readonly float _colorChangeDuration = 1f;
        private Color _currentColor;

        private Sequence _lightOnSequence, _lightOffSequence;
        private Guid _lightOnSequenceID, _lightOffSequenceID;

        public void Init(ColoredBoxController coloredBoxController)
        {
            _aisOnThis.Clear();
            _playerIsOnThis = false;
            transform.SetParent(coloredBoxController.transform);
            _meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
            _meshRenderer.material = lightMaterials[Random.Range(0, lightMaterials.Length)];
            _meshRenderer.material.color = _lightOff;
            _currentColor = _lightOff;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Ai ai) && !_aisOnThis.Contains(ai))
            {
                _aisOnThis.Add(ai);

                if (!_playerIsOnThis)
                    StartLightOnSequence();
            }

            if (other.TryGetComponent(out Player player) && !_playerIsOnThis)
            {
                _playerIsOnThis = true;

                if (_aisOnThis.Count == 0)
                    StartLightOnSequence();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Ai ai) && _aisOnThis.Contains(ai))
            {
                _aisOnThis.Remove(ai);

                if (_aisOnThis.Count == 0 && !_playerIsOnThis)
                    StartLightOffSequence();
            }

            if (other.TryGetComponent(out Player player) && _playerIsOnThis)
            {
                _playerIsOnThis = false;

                if (_aisOnThis.Count == 0 && !_playerIsOnThis)
                {
                    DeleteLightOnSequence();
                    StartLightOffSequence();
                }
            }
        }

        #region DOTWEEN FUNCTIONS
        private void StartLightOnSequence()
        {
            if (_lightOnSequence != null && _lightOnSequence.IsPlaying()) return;

            DeleteLightOffSequence();
            CreateLightOnSequence();
            _lightOnSequence.Play();
        }
        private void CreateLightOnSequence()
        {
            if (_lightOnSequence == null)
            {
                _lightOnSequence = DOTween.Sequence();
                _lightOnSequenceID = Guid.NewGuid();
                _lightOnSequence.id = _lightOnSequenceID;

                _lightOnSequence.Append(DOVirtual.Color(_currentColor, _lightOn, _colorChangeDuration, r =>
                {
                    _currentColor = r;
                    _meshRenderer.material.color = _currentColor;
                })).OnComplete(() =>
                {
                    DeleteLightOnSequence();
                });
            }
        }
        private void DeleteLightOnSequence()
        {
            DOTween.Kill(_lightOnSequenceID);
            _lightOnSequence = null;
        }
        // #######################
        private void StartLightOffSequence()
        {
            if (_lightOffSequence != null && _lightOffSequence.IsPlaying()) return;

            DeleteLightOnSequence();
            CreateLightOffSequence();
            _lightOffSequence.Play();
        }
        private void CreateLightOffSequence()
        {
            if (_lightOffSequence == null)
            {
                _lightOffSequence = DOTween.Sequence();
                _lightOffSequenceID = Guid.NewGuid();
                _lightOffSequence.id = _lightOffSequenceID;

                _lightOffSequence.Append(DOVirtual.Color(_currentColor, _lightOff, _colorChangeDuration * 4f, r =>
                {
                    _currentColor = r;
                    _meshRenderer.material.color = _currentColor;
                })).OnComplete(() =>
                {
                    DeleteLightOffSequence();
                });
            }
        }
        private void DeleteLightOffSequence()
        {
            DOTween.Kill(_lightOffSequenceID);
            _lightOffSequence = null;
        }
        #endregion
    }
}
