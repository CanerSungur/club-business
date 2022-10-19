using UnityEngine;
using ZestGames;
using TMPro;

namespace ClubBusiness
{
    public class BartenderHireArea : MonoBehaviour
    {
        private TextMeshPro _costText;

        public void Init(Bar bar)
        {
            if (_costText == null)
                _costText = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetComponent<TextMeshPro>();

            _costText.text = Bar.BartenderHiredCost.ToString();
        }

        public void OpenHireCanvas()
        {
            if (!BartenderHireCanvas.IsOpen)
            {
                BarUpgradeEvents.OnOpenHireCanvas?.Invoke();
                PlayerEvents.OnOpenedUpgradeCanvas?.Invoke();
            }
        }
        public void CloseHireCanvas()
        {
            if (BartenderHireCanvas.IsOpen)
            {
                BarUpgradeEvents.OnCloseHireCanvas?.Invoke();
                PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
            }
        }
    }
}
