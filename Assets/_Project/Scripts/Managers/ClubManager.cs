using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class ClubManager : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private Transform[] exitTransforms;

        #region PROPERTIES
        public static int ClubCapacity { get; private set; }
        public static Transform[] ExitTransforms { get; private set; }
        #endregion

        #region CONTROLS
        public static bool ClubHasCapacity => CustomerManager.CustomersInside.Count < ClubCapacity;
        public static bool ToiletIsAvailable => Toilet.EmptyToiletItems.Count > 0;
        #endregion

        public void Init(GameManager gameManager)
        {
            ClubCapacity = 100;
            ExitTransforms = exitTransforms;
        }
    }
}
