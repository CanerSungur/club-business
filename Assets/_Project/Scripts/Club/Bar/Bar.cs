using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class Bar : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private Bartender bartender;
        [SerializeField] private BarUpgradeCanvas barUpgradeCanvas;
        [SerializeField] private Transform beerSpawnTransform;

        public static Transform BeerSpawnTransform { get; private set; }

        public static bool BartenderHired { get; private set; }
        public static int BartenderHiredCost { get; private set; }
        // ##########
        public static float BartenderStamina { get; private set; }
        public static int BartenderStaminaLevel { get; private set; }
        public static int BartenderStaminaCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, BartenderStaminaLevel));
        // ##########
        public static float BartenderSpeed { get; private set; }
        public static int BartenderSpeedLevel { get; private set; }
        public static int BartenderSpeedCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, BartenderSpeedLevel));

        // cost data
        private static readonly int _upgradeCost = 30;
        private static readonly float _upgradeCostIncreaseRate = 1.2f;

        // core data
        private readonly float _coreBartenderStamina = 20f;
        private readonly float _coreBartenderSpeed = 3f;

        // increment data
        private readonly float _bartenderStaminaIncrement = 5f;
        private readonly float _bartenderSpeedIncrement = 0.5f;

        private void Start()
        {
            BartenderHiredCost = 1000;
            BeerSpawnTransform = beerSpawnTransform;

            LoadData();

            UpdateBartenderStamina();
            UpdateBartenderSpeed();
            UpdateBartenderHire();

            barUpgradeCanvas.Init(this);

            BarUpgradeEvents.OnUpgradeBartenderHire += BartenderHireUpgrade;
            BarUpgradeEvents.OnUpgradeBartenderStamina += BartenderStaminaUpgrade;
            BarUpgradeEvents.OnUpgradeBartenderSpeed += BartenderSpeedUpgrade;
        }

        private void OnDisable()
        {
            BarUpgradeEvents.OnUpgradeBartenderHire -= BartenderHireUpgrade;
            BarUpgradeEvents.OnUpgradeBartenderStamina -= BartenderStaminaUpgrade;
            BarUpgradeEvents.OnUpgradeBartenderSpeed -= BartenderSpeedUpgrade;

            SaveData();
        }

        #region UPGRADE FUNCTIONS
        private void BartenderHireUpgrade()
        {
            IncreaseBartenderHireLevel();
            UpdateBartenderHire();
        }
        private void BartenderStaminaUpgrade()
        {
            IncreaseBartenderStaminaLevel();
            UpdateBartenderStamina();
            //PlayerEvents.OnCheer?.Invoke();
        }
        private void BartenderSpeedUpgrade()
        {
            IncreaseBartenderSpeedLevel();
            UpdateBartenderSpeed();
            //PlayerEvents.OnCheer?.Invoke();
        }
        #endregion

        #region UPDATE FUNCTIONS
        private void UpdateBartenderHire()
        {
            if (BartenderHired)
            {
                bartender.gameObject.SetActive(true);
                bartender.Init(this);
            }
            else
                bartender.gameObject.SetActive(false);
        }
        private void UpdateBartenderStamina()
        {
            BartenderStamina = _coreBartenderStamina + _bartenderStaminaIncrement * (BartenderStaminaLevel - 1);
            BarEvents.OnSetCurrentBartenderStamina?.Invoke();
        }
        private void UpdateBartenderSpeed()
        {
            BartenderSpeed = _coreBartenderSpeed + _bartenderSpeedIncrement * (BartenderSpeedLevel - 1);
            BarEvents.OnSetCurrentBartenderSpeed?.Invoke();
        }
        #endregion

        #region INCREMENT FUNCTIONS
        private void IncreaseBartenderHireLevel()
        {
            if (DataManager.TotalMoney >= BartenderHiredCost)
            {
                BartenderHired = true;
                BarEvents.OnBartenderHired?.Invoke();

                CollectableEvents.OnSpend?.Invoke(BartenderHiredCost);
                BarUpgradeEvents.OnUpdateUpgradeTexts?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            }
        }
        private void IncreaseBartenderStaminaLevel()
        {
            if (DataManager.TotalMoney >= BartenderStaminaCost)
            {
                CollectableEvents.OnSpend?.Invoke(BartenderStaminaCost);
                BartenderStaminaLevel++;
                BarUpgradeEvents.OnUpdateUpgradeTexts?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            }
        }
        private void IncreaseBartenderSpeedLevel()
        {
            if (DataManager.TotalMoney >= BartenderSpeedCost)
            {
                CollectableEvents.OnSpend?.Invoke(BartenderSpeedCost);
                BartenderSpeedLevel++;
                BarUpgradeEvents.OnUpdateUpgradeTexts?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            }
        }
        #endregion

        #region SAVE-LOAD
        private void LoadData()
        {
            BartenderHired = PlayerPrefs.GetInt("BartenderHired", 0) == 1 ? true : false;
            BartenderStaminaLevel = PlayerPrefs.GetInt("BartenderStaminaLevel", 1);
            BartenderSpeedLevel = PlayerPrefs.GetInt("BartenderSpeedLevel", 1);
        }
        private void SaveData()
        {
            PlayerPrefs.SetInt("BartenderHired", BartenderHired == true ? 1 : 0);
            PlayerPrefs.SetInt("BartenderStaminaLevel", BartenderStaminaLevel);
            PlayerPrefs.SetInt("BartenderSpeedLevel", BartenderSpeedLevel);
            PlayerPrefs.Save();
        }
        //private void OnApplicationPause(bool pause) => SaveData();
        //private void OnApplicationQuit() => SaveData();
        #endregion
    }
}
