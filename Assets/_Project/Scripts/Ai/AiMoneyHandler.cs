using UnityEngine;
using ZestGames;
using System.Collections;

namespace ClubBusiness
{
    public class AiMoneyHandler : MonoBehaviour
    {
        private Ai _ai;

        private readonly WaitForSeconds _spawnMoneyDelay = new WaitForSeconds(0.05f);

        #region MONEY VALUES
        private readonly int _drinkValue = 10;
        private readonly int _toiletValue = 5;
        #endregion

        public void Init(Ai ai)
        {
            if (_ai == null)
                _ai = ai;
        }

        private void Tip()
        {
            if (_ai.AngerHandler.CurrentMood == Enums.AiMood.Happy)
            {
                // tip 3 money: Money value can change
            }
            else if (_ai.AngerHandler.CurrentMood == Enums.AiMood.VeryHappy)
            {
                // tip 5 money: Money value can change
            }
            else
            {
                // no tip
            }
        }

        #region COLLECTING MONEY
        private IEnumerator SpawnMoneyCoroutine(int value)
        {
            int currentCount = 0;
            while (currentCount < value)
            {
                MoneyCanvas.Instance.SpawnCollectMoney(transform);
                currentCount++;

                yield return _spawnMoneyDelay;
            }
        }

        #region PUBLICS
        public void StartSpawningMoney(int value) => StartCoroutine(SpawnMoneyCoroutine(value));
        public void StartSpawningDrinkingMoney() => StartCoroutine(SpawnMoneyCoroutine(_drinkValue));
        public void StartSpawningToiletMoney() => StartCoroutine(SpawnMoneyCoroutine(_toiletValue));
        #endregion   
        #endregion
    }
}
