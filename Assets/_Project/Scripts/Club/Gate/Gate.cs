using UnityEngine;
using ZestCore.Utility;
using ZestGames;

namespace ClubBusiness
{
    public class Gate : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private Bodyguard bodyguard;
        [SerializeField] private GateUpgradeCanvas gateUpgradeCanvas;

        #region UPGRADE LEVEL CAP
        public static int BodyguardStaminaLevelCap { get; private set; }
        public static int BodyguardLetInDurationLevelCap { get; private set; }
        #endregion

        public static bool BodyguardHired { get; private set; }
        public static int BodyguardHiredCost { get; private set; }
        // ##########
        public static float BodyguardStamina { get; private set; }
        public static int BodyguardStaminaLevel { get; private set; }
        public static int BodyguardStaminaCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, BodyguardStaminaLevel));
        // ##########
        public static float BodyguardLetInDuration { get; private set; } // level 30 max.
        public static int BodyguardLetInDurationLevel { get; private set; } // level 15 max.
        public static int BodyguardLetInDurationCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, BodyguardLetInDurationLevel));

        // cost data
        private static readonly int _upgradeCost = 30;
        private static readonly float _upgradeCostIncreaseRate = 1.3f;

        // core data
        private readonly float _coreBodyguardStamina = 5f;
        private readonly float _coreBodyguardLetInDuration = 5f;

        // increment data
        private readonly float _bodyguardStaminaIncrement = 1f;
        private readonly float _bodyguardLetInDurationDecrease = 0.28f;

        public void Start()
        {
            BodyguardHiredCost = 500;
            BodyguardStaminaLevelCap = 30;
            BodyguardLetInDurationLevelCap = 15;

            LoadData();
            
            UpdateBodyguardStamina();
            UpdateBodyguardSpeed();
            UpdateBodyguardHire();
            
            gateUpgradeCanvas.Init(this);

            GateUpgradeEvents.OnUpgradeBodyguardHire += BodyguardHireUpgrade;
            GateUpgradeEvents.OnUpgradeBodyguardStamina += BodyguardStaminaUpgrade;
            GateUpgradeEvents.OnUpgradeBodyguardSpeed += BodyguardSpeedUpgrade;
        }

        private void OnDisable()
        {
            GateUpgradeEvents.OnUpgradeBodyguardHire -= BodyguardHireUpgrade;
            GateUpgradeEvents.OnUpgradeBodyguardStamina -= BodyguardStaminaUpgrade;
            GateUpgradeEvents.OnUpgradeBodyguardSpeed -= BodyguardSpeedUpgrade;

            SaveData();
        }

        #region UPGRADE FUNCTIONS
        private void BodyguardHireUpgrade()
        {
            IncreaseBodyguardHireLevel();
            UpdateBodyguardHire();
        }
        private void BodyguardStaminaUpgrade()
        {
            IncreaseBodyguardStaminaLevel();
            UpdateBodyguardStamina();
            //PlayerEvents.OnCheer?.Invoke();
        }
        private void BodyguardSpeedUpgrade()
        {
            IncreaseBodyguardSpeedLevel();
            UpdateBodyguardSpeed();
            //PlayerEvents.OnCheer?.Invoke();
        }
        #endregion

        #region UPDATE FUNCTIONS
        private void UpdateBodyguardHire()
        {
            if (BodyguardHired)
            {
                bodyguard.gameObject.SetActive(true);
                bodyguard.Init(this);
            }
            else
                bodyguard.gameObject.SetActive(false);
        }
        private void UpdateBodyguardStamina()
        {
            BodyguardStamina = _coreBodyguardStamina + _bodyguardStaminaIncrement * (BodyguardStaminaLevel - 1);
            GateEvents.OnSetCurrentBodyguardStamina?.Invoke();
        }
        private void UpdateBodyguardSpeed()
        {
            BodyguardLetInDuration = _coreBodyguardLetInDuration - _bodyguardLetInDurationDecrease * (BodyguardLetInDurationLevel - 1);
            GateEvents.OnSetCurrentBodyguardSpeed?.Invoke();
        }
        #endregion

        #region INCREMENT FUNCTIONS
        private void IncreaseBodyguardHireLevel()
        {
            if (DataManager.TotalMoney >= BodyguardHiredCost)
            {
                BodyguardHired = true;
                GateEvents.OnBodyguardHired?.Invoke();

                CollectableEvents.OnSpend?.Invoke(BodyguardHiredCost);
                GateUpgradeEvents.OnUpdateUpgradeTexts?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            }
        }
        private void IncreaseBodyguardStaminaLevel()
        {
            if (DataManager.TotalMoney >= BodyguardStaminaCost)
            {
                CollectableEvents.OnSpend?.Invoke(BodyguardStaminaCost);
                BodyguardStaminaLevel++;
                GateUpgradeEvents.OnUpdateUpgradeTexts?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            }
        }
        private void IncreaseBodyguardSpeedLevel()
        {
            if (DataManager.TotalMoney >= BodyguardLetInDurationCost)
            {
                CollectableEvents.OnSpend?.Invoke(BodyguardLetInDurationCost);
                BodyguardLetInDurationLevel++;
                GateUpgradeEvents.OnUpdateUpgradeTexts?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            }
        }
        #endregion

        #region SAVE-LOAD
        private void LoadData()
        {
            BodyguardHired = PlayerPrefs.GetInt("BodyguardHired", 0) == 1 ? true : false;
            BodyguardStaminaLevel = PlayerPrefs.GetInt("BodyguardStaminaLevel", 1);
            BodyguardLetInDurationLevel = PlayerPrefs.GetInt("BodyguardSpeedLevel", 1);
        }
        private void SaveData()
        {
            PlayerPrefs.SetInt("BodyguardHired", BodyguardHired == true ? 1 : 0);
            PlayerPrefs.SetInt("BodyguardStaminaLevel", BodyguardStaminaLevel);
            PlayerPrefs.SetInt("BodyguardSpeedLevel", BodyguardLetInDurationLevel);
            PlayerPrefs.Save();
        }
        //private void OnApplicationPause(bool pause) => SaveData();
        //private void OnApplicationQuit() => SaveData();
        #endregion
    }
}
