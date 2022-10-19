using TMPro;
using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BodyguardHireArea : MonoBehaviour
    {
        private TextMeshPro _costText;

        public void Init(Gate gate)
        {
            if (_costText == null)
                _costText = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetComponent<TextMeshPro>();

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
