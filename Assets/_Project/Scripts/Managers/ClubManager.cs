using System.Collections;
using UnityEngine;
using ZestCore.Utility;
using ZestGames;

namespace ClubBusiness
{
    public class ClubManager : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private Transform[] exitTransforms;

        #region FIGHTING
        private int _maxFightAtOnceCount = 1;
        private static readonly int fightChance = 100;
        private readonly WaitForSeconds _waitForTriggerFightDelay = new WaitForSeconds(5f);
        #endregion

        #region PROPERTIES
        public static int ClubCapacity { get; private set; }
        public static int DanceFloorCapacity { get; private set; }
        public static Transform[] ExitTransforms { get; private set; }
        public static bool CanTriggerFight => CustomerManager.CustomersOnDanceFloor.Count > 1 && RNG.RollDice(fightChance);
        #endregion

        #region CONTROLS
        public static bool ClubHasCapacity => CustomerManager.CustomersInside.Count < ClubCapacity;
        public static bool ToiletIsAvailable => Toilet.EmptyToiletItems.Count > 0;
        public static bool DanceFloorHasCapacity => CustomerManager.CustomersOnDanceFloor.Count < DanceFloorCapacity;
        #endregion

        public void Init(GameManager gameManager)
        {
            ClubCapacity = 100;
            DanceFloorCapacity = 99;
            ExitTransforms = exitTransforms;

            //StartCoroutine(TriggerFightCoroutine());
        }

        #region COROUTINE FUNCTIONS
        private IEnumerator TriggerFightCoroutine()
        {
            while (true)
            {
                if (CanTriggerFight)
                {
                    Ai randomDancingAi = CustomerManager.CustomersOnDanceFloor[Random.Range(0, CustomerManager.CustomersOnDanceFloor.Count)];
                    randomDancingAi.StateManager.SwitchState(randomDancingAi.StateManager.AttackState);
                }

                yield return _waitForTriggerFightDelay;
            }
        }
        #endregion
    }
}
