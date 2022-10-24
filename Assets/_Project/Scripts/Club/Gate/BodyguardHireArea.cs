using TMPro;
using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BodyguardHireArea : MonoBehaviour
    {
        private TextMeshProUGUI _costText;

        public void Init(Gate gate)
        {
            if (_costText == null)
                _costText = transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

            _costText.text = Gate.BodyguardHiredCost.ToString();
        }

        public void OpenHireCanvas()
        {
            if (!BodyguardHireCanvas.IsOpen)
            {
                GateUpgradeEvents.OnOpenHireCanvas?.Invoke();
                PlayerEvents.OnOpenedUpgradeCanvas?.Invoke();
            }
        }
        public void CloseHireCanvas()
        {
            if (BodyguardHireCanvas.IsOpen)
            {
                GateUpgradeEvents.OnCloseHireCanvas?.Invoke();
                PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
            }
        }
    }
}
