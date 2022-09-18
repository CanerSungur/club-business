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

        #region UPGRADE LEVEL CAP
        public static int BartenderStaminaLevelCap { get; private set; }
        public static int BartenderPourDurationLevelCap { get; private set; }
        #endregion

        public static bool BartenderHired { get; private set; }
        public static int BartenderHiredCost { get; private set; }
        // ##########
        public static float BartenderStamina { get; private set; }
        public static int BartenderStaminaLevel { get; private set; }
        public static int BartenderStaminaCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, BartenderStaminaLevel));
        // ##########
        public static float BartenderPourDuration { get; private set; }
        public static int BartenderPourDurationLevel { get; private set; }
        public static int BartenderPourDurationCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, BartenderPourDurationLevel));

        // cost data
        private static readonly int _upgradeCost = 70;
        private static readonly float _upgradeCostIncreaseRate = 1.3f;

        // core data
        private readonly float _coreBartenderStamina = 5f;
        private readonly float _coreBartenderPourDuration = 5f;

        // increment data
        private readonly float _bartenderStaminaIncrement = 1f;
        private readonly float _bartenderPourDurationDecrease = 0.28f;

        private void Start()
        {
            BartenderHiredCost = 1000;
            BeerSpawnTransform = beerSpawnTransform;
            BartenderStaminaLevelCap = 30;
            BartenderPourDurationLevelCap = 15;

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
            BartenderPourDuration = _coreBartenderPourDuration + _bartenderPourDurationDecrease * (BartenderPourDurationLevel - 1);
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
            if (DataManager.TotalMoney >= BartenderPourDurationCost)
            {
                CollectableEvents.OnSpend?.Invoke(BartenderPourDurationCost);
                BartenderPourDurationLevel++;
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
            BartenderPourDurationLevel = PlayerPrefs.GetInt("BartenderSpeedLevel", 1);
        }
        private void SaveData()
        {
            PlayerPrefs.SetInt("BartenderHired", BartenderHired == true ? 1 : 0);
            PlayerPrefs.SetInt("BartenderStaminaLevel", BartenderStaminaLevel);
            PlayerPrefs.SetInt("BartenderSpeedLevel", BartenderPourDurationLevel);
            PlayerPrefs.Save();
        }
        //private void OnApplicationPause(bool pause) => SaveData();
        //private void OnApplicationQuit() => SaveData();
        #endregion
    }
}
