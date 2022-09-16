using UnityEngine;
using ZestGames;
using DG.Tweening;
using System;

namespace ClubBusiness
{
    public class PlayerDrinkThrower : MonoBehaviour
    {
        private Player _player;
        private Ai _targetAi;

        #region SEQUENCE
        private Sequence _throwSequence;
        private Guid _throwSequenceID;
        #endregion

        public void Init(Player player)
        {
            _player = player;

            PlayerEvents.OnThrowADrink += ThrowADrink;
        }

        private void OnDisable()
        {
            if (_player == null) return;
            PlayerEvents.OnThrowADrink -= ThrowADrink;
        }

        private void ThrowADrink(Ai ai)
        {
            Beer beer = ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.Beer, Bar.BeerSpawnTransform.position, Quaternion.identity).GetComponent<Beer>();
            beer.Init(ai);
        }

        #region DOTWEEN FUNCTIONS
        private void StartThrowSequence()
        {
            CreateThrowSequence();
            _throwSequence.Play();
        }
        private void CreateThrowSequence()
        {
            if (_throwSequence == null)
            {
                _throwSequence = DOTween.Sequence();
                _throwSequenceID = Guid.NewGuid();
                _throwSequence.id = _throwSequenceID;


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
