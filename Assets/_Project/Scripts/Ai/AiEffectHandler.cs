using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class AiEffectHandler : MonoBehaviour
    {
        private Ai _ai;
        private FightSmoke _fightSmoke;

        [Header("-- DRINK SETUP --")]
        [SerializeField] private GameObject beerObj;

        [Header("-- FIGHT SETUP --")]
        [SerializeField] private Animator argueAnimator;
        [SerializeField] private ParticleSystem dizzyPS;

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
            _ai.OnStartArguing += Open;
            _ai.OnStopArguing += Close;
            _ai.OnGetKnockedOut += GetKnockedOut;
            _ai.OnGetUp += GetUp;
        }

        private void OnDisable()
        {
            if (_ai == null) return;
            _ai.OnDrink -= EnableBeer;
            _ai.OnStartArguing -= Open;
            _ai.OnStopArguing -= Close;
            _ai.OnGetKnockedOut -= GetKnockedOut;
            _ai.OnGetUp -= GetUp;
        }

        #region FIGHT FUNCTIONS
        private void GetKnockedOut() => dizzyPS.Play();
        private void GetUp() => dizzyPS.Stop();
        private void Open(Enums.AiStateType ignoreThis)
        {
            argueAnimator.SetBool("Open", true);
        }
        private void Close()
        {
            argueAnimator.SetBool("Open", false);
        }
        #endregion

        #region BEER FUNCTIONS
        private void EnableBeer() => beerObj.SetActive(true);
        public void DisableBeer() => beerObj.SetActive(false);
        #endregion
    }
}
