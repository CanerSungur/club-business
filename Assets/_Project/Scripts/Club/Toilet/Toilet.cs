using System.Collections.Generic;
using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class Toilet : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private Cleaner cleaner;
        [SerializeField] private ToiletUpgradeCanvas toiletUpgradeCanvas;

        #region FIX SECTION
        public static int FixCount { get; private set; }
        #endregion

        #region TOILET ITEM LIST
        private static List<ToiletItem> _emptyToiletItems;
        public static List<ToiletItem> EmptyToiletItems => _emptyToiletItems == null ? _emptyToiletItems = new List<ToiletItem>() : _emptyToiletItems;
        public static void AddEmptyToiletItem(ToiletItem toiletItem)
        {
            if (!EmptyToiletItems.Contains(toiletItem))
            {
                EmptyToiletItems.Add(toiletItem);
            }
        }
        public static void RemoveEmptyToiletItem(ToiletItem toiletItem)
        {
            if (EmptyToiletItems.Contains(toiletItem))
            {
                EmptyToiletItems.Remove(toiletItem);
            }
        }
        #endregion

        public void Start()
        {
            //toiletUpgradeCanvas.Init(this);
            //cleaner.Init(this);

            FixCount = 5;
        }
    }
}
