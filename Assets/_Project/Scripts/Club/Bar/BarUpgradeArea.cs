using ZestGames;

namespace ClubBusiness
{
    public class BarUpgradeArea : UpgradeAreaBase
    {
        public override void OpenUpgradeCanvas()
        {
            if (!BarUpgradeCanvas.IsOpen)
            {
                BarUpgradeEvents.OnOpenCanvas?.Invoke();
                PlayerEvents.OnOpenedUpgradeCanvas?.Invoke();
            }
        }

        public override void CloseUpgradeCanvas()
        {
            if (BarUpgradeCanvas.IsOpen)
            {
                BarUpgradeEvents.OnCloseCanvas?.Invoke();
                PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
            }
        }
    }
}
