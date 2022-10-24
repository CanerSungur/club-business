using TMPro;
using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class CleanerHireArea : MonoBehaviour
    {
        private TextMeshProUGUI _costText;

        public void Init(Toilet toilet)
        {
            if (_costText == null)
                _costText = transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

            _costText.text = Toilet.CleanerHiredCost.ToString();
        }

        public void OpenHireCanvas()
        {
            if (!CleanerHireCanvas.IsOpen)
            {
                ToiletUpgradeEvents.OnOpenHireCanvas?.Invoke();
                PlayerEvents.OnOpenedUpgradeCanvas?.Invoke();
            }
        }
        public void CloseHireCanvas()
        {
            if (CleanerHireCanvas.IsOpen)
            {
                ToiletUpgradeEvents.OnCloseHireCanvas?.Invoke();
                PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
            }
        }
    }
}
