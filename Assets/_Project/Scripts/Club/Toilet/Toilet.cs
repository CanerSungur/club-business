using System.Collections.Generic;
using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class Toilet : MonoBehaviour
    {
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
    }
}
