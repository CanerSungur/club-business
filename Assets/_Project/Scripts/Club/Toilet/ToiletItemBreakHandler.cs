using UnityEngine;

namespace ClubBusiness
{
    public class ToiletItemBreakHandler : MonoBehaviour
    {
        private ToiletItem _toiletItem;
        private int _currentFixCount;

        [Header("-- SETUP --")]
        [SerializeField] private LiquidPuddle liquidPuddle;
        [SerializeField] private ParticleSystem liquidSplashPS;

        public void Init(ToiletItem toiletItem)
        {
            if (_toiletItem == null)
                _toiletItem = toiletItem;

            _currentFixCount = 0;
            liquidPuddle.gameObject.SetActive(false);

            _toiletItem.OnBreak += Break;
            _toiletItem.OnFix += Fix;
        }

        private void OnDisable()
        {
            if (_toiletItem == null) return;

            _toiletItem.OnBreak -= Break;
            _toiletItem.OnFix -= Fix;
        }

        private void CompleteFixing()
        {
            _toiletItem.OnFixCompleted?.Invoke();
            liquidPuddle.gameObject.SetActive(false);
            liquidSplashPS.Stop();
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
            liquidPuddle.DecreasePuddleScale();
            // Decrease splash amount


            if (_currentFixCount >= Toilet.FixCount && _toiletItem.IsBroken)
                CompleteFixing();
        }
        #endregion
    }
}
