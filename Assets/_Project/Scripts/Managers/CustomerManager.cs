using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class CustomerManager : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private int customersOutsideMaxCount = 10;
        [SerializeField] private int customersInsideMaxCount = 20;
        [SerializeField] private Transform spawnTransform;

        #region SPAWN SETUP
        private readonly WaitForSeconds _waitForSpawnDelay = new WaitForSeconds(3f);
        #endregion

        #region CUSTOMERS INSIDE
        private static List<Ai> _customersInside;
        public static List<Ai> CustomersInside => _customersInside == null ? _customersInside = new List<Ai>() : _customersInside;
        public static void AddCustomerInside(Ai ai)
        {
            if (!CustomersInside.Contains(ai))
                CustomersInside.Add(ai);
        }
        public static void RemoveCustomerInside(Ai ai)
        {
            if (CustomersInside.Contains(ai))
                CustomersInside.Remove(ai);
        }
        #endregion

        #region CUSTOMERS OUTSIDE
        private static List<Ai> _customersOutside;
        public static List<Ai> CustomersOutside => _customersOutside == null ? _customersOutside = new List<Ai>() : _customersOutside;
        public static void AddCustomerOutside(Ai ai)
        {
            if (!CustomersOutside.Contains(ai))
                CustomersOutside.Add(ai);
        }
        public static void RemoveCustomerOutside(Ai ai)
        {
            if (CustomersOutside.Contains(ai))
                CustomersOutside.Remove(ai);
        }
        #endregion

        #region CUSTOMERS ON DANCE FLOOR
            private static List<Ai> _customersOnDanceFloor;
        public static List<Ai> CustomersOnDanceFloor => _customersOnDanceFloor == null ? _customersOnDanceFloor = new List<Ai>() : _customersOnDanceFloor;
        public static void AddCustomerOnDanceFloor(Ai ai)
        {
            if (!CustomersOnDanceFloor.Contains(ai))
                CustomersOnDanceFloor.Add(ai);
        }
        public static void RemoveCustomerFromDanceFloor(Ai ai)
        {
            if (CustomersOnDanceFloor.Contains(ai))
                CustomersOnDanceFloor.Remove(ai);
        }
        #endregion

        public static float CustomerWaitDuration { get; private set; }

        public void Init(GameManager gameManager)
        {
            CustomerWaitDuration = 5f;

            StartCoroutine(SpawnCustomerOutsideCoroutine());
        }

        #region COROUTINE FUNCTIONS
        private IEnumerator SpawnCustomerOutsideCoroutine()
        {
            while (true)
            {
                if (CustomersOutside.Count < customersOutsideMaxCount && GameManager.GameState == Enums.GameState.Started)
                {
                    Ai customerOutside = ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.Customer, spawnTransform.position, Quaternion.identity).GetComponent<Ai>();
                    customerOutside.Init(this, Enums.AiLocation.Outside);

                    //Debug.Log("Outside: " + CustomersOutside.Count);
                    //Debug.Log("Inside: " + CustomersInside.Count);
                }

                yield return _waitForSpawnDelay;
            }
        }
        #endregion
    }
}
