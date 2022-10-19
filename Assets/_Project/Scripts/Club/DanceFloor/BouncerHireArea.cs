using TMPro;
using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BouncerHireArea : MonoBehaviour
    {
        private TextMeshPro _costText;

        public void Init(DanceFloor danceFloor)
        {
            if (_costText == null)
                _costText = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetComponent<TextMeshPro>();

            _costText.text = DanceFloor.BouncerHiredCost.ToString();
        }

        public void OpenHireCanvas()
        {
            if (!BouncerHireCanvas.IsOpen)
            {
                DanceFloorUpgradeEvents.OnOpenHireCanvas?.Invoke();
                PlayerEvents.OnOpenedUpgradeCanvas?.Invoke();
            }
        }
        public void CloseHireCanvas()
        {
            if (BouncerHireCanvas.IsOpen)
            {
                DanceFloorUpgradeEvents.OnCloseHireCanvas?.Invoke();
                PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
            }
        }
    }
}
