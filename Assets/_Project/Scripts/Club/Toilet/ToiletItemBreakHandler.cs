using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class ToiletItemBreakHandler : MonoBehaviour
    {
        private ToiletItem _toiletItem;
        private int _currentFixCount;

        [Header("-- SETUP --")]
        [SerializeField] private LiquidPuddle liquidPuddle;
        [SerializeField] private ParticleSystem liquidSplashPS;

        #region LIQUID SPLASH RATE
        private readonly float _defaultSplashRate = 20f;
        private readonly float _decreaseSplashRate = 0.3f;
        private float _currentSplashRate;
        #endregion

        public void Init(ToiletItem toiletItem)
        {
            if (_toiletItem == null)
                _toiletItem = toiletItem;

            _currentFixCount = 0;
            liquidPuddle.gameObject.SetActive(false);
            _currentSplashRate = _defaultSplashRate;
            SetLiquidSplashRate(_currentSplashRate);

            _toiletItem.OnBreak += Break;
            _toiletItem.OnFix += Fix;
        }

        private void OnDisable()
        {
            if (_toiletItem == null) return;

            _toiletItem.OnBreak -= Break;
            _toiletItem.OnFix -= Fix;
        }

        public void CompleteFixing()
        {
            _toiletItem.OnFixCompleted?.Invoke();
            liquidPuddle.gameObject.SetActive(false);
            liquidSplashPS.Stop();
            _currentSplashRate = _defaultSplashRate;
            SetLiquidSplashRate(_currentSplashRate);
        }
        private void SetLiquidSplashRate(float currentRate)
        {
            var emission = liquidSplashPS.emission;
            emission.rateOverTime = currentRate;
        }

        #region EVENT HANDLER FUNCTIONS
        private void Break()
        {
            liquidPuddle.gameObject.SetActive(true);
            liquidPuddle.Init(this);
            
            liquidSplashPS.Play();
        }
        private void Fix()
        {
            _currentFixCount++;
            // Decrease puddle scale
            liquidPuddle.DecreasePuddleScale(_currentFixCount);
            // Decrease splash amount
            SetLiquidSplashRate(_currentSplashRate - (_decreaseSplashRate * _currentFixCount));

            if (_currentFixCount >= Toilet.FixCount && _toiletItem.IsBroken)
                CompleteFixing();
        }
        #endregion
    }
}
