using System.Collections;
using UnityEngine;
using ZestCore.Utility;
using ZestGames;

namespace ClubBusiness
{
    public class DanceFloor : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private Bouncer bouncer;
        [SerializeField] private DanceFloorUpgradeCanvas danceFloorUpgradeCanvas;
        [SerializeField] private Transform bouncerWaitTransform;

        #region FIGHTING
        private static bool _fightIsHappening;
        private static int _currentFightCount = 0;
        private static int _maxFightAtOnceCount = 1;
        private static readonly int _fightChance = 50;
        private readonly WaitForSeconds _waitForTriggerFightDelay = new WaitForSeconds(5f);
        private readonly float _argueDuration = 10f;
        private readonly float _fightDuration = 15f;
        #endregion

        #region PROPERTIES
        public static bool CanBouncerBreakFight => _currentFightCount > 0 && AttackerAi != null;
        public static bool CanTriggerFight => CustomerManager.CustomersCanFight.Count > 1 && RNG.RollDice(_fightChance) && _currentFightCount < _maxFightAtOnceCount;
        public static int Capacity { get; private set; }
        public static bool HasCapacity => CustomerManager.CustomersOnDanceFloor.Count < Capacity;
        public static Ai AttackerAi { get; private set; }
        public static Ai DefenderAi { get; private set; }
        public static float ArgueDuration { get; private set; }
        public static float FightDuration { get; private set; }
        public static Transform BouncerWaitTransform { get; private set; }
        #endregion

        #region UPGRADE LEVEL CAP
        public static int BouncerStaminaLevelCap { get; private set; }
        public static int BouncerPowerLevelCap { get; private set; }
        #endregion

        #region UPGRADES
        public static bool BouncerHired { get; private set; }
        public static int BouncerHiredCost { get; private set; }
        // ##########
        public static float BouncerStamina { get; private set; }
        public static int BouncerStaminaLevel { get; private set; }
        public static int BouncerStaminaCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, BouncerStaminaLevel));
        // ##########
        public static float BouncerPower { get; private set; } // level 30 max.
        public static int BouncerPowerLevel { get; private set; } // level 15 max.
        public static int BouncerPowerCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, BouncerPowerLevel));

        // cost data
        private static readonly int _upgradeCost = 30;
        private static readonly float _upgradeCostIncreaseRate = 1.3f;

        // core data
        private readonly float _coreBouncerStamina = 2f;
        private readonly float _coreBouncerPower = 5f;

        // increment data
        private readonly float _bouncerStaminaIncrement = 1f;
        private readonly float _bouncerPowerIncrement = 0.28f;
        #endregion

        private void Start()
        {
            BouncerHiredCost = 2000;
            Capacity = 99;
            AttackerAi = DefenderAi = null;
            ArgueDuration = _argueDuration;
            FightDuration = _fightDuration;
            BouncerWaitTransform = bouncerWaitTransform;
            BouncerStaminaLevelCap = 5;
            BouncerPowerLevelCap = 10;

            LoadData();

            UpdateBouncerStamina();
            UpdateBouncerPower();
            UpdateBouncerHire();

            danceFloorUpgradeCanvas.Init(this);

            DanceFloorUpgradeEvents.OnUpgradeBouncerHire += BouncerHireUpgrade;
            DanceFloorUpgradeEvents.OnUpgradeBouncerStamina += BouncerStaminaUpgrade;
            DanceFloorUpgradeEvents.OnUpgradeBouncerPower += BouncerPowerUpgrade;

            StartCoroutine(TriggerFightCoroutine());

            ClubEvents.OnAFightEnded += AFightEnded;
        }

        private void OnDisable()
        {
            ClubEvents.OnAFightEnded -= AFightEnded;

            DanceFloorUpgradeEvents.OnUpgradeBouncerHire -= BouncerHireUpgrade;
            DanceFloorUpgradeEvents.OnUpgradeBouncerStamina -= BouncerStaminaUpgrade;
            DanceFloorUpgradeEvents.OnUpgradeBouncerPower -= BouncerPowerUpgrade;

            SaveData();
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
            AttackerAi = DefenderAi = null;
            _currentFightCount--;
            if (_currentFightCount < 0)
                _currentFightCount = 0;
        }
        #endregion

        #region FIGHT SECTION
        private void StartFight()
        {
            if (CanTriggerFight)
            {
                AttackerAi = CustomerManager.CustomersCanFight[Random.Range(0, CustomerManager.CustomersCanFight.Count)];
                CustomerManager.CustomersCanFight.Remove(AttackerAi);
                DefenderAi = CustomerManager.CustomersCanFight[Random.Range(0, CustomerManager.CustomersCanFight.Count)];
                CustomerManager.CustomersCanFight.Remove(DefenderAi);
                //for (int i = 0; i < CustomerManager.CustomersOnDanceFloor.Count; i++)
                //{
                //    if (CustomerManager.CustomersOnDanceFloor[i] != AttackerAi)
                //    {
                //        DefenderAi = CustomerManager.CustomersOnDanceFloor[i];
                //        CustomerManager.CustomersCanFight.Remove(DefenderAi);
                //        break;
                //    }
                //}

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
                    AttackerAi = CustomerManager.CustomersCanFight[Random.Range(0, CustomerManager.CustomersCanFight.Count)];
                    CustomerManager.CustomersCanFight.Remove(AttackerAi);
                    DefenderAi = CustomerManager.CustomersCanFight[Random.Range(0, CustomerManager.CustomersCanFight.Count)];
                    CustomerManager.CustomersCanFight.Remove(DefenderAi);

                    //AttackerAi = CustomerManager.CustomersOnDanceFloor[Random.Range(0, CustomerManager.CustomersOnDanceFloor.Count)];
                    //for (int i = 0; i < CustomerManager.CustomersOnDanceFloor.Count; i++)
                    //{
                    //    if (CustomerManager.CustomersOnDanceFloor[i] != AttackerAi)
                    //    {
                    //        DefenderAi = CustomerManager.CustomersOnDanceFloor[i];
                    //        break;
                    //    }
                    //}

                    AttackerAi.StateManager.SwitchState(AttackerAi.StateManager.PickFightState);
                    _currentFightCount++;
                }

                yield return _waitForTriggerFightDelay;
            }
        }
        #endregion
        #endregion

        #region UPGRADE SECTION
        #region UPGRADE FUNCTIONS
        private void BouncerHireUpgrade()
        {
            IncreaseBouncerHireLevel();
            UpdateBouncerHire();
        }
        private void BouncerStaminaUpgrade()
        {
            IncreaseBouncerStaminaLevel();
            UpdateBouncerStamina();
            //PlayerEvents.OnCheer?.Invoke();
        }
        private void BouncerPowerUpgrade()
        {
            IncreaseBouncerPowerLevel();
            UpdateBouncerPower();
            //PlayerEvents.OnCheer?.Invoke();
        }
        #endregion

        #region UPDATE FUNCTIONS
        private void UpdateBouncerHire()
        {
            if (BouncerHired)
            {
                bouncer.gameObject.SetActive(true);
                bouncer.Init(this);
            }
            else
                bouncer.gameObject.SetActive(false);
        }
        private void UpdateBouncerStamina()
        {
            BouncerStamina = _coreBouncerStamina + _bouncerStaminaIncrement * (BouncerStaminaLevel - 1);
            DanceFloorEvents.OnSetCurrentBouncerStamina?.Invoke();
        }
        private void UpdateBouncerPower()
        {
            BouncerPower = _coreBouncerPower + _bouncerPowerIncrement * (BouncerPowerLevel - 1);
            DanceFloorEvents.OnSetCurrentBouncerPower?.Invoke();
        }
        #endregion

        #region INCREMENT FUNCTIONS
        private void IncreaseBouncerHireLevel()
        {
            if (DataManager.TotalMoney >= BouncerHiredCost)
            {
                BouncerHired = true;
                DanceFloorEvents.OnBouncerHired?.Invoke();

                CollectableEvents.OnSpend?.Invoke(BouncerHiredCost);
                DanceFloorUpgradeEvents.OnUpdateUpgradeTexts?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            }
        }
        private void IncreaseBouncerStaminaLevel()
        {
            if (DataManager.TotalMoney >= BouncerStaminaCost)
            {
                CollectableEvents.OnSpend?.Invoke(BouncerStaminaCost);
                BouncerStaminaLevel++;
                DanceFloorUpgradeEvents.OnUpdateUpgradeTexts?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            }
        }
        private void IncreaseBouncerPowerLevel()
        {
            if (DataManager.TotalMoney >= BouncerPowerCost)
            {
                CollectableEvents.OnSpend?.Invoke(BouncerPowerCost);
                BouncerPowerLevel++;
                DanceFloorUpgradeEvents.OnUpdateUpgradeTexts?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            }
        }
        #endregion

        #region SAVE-LOAD
        private void LoadData()
        {
            BouncerHired = PlayerPrefs.GetInt("BouncerHired", 0) == 1 ? true : false;
            BouncerStaminaLevel = PlayerPrefs.GetInt("BouncerStaminaLevel", 1);
            BouncerPowerLevel = PlayerPrefs.GetInt("BouncerPowerLevel", 1);
        }
        private void SaveData()
        {
            PlayerPrefs.SetInt("BouncerHired", BouncerHired == true ? 1 : 0);
            PlayerPrefs.SetInt("BouncerStaminaLevel", BouncerStaminaLevel);
            PlayerPrefs.SetInt("BouncerPowerLevel", BouncerPowerLevel);
            PlayerPrefs.Save();
        }
        //private void OnApplicationPause(bool pause) => SaveData();
        //private void OnApplicationQuit() => SaveData();
        #endregion
        #endregion
    }
}
