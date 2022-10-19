using System.Collections.Generic;
using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class Toilet : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private Cleaner cleaner;
        [SerializeField] private Transform cleanerExitTransform;
        
        [Header("-- TOILET UPGRADE SETUP --")]
        [SerializeField] private ToiletUpgradeCanvas toiletUpgradeCanvas;
        [SerializeField] private ToiletUpgradeArea toiletUpgradeArea;

        [Header("-- CLEANER HIRE SETUP --")]
        [SerializeField] private CleanerHireCanvas cleanerHireCanvas;
        [SerializeField] private CleanerHireArea cleanerHireArea;

        #region PROPERTIES
        public static Transform ExitTransform { get; private set; }
        public static bool CanCleanerFixToilet => BrokenToiletItems.Count > 0f;
        public static int FixCount { get; private set; }
        #endregion

        #region UPGRADE LEVEL CAP
        public static int CleanerStaminaLevelCap { get; private set; }
        public static int CleanerSpeedLevelCap { get; private set; }
        public static int ToiletDurationLevelCap { get; private set; }
        #endregion

        #region UPGRADE SECTION
        public static bool CleanerHired { get; private set; }
        public static int CleanerHiredCost { get; private set; }
        // ##########
        public static float CleanerStamina { get; private set; }
        public static int CleanerStaminaLevel { get; private set; }
        public static int CleanerStaminaCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, CleanerStaminaLevel));
        // ##########
        public static float CleanerSpeed { get; private set; }
        public static int CleanerSpeedLevel { get; private set; }
        public static int CleanerSpeedCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, CleanerSpeedLevel));
        // ##########
        public static int ToiletDuration { get; private set; }
        public static int ToiletDurationLevel { get; private set; }
        public static int ToiletDurationCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, ToiletDurationLevel) * 3f);

        // cost data
        private static readonly int _upgradeCost = 100;
        private static readonly float _upgradeCostIncreaseRate = 1.3f;

        // core data
        private readonly float _coreCleanerStamina = 3f;
        private readonly float _coreCleanerSpeed = 1f;
        private readonly int _coreToiletDuration = 2;

        // increment data
        private readonly float _cleanerStaminaIncrement = 1f;
        private readonly float _cleanerSpeedIncrement = 0.2f;
        private readonly int _toiletDurationIncrement = 1;
        #endregion

        #region BROKEN TOILET SECTION
        private static List<ToiletItem> _brokenToiletItems;
        public static List<ToiletItem> BrokenToiletItems => _brokenToiletItems == null ? _brokenToiletItems = new List<ToiletItem>() : _brokenToiletItems;
        public static void AddBrokenToiletItem(ToiletItem toiletItem)
        {
            if (!BrokenToiletItems.Contains(toiletItem))
            {
                BrokenToiletItems.Add(toiletItem);
            }
        }
        public static void RemoveBrokenToiletItem(ToiletItem toiletItem)
        {
            if (BrokenToiletItems.Contains(toiletItem))
            {
                BrokenToiletItems.Remove(toiletItem);
            }
        }
        #endregion

        #region EMPTY TOILET SECTION
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
            CleanerHiredCost = 500;
            FixCount = 5;
            ExitTransform = cleanerExitTransform;
            CleanerStaminaLevelCap = 20;
            CleanerSpeedLevelCap = 10;
            ToiletDurationLevelCap = 5;

            LoadData();

            UpdateCleanerStamina();
            UpdateCleanerSpeed();
            UpdateToiletDuration();
            UpdateCleanerHire();

            CheckForCleanerActivation();

            ToiletUpgradeEvents.OnUpgradeCleanerHire += CleanerHireUpgrade;
            ToiletUpgradeEvents.OnUpgradeCleanerStamina += CleanerStaminaUpgrade;
            ToiletUpgradeEvents.OnUpgradeCleanerSpeed += CleanerSpeedUpgrade;
            ToiletUpgradeEvents.OnUpgradeToiletDuration += ToiletDurationUpgrade;
        }

        private void OnDisable()
        {
            ToiletUpgradeEvents.OnUpgradeCleanerHire -= CleanerHireUpgrade;
            ToiletUpgradeEvents.OnUpgradeCleanerStamina -= CleanerStaminaUpgrade;
            ToiletUpgradeEvents.OnUpgradeCleanerSpeed -= CleanerSpeedUpgrade;
            ToiletUpgradeEvents.OnUpgradeToiletDuration -= ToiletDurationUpgrade;

            SaveData();
        }

        private void CheckForCleanerActivation()
        {
            if (CleanerHired)
                CleanerIsHired();
            else
                CleanerIsNotHired();
        }
        private void CleanerIsNotHired()
        {
            cleanerHireCanvas.gameObject.SetActive(true);
            cleanerHireCanvas.Init(this);
            cleanerHireArea.gameObject.SetActive(true);
            cleanerHireArea.Init(this);

            toiletUpgradeCanvas.gameObject.SetActive(false);
            toiletUpgradeArea.gameObject.SetActive(false);
        }

        #region PUBLICS
        public void CleanerIsHired()
        {
            cleanerHireCanvas.gameObject.SetActive(false);
            cleanerHireArea.gameObject.SetActive(false);

            toiletUpgradeCanvas.gameObject.SetActive(true);
            toiletUpgradeCanvas.Init(this);
            toiletUpgradeArea.gameObject.SetActive(true);
        }
        #endregion

        #region UPGRADE FUNCTIONS
        private void CleanerHireUpgrade()
        {
            IncreaseCleanerHireLevel();
            UpdateCleanerHire();
        }
        private void CleanerStaminaUpgrade()
        {
            IncreaseCleanerStaminaLevel();
            UpdateCleanerStamina();
            //PlayerEvents.OnCheer?.Invoke();
        }
        private void CleanerSpeedUpgrade()
        {
            IncreaseCleanerSpeedLevel();
            UpdateCleanerSpeed();
            //PlayerEvents.OnCheer?.Invoke();
        }
        private void ToiletDurationUpgrade()
        {
            IncreaseToiletDurationLevel();
            UpdateToiletDuration();
        }
        #endregion

        #region UPDATE FUNCTIONS
        private void UpdateCleanerHire()
        {
            if (CleanerHired)
            {
                cleaner.gameObject.SetActive(true);
                cleaner.Init(this);
            }
            else
                cleaner.gameObject.SetActive(false);
        }
        private void UpdateCleanerStamina()
        {
            CleanerStamina = _coreCleanerStamina + _cleanerStaminaIncrement * (CleanerStaminaLevel - 1);
            ToiletEvents.OnSetCurrentCleanerStamina?.Invoke();
        }
        private void UpdateCleanerSpeed()
        {
            CleanerSpeed = _coreCleanerSpeed + _cleanerSpeedIncrement * (CleanerSpeedLevel - 1);
            ToiletEvents.OnSetCurrentCleanerSpeed?.Invoke();
        }
        private void UpdateToiletDuration()
        {
            ToiletDuration = _coreToiletDuration + _toiletDurationIncrement * (ToiletDurationLevel - 1);
            ToiletEvents.OnSetCurrentToiletDuration?.Invoke();
        }
        #endregion

        #region INCREMENT FUNCTIONS
        private void IncreaseCleanerHireLevel()
        {
            if (DataManager.TotalMoney >= CleanerHiredCost)
            {
                CleanerHired = true;
                ToiletEvents.OnCleanerHired?.Invoke();

                CollectableEvents.OnSpend?.Invoke(CleanerHiredCost);
                ToiletUpgradeEvents.OnUpdateUpgradeTexts?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            }
        }
        private void IncreaseCleanerStaminaLevel()
        {
            if (DataManager.TotalMoney >= CleanerStaminaCost)
            {
                CollectableEvents.OnSpend?.Invoke(CleanerStaminaCost);
                CleanerStaminaLevel++;
                ToiletUpgradeEvents.OnUpdateUpgradeTexts?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            }
        }
        private void IncreaseCleanerSpeedLevel()
        {
            if (DataManager.TotalMoney >= CleanerSpeedCost)
            {
                CollectableEvents.OnSpend?.Invoke(CleanerSpeedCost);
                CleanerSpeedLevel++;
                ToiletUpgradeEvents.OnUpdateUpgradeTexts?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            }
        }
        private void IncreaseToiletDurationLevel()
        {
            if (DataManager.TotalMoney >= ToiletDurationCost)
            {
                CollectableEvents.OnSpend?.Invoke(ToiletDurationCost);
                ToiletDurationLevel++;
                ToiletUpgradeEvents.OnUpdateUpgradeTexts?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            }
        }
        #endregion

        #region SAVE-LOAD
        private void LoadData()
        {
            CleanerHired = PlayerPrefs.GetInt("CleanerHired", 0) == 1 ? true : false;
            CleanerStaminaLevel = PlayerPrefs.GetInt("CleanerStaminaLevel", 1);
            CleanerSpeedLevel = PlayerPrefs.GetInt("CleanerSpeedLevel", 1);
            ToiletDurationLevel = PlayerPrefs.GetInt("ToiletDurationLevel", 1);
        }
        private void SaveData()
        {
            PlayerPrefs.SetInt("CleanerHired", CleanerHired == true ? 1 : 0);
            PlayerPrefs.SetInt("CleanerStaminaLevel", CleanerStaminaLevel);
            PlayerPrefs.SetInt("CleanerSpeedLevel", CleanerSpeedLevel);
            PlayerPrefs.SetInt("ToiletDurationLevel", ToiletDurationLevel);
            PlayerPrefs.Save();
        }
        //private void OnApplicationPause(bool pause) => SaveData();
        //private void OnApplicationQuit() => SaveData();
        #endregion
    }
}
