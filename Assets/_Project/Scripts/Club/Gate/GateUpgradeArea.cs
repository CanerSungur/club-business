using ZestGames;

namespace ClubBusiness
{
    public class GateUpgradeArea : UpgradeAreaBase
    {
        public override void OpenUpgradeCanvas()
        {
            if (!GateUpgradeCanvas.IsOpen)
            {
                GateUpgradeEvents.OnOpenCanvas?.Invoke();
                PlayerEvents.OnOpenedUpgradeCanvas?.Invoke();
            }
        }

        public override void CloseUpgradeCanvas()
        {
            if (GateUpgradeCanvas.IsOpen)
            {
                GateUpgradeEvents.OnCloseCanvas?.Invoke();
                PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
            }
        }
    }
}
