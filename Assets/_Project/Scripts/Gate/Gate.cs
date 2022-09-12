using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class Gate : MonoBehaviour
    {
        public static bool BodyguardHired { get; private set; }
        public static int BodyguardHiredCost { get; private set; }
        // ##########
        public static float BodyguardStamina { get; private set; }
        public static int BodyguardStaminaLevel { get; private set; }
        public static int BodyguardStaminaCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, BodyguardStaminaLevel));
        // ##########
        public static float BodyguardSpeed { get; private set; }
        public static int BodyguardSpeedLevel { get; private set; }
        public static int BodyguardSpeedCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, BodyguardSpeedLevel));

        // cost data
        private static readonly int _upgradeCost = 30;
        private static readonly float _upgradeCostIncreaseRate = 1.2f;

        // core data
        private readonly float _coreBodyguardStamina = 50f;
        private readonly float _coreBodyguardSpeed = 3f;

        // increment data
        private readonly float _bodyguardStaminaIncrement = 5f;
        private readonly float _bodyguardSpeedIncrement = 0.5f;

        public void Init(Gate gate)
        {
            LoadData();

            UpdateBodyguardHire();
            UpdateBodyguardStamina();
            UpdateBodyguardSpeed();

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
            BodyguardHired = true;
            GateEvents.OnBodyguardHired?.Invoke();
        }
        private void UpdateBodyguardStamina()
        {
            BodyguardStamina = _coreBodyguardStamina + _bodyguardStaminaIncrement * (BodyguardStaminaLevel - 1);
            GateEvents.OnSetCurrentBodyguardStamina?.Invoke();
        }
        private void UpdateBodyguardSpeed()
        {
            BodyguardSpeed = _coreBodyguardSpeed + _bodyguardSpeedIncrement * (BodyguardSpeedLevel - 1);
            GateEvents.OnSetCurrentBodyguardSpeed?.Invoke();
        }
        #endregion

        #region INCREMENT FUNCTIONS
        private void IncreaseBodyguardHireLevel()
        {
            if (DataManager.TotalMoney >= BodyguardHiredCost)
            {
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
            if (DataManager.TotalMoney >= BodyguardSpeedCost)
            {
                CollectableEvents.OnSpend?.Invoke(BodyguardSpeedCost);
                BodyguardSpeedLevel++;
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
            BodyguardSpeedLevel = PlayerPrefs.GetInt("BodyguardSpeedLevel", 1);
        }
        private void SaveData()
        {
            PlayerPrefs.SetInt("BodyguardHired", BodyguardHired == true ? 1 : 0);
            PlayerPrefs.SetInt("BodyguardStaminaLevel", BodyguardStaminaLevel);
            PlayerPrefs.SetInt("BodyguardSpeedLevel", BodyguardSpeedLevel);
            PlayerPrefs.Save();
        }
        private void OnApplicationPause(bool pause) => SaveData();
        private void OnApplicationQuit() => SaveData();
        #endregion
    }
}
