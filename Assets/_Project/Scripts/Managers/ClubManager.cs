using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class ClubManager : MonoBehaviour
    {
        public static int ClubCapacity { get; private set; }
        public static int DanceFloorCapacity { get; private set; }

        #region CONTROLS
        public static bool ClubHasCapacity => CustomerManager.CustomersInside.Count < ClubCapacity;
        public static bool ToiletIsAvailable => Toilet.EmptyToiletItems.Count > 0;
        public static bool DanceFloorHasCapacity => CustomerManager.CustomersOnDanceFloor.Count < DanceFloorCapacity;
        #endregion

        public void Init(GameManager gameManager)
        {
            ClubCapacity = 20;
            DanceFloorCapacity = 5;
        }
    }
}
