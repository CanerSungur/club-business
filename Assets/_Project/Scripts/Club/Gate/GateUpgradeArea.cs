using ZestGames;

namespace ClubBusiness
{
    public class GateUpgradeArea : UpgradeAreaBase
    {
        public override void OpenUpgradeCanvas()
        {
            if (!UpgradeCanvas.IsOpen)
            {
                GateUpgradeEvents.OnOpenCanvas?.Invoke();
                PlayerEvents.OnOpenedUpgradeCanvas?.Invoke();
            }
        }

        public override void CloseUpgradeCanvas()
        {
            if (UpgradeCanvas.IsOpen)
                GateUpgradeEvents.OnCloseCanvas?.Invoke();
        }
    }
}
