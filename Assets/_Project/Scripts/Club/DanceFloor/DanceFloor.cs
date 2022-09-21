using System.Collections;
using UnityEngine;
using ZestCore.Utility;
using ZestGames;

namespace ClubBusiness
{
    public class DanceFloor : MonoBehaviour
    {
        #region FIGHTING
        private static int _currentFightCount = 0;
        private static int _maxFightAtOnceCount = 1;
        private static readonly int _fightChance = 50;
        private readonly WaitForSeconds _waitForTriggerFightDelay = new WaitForSeconds(2f);
        private readonly float _argueDuration = 10f;
        private readonly float _fightDuration = 15f;
        #endregion

        public static bool CanTriggerFight => CustomerManager.CustomersOnDanceFloor.Count > 1 && RNG.RollDice(_fightChance) && _currentFightCount < _maxFightAtOnceCount;
        public static int Capacity { get; private set; }
        public static bool HasCapacity => CustomerManager.CustomersOnDanceFloor.Count < Capacity;
        public static Ai AttackerAi { get; private set; }
        public static Ai DefenderAi { get; private set; }
        public static float ArgueDuration { get; private set; }
        public static float FightDuration { get; private set; }

        private void Start()
        {
            Capacity = 99;
            AttackerAi = DefenderAi = null;
            ArgueDuration = _argueDuration;
            FightDuration = _fightDuration;

            StartCoroutine(TriggerFightCoroutine());

            ClubEvents.OnAFightEnded += AFightEnded;
        }

        private void OnDisable()
        {
            ClubEvents.OnAFightEnded -= AFightEnded;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartFight();
            }
        }

        #region EVENT HANDLER FUNCTIONS
        private void AFightEnded()
        {
            _currentFightCount--;
            if (_currentFightCount < 0)
                _currentFightCount = 0;
        }
        #endregion

        private void StartFight()
        {
            if (CanTriggerFight)
            {
                AttackerAi = CustomerManager.CustomersOnDanceFloor[Random.Range(0, CustomerManager.CustomersOnDanceFloor.Count)];
                for (int i = 0; i < CustomerManager.CustomersOnDanceFloor.Count; i++)
                {
                    if (CustomerManager.CustomersOnDanceFloor[i] != AttackerAi)
                    {
                        DefenderAi = CustomerManager.CustomersOnDanceFloor[i];
                        break;
                    }
                }

                AttackerAi.StateManager.SwitchState(AttackerAi.StateManager.PickFightState);
                _currentFightCount++;
            }
        }

        #region COROUTINE FUNCTIONS
        private IEnumerator TriggerFightCoroutine()
        {
            while (true)
            {
                if (CanTriggerFight)
                {
                    AttackerAi = CustomerManager.CustomersOnDanceFloor[Random.Range(0, CustomerManager.CustomersOnDanceFloor.Count)];
                    for (int i = 0; i < CustomerManager.CustomersOnDanceFloor.Count; i++)
                    {
                        if (CustomerManager.CustomersOnDanceFloor[i] != AttackerAi)
                        {
                            DefenderAi = CustomerManager.CustomersOnDanceFloor[i];
                            break;
                        }
                    }

                    AttackerAi.StateManager.SwitchState(AttackerAi.StateManager.PickFightState);
                    _currentFightCount++;
                }

                yield return _waitForTriggerFightDelay;
            }
        }
    }
    #endregion
}
