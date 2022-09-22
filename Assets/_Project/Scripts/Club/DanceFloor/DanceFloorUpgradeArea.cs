using ZestGames;

namespace ClubBusiness
{
    public class DanceFloorUpgradeArea : UpgradeAreaBase
    {
        public override void OpenUpgradeCanvas()
        {
            if (!DanceFloorUpgradeCanvas.IsOpen)
            {
                DanceFloorUpgradeEvents.OnOpenCanvas?.Invoke();
                PlayerEvents.OnOpenedUpgradeCanvas?.Invoke();
            }
        }

        public override void CloseUpgradeCanvas()
        {
            if (DanceFloorUpgradeCanvas.IsOpen)
            {
                DanceFloorUpgradeEvents.OnCloseCanvas?.Invoke();
                PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
            }
        }
    }
}
