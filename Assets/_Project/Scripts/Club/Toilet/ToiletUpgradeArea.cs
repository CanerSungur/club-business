using ZestGames;

namespace ClubBusiness
{
    public class ToiletUpgradeArea : UpgradeAreaBase
    {
        public override void OpenUpgradeCanvas()
        {
            if (!ToiletUpgradeCanvas.IsOpen)
            {
                ToiletUpgradeEvents.OnOpenCanvas?.Invoke();
                PlayerEvents.OnOpenedUpgradeCanvas?.Invoke();
            }
        }
        public override void CloseUpgradeCanvas()
        {
            if (ToiletUpgradeCanvas.IsOpen)
            {
                ToiletUpgradeEvents.OnCloseCanvas?.Invoke();
                PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
            }
        }
    }
}
