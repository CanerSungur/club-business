using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class AiEffectHandler : MonoBehaviour
    {
        private Ai _ai;
        private FightSmoke _fightSmoke;

        [Header("-- SETUP --")]
        [SerializeField] private GameObject beerObj;

        public void Init(Ai ai)
        {
            if (_ai == null)
            {
                _ai = ai;
                _fightSmoke = GetComponentInChildren<FightSmoke>();
                _fightSmoke.Init(_ai);
            }

            beerObj.SetActive(false);
            _ai.OnDrink += EnableBeer;
        }

        private void OnDisable()
        {
            if (_ai == null) return;
            _ai.OnDrink -= EnableBeer;
        }

        #region BEER FUNCTIONS
        private void EnableBeer() => beerObj.SetActive(true);
        public void DisableBeer() => beerObj.SetActive(false);
        #endregion
    }
}
