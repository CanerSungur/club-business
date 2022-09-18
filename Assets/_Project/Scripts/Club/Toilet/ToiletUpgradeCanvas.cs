using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class ToiletUpgradeCanvas : MonoBehaviour
    {
        public enum Type { Idle, Incremental }
        [SerializeField] private Type _currentType;
        public Type CurrentType => _currentType;

        private CustomButton _closeButton, _emptySpaceButton;
        public static bool IsOpen { get; private set; }

        #region ANIMATION
        private Animator _animator;
        private readonly int _openID = Animator.StringToHash("Open");
        private readonly int _closeID = Animator.StringToHash("Close");
        #endregion

        [Header("-- STAMINA SETUP --")]
        [SerializeField] private UpgradeCanvasItem cleanerHire;
        [SerializeField] private UpgradeCanvasItem cleanerStamina;
        [SerializeField] private UpgradeCanvasItem cleanerSpeed;
        [SerializeField] private UpgradeCanvasItem toiletDuration;

        public void Init(Toilet toilet)
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
                if (_currentType == Type.Idle)
                {
                    _closeButton = transform.GetChild(0).GetChild(0).GetComponent<CustomButton>();
                    _emptySpaceButton = transform.GetChild(1).GetComponent<CustomButton>();
                }

                cleanerHire.Init();
                cleanerStamina.Init();
                cleanerSpeed.Init();
                toiletDuration.Init();
            }

            //Delayer.DoActionAfterDelay(this, 0.5f, UpdateTexts);
            UpdateTexts();

            IsOpen = false;

            if (_currentType == Type.Idle)
            {
                _closeButton.onClick.AddListener(CloseCanvasClicked);
                _emptySpaceButton.onClick.AddListener(CloseCanvasClicked);
            }

            cleanerHire.Button.onClick.AddListener(CleanerHireUpgradeClicked);
            cleanerStamina.Button.onClick.AddListener(CleanerStaminaUpgradeClicked);
            cleanerSpeed.Button.onClick.AddListener(CleanerSpeedUpgradeClicked);
            toiletDuration.Button.onClick.AddListener(ToiletDurationUpgradeClicked);

            ToiletUpgradeEvents.OnUpdateUpgradeTexts += UpdateTexts;

            ToiletUpgradeEvents.OnOpenCanvas += EnableCanvas;
            ToiletUpgradeEvents.OnCloseCanvas += DisableCanvas;
        }

        private void OnDisable()
        {
            if (_currentType == Type.Idle)
            {
                _closeButton.onClick.RemoveListener(CloseCanvasClicked);
                _emptySpaceButton.onClick.RemoveListener(CloseCanvasClicked);
            }

            cleanerHire.Button.onClick.RemoveListener(CleanerHireUpgradeClicked);
            cleanerStamina.Button.onClick.RemoveListener(CleanerStaminaUpgradeClicked);
            cleanerSpeed.Button.onClick.RemoveListener(CleanerSpeedUpgradeClicked);
            toiletDuration.Button.onClick.RemoveListener(ToiletDurationUpgradeClicked);

            ToiletUpgradeEvents.OnUpdateUpgradeTexts -= UpdateTexts;

            ToiletUpgradeEvents.OnOpenCanvas -= EnableCanvas;
            ToiletUpgradeEvents.OnCloseCanvas -= DisableCanvas;
        }

        #region UPDATERS
        private void UpdateTexts()
        {
            #region CLEANER HIRE
            if (Toilet.CleanerHired)
            {
                cleanerHire.Button.gameObject.SetActive(false);
                cleanerHire.LevelText.text = "ALREADY HIRED!";
            }
            else
            {
                cleanerHire.Button.gameObject.SetActive(true);
                cleanerHire.LevelText.text = "";
                cleanerHire.CostText.text = Toilet.CleanerHiredCost.ToString();
            }
            #endregion

            #region CLEANER STAMINA
            if (Toilet.CleanerStaminaLevel >= Toilet.CleanerStaminaLevelCap)
            {
                cleanerStamina.Button.gameObject.SetActive(false);
                cleanerStamina.LevelText.text = "MAX LEVEL!";
            }
            else
            {
                cleanerStamina.Button.gameObject.SetActive(true);
                if (_currentType == Type.Idle)
                    cleanerStamina.LevelText.text = $"Level {Toilet.CleanerStaminaLevel}";
                else
                    cleanerStamina.LevelText.text = Toilet.CleanerStaminaLevel.ToString();
                cleanerStamina.CostText.text = Toilet.CleanerStaminaCost.ToString();
            }
            #endregion


            #region CLEANER SPEED
            if (Toilet.CleanerSpeedLevel >= Toilet.CleanerSpeedLevelCap)
            {
                cleanerSpeed.Button.gameObject.SetActive(false);
                cleanerSpeed.LevelText.text = "MAX LEVEL!";
            }
            else
            {
                cleanerSpeed.Button.gameObject.SetActive(true);
                if (_currentType == Type.Idle)
                    cleanerSpeed.LevelText.text = $"Level {Toilet.CleanerSpeedLevel}";
                else
                    cleanerSpeed.LevelText.text = Toilet.CleanerSpeedLevel.ToString();
                cleanerSpeed.CostText.text = Toilet.CleanerSpeedCost.ToString();
            }
            #endregion

            #region TOILET DURATION
            if (Toilet.ToiletDurationLevel >= Toilet.ToiletDurationLevelCap)
            {
                toiletDuration.Button.gameObject.SetActive(false);
                toiletDuration.LevelText.text = "MAX LEVEL!";
            }
            else
            {
                toiletDuration.Button.gameObject.SetActive(true);
                if (_currentType == Type.Idle)
                    toiletDuration.LevelText.text = $"Level {Toilet.ToiletDurationLevel}";
                else
                    toiletDuration.LevelText.text = Toilet.ToiletDurationLevel.ToString();
                toiletDuration.CostText.text = Toilet.ToiletDurationCost.ToString();
            }
            #endregion

            CheckForMoneySufficiency();
        }

        private void CheckForMoneySufficiency()
        {
            cleanerHire.Button.interactable = DataManager.TotalMoney >= Toilet.CleanerHiredCost && !Toilet.CleanerHired;
            cleanerStamina.Button.interactable = DataManager.TotalMoney >= Toilet.CleanerStaminaCost && Toilet.CleanerHired && Toilet.CleanerStaminaLevel < Toilet.CleanerStaminaLevelCap;
            cleanerSpeed.Button.interactable = DataManager.TotalMoney >= Toilet.CleanerSpeedCost && Toilet.CleanerHired && Toilet.CleanerSpeedLevel < Toilet.CleanerSpeedLevelCap;
            toiletDuration.Button.interactable = DataManager.TotalMoney >= Toilet.ToiletDuration && Toilet.ToiletDurationLevel < Toilet.ToiletDurationLevelCap;
        }
        #endregion

        #region UPGRADE FUNCTIONS
        private void CloseCanvas()
        {
            ToiletUpgradeEvents.OnCloseCanvas?.Invoke();
            PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
        }
        private void UpgradeCleanerHire() => ToiletUpgradeEvents.OnUpgradeCleanerHire?.Invoke();
        private void UpgradeCleanerStamina() => ToiletUpgradeEvents.OnUpgradeCleanerStamina?.Invoke();
        private void UpgradeCleanerSpeed() => ToiletUpgradeEvents.OnUpgradeCleanerSpeed?.Invoke();
        private void UpgradeToiletDuration() => ToiletUpgradeEvents.OnUpgradeToiletDuration?.Invoke();
        #endregion

        #region CLICK TRIGGER FUNCTIONS
        private void CloseCanvasClicked()
        {
            if (_currentType == Type.Idle)
            {
                _closeButton.interactable = _emptySpaceButton.interactable = false;
                _closeButton.TriggerClick(CloseCanvas);
            }
        }
        private void CleanerHireUpgradeClicked() => cleanerHire.Button.TriggerClick(UpgradeCleanerHire);
        private void CleanerStaminaUpgradeClicked() => cleanerStamina.Button.TriggerClick(UpgradeCleanerStamina);
        private void CleanerSpeedUpgradeClicked() => cleanerSpeed.Button.TriggerClick(UpgradeCleanerSpeed);
        private void ToiletDurationUpgradeClicked() => toiletDuration.Button.TriggerClick(UpgradeToiletDuration);
        #endregion

        #region ANIMATOR FUNCTIONS
        private void EnableCanvas()
        {
            AudioHandler.PlayAudio(Enums.AudioType.UpgradeMenu);

            if (_currentType == Type.Idle)
                _closeButton.interactable = _emptySpaceButton.interactable = true;

            _animator.SetTrigger(_openID);
            IsOpen = true;

            CheckForMoneySufficiency();
        }
        private void DisableCanvas()
        {
            AudioHandler.PlayAudio(Enums.AudioType.UpgradeMenu);

            if (_currentType == Type.Idle)
                _closeButton.interactable = _emptySpaceButton.interactable = false;

            _animator.SetTrigger(_closeID);
            IsOpen = false;
        }
        #endregion
    }
}
