using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BarUpgradeCanvas : MonoBehaviour
    {
        private Bar _bar;
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
        //[SerializeField] private UpgradeCanvasItem bartenderHire;
        [SerializeField] private UpgradeCanvasItem bartenderStamina;
        [SerializeField] private UpgradeCanvasItem bartenderSpeed;

        public void Init(Bar bar)
        {
            if (_animator == null)
            {
                _bar = bar;
                _animator = GetComponent<Animator>();
                if (_currentType == Type.Idle)
                {
                    _closeButton = transform.GetChild(0).GetChild(0).GetComponent<CustomButton>();
                    _emptySpaceButton = transform.GetChild(1).GetComponent<CustomButton>();
                }

                //bartenderHire.Init();
                bartenderStamina.Init();
                bartenderSpeed.Init();
            }

            //Delayer.DoActionAfterDelay(this, 0.5f, UpdateTexts);
            UpdateTexts();

            IsOpen = false;

            if (_currentType == Type.Idle)
            {
                _closeButton.onClick.AddListener(CloseCanvasClicked);
                _emptySpaceButton.onClick.AddListener(CloseCanvasClicked);
            }

            //bartenderHire.Button.onClick.AddListener(BartenderHireUpgradeClicked);
            bartenderStamina.Button.onClick.AddListener(BartenderStaminaUpgradeClicked);
            bartenderSpeed.Button.onClick.AddListener(BartenderSpeedUpgradeClicked);

            BarUpgradeEvents.OnUpdateUpgradeTexts += UpdateTexts;

            BarUpgradeEvents.OnOpenCanvas += EnableCanvas;
            BarUpgradeEvents.OnCloseCanvas += DisableCanvas;
        }

        private void OnDisable()
        {
            if (_bar == null) return;

            if (_currentType == Type.Idle)
            {
                _closeButton.onClick.RemoveListener(CloseCanvasClicked);
                _emptySpaceButton.onClick.RemoveListener(CloseCanvasClicked);
            }

            //bartenderHire.Button.onClick.RemoveListener(BartenderHireUpgradeClicked);
            bartenderStamina.Button.onClick.RemoveListener(BartenderStaminaUpgradeClicked);
            bartenderSpeed.Button.onClick.RemoveListener(BartenderSpeedUpgradeClicked);

            BarUpgradeEvents.OnUpdateUpgradeTexts -= UpdateTexts;

            BarUpgradeEvents.OnOpenCanvas -= EnableCanvas;
            BarUpgradeEvents.OnCloseCanvas -= DisableCanvas;
        }

        #region UPDATERS
        private void UpdateTexts()
        {
            #region BARTENDER HIRE
            //if (Bar.BartenderHired)
            //{
                //bartenderHire.Button.gameObject.SetActive(false);
                //bartenderHire.LevelText.text = "HIRED!";
            //}
            //else
            //{
                //bartenderHire.Button.gameObject.SetActive(true);
                //bartenderHire.LevelText.text = "";
                //bartenderHire.CostText.text = Bar.BartenderHiredCost.ToString();
            //}
            #endregion

            #region BARTENDER STAMINA
            if (Bar.BartenderStaminaLevel >= Bar.BartenderStaminaLevelCap)
            {
                bartenderStamina.Button.gameObject.SetActive(false);
                bartenderStamina.LevelText.text = "MAX LEVEL!";
            }
            else
            {
                bartenderStamina.Button.gameObject.SetActive(true);
                if (_currentType == Type.Idle)
                    bartenderStamina.LevelText.text = $"Level {Bar.BartenderStaminaLevel}";
                else
                    bartenderStamina.LevelText.text = Bar.BartenderStaminaLevel.ToString();
                bartenderStamina.CostText.text = Bar.BartenderStaminaCost.ToString();
            }
            #endregion

            #region BARTENDER SPEED
            if (Bar.BartenderPourDurationLevel >= Bar.BartenderPourDurationLevelCap)
            {
                bartenderSpeed.Button.gameObject.SetActive(false);
                bartenderSpeed.LevelText.text = "MAX LEVEL!";
            }
            else
            {
                bartenderSpeed.Button.gameObject.SetActive(true);
                if (_currentType == Type.Idle)
                    bartenderSpeed.LevelText.text = $"Level {Bar.BartenderPourDurationLevel}";
                else
                    bartenderSpeed.LevelText.text = Bar.BartenderPourDurationLevel.ToString();
                bartenderSpeed.CostText.text = Bar.BartenderPourDurationCost.ToString();
            }
            #endregion

            CheckForMoneySufficiency();
        }

        private void CheckForMoneySufficiency()
        {
            //bartenderHire.Button.interactable = DataManager.TotalMoney >= Bar.BartenderHiredCost && !Bar.BartenderHired;
            bartenderStamina.Button.interactable = DataManager.TotalMoney >= Bar.BartenderStaminaCost && Bar.BartenderHired && Bar.BartenderStaminaLevel < Bar.BartenderStaminaLevelCap;
            bartenderSpeed.Button.interactable = DataManager.TotalMoney >= Bar.BartenderPourDurationCost && Bar.BartenderHired && Bar.BartenderPourDurationLevel < Bar.BartenderPourDurationLevelCap;
        }
        #endregion

        #region UPGRADE FUNCTIONS
        private void CloseCanvas()
        {
            BarUpgradeEvents.OnCloseCanvas?.Invoke();
            PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
        }
        //private void UpgradeBartenderHire() => BarUpgradeEvents.OnUpgradeBartenderHire?.Invoke();
        private void UpgradeBartenderStamina() => BarUpgradeEvents.OnUpgradeBartenderStamina?.Invoke();
        private void UpgradeBartenderSpeed() => BarUpgradeEvents.OnUpgradeBartenderSpeed?.Invoke();
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
        //private void BartenderHireUpgradeClicked() => bartenderHire.Button.TriggerClick(UpgradeBartenderHire);
        private void BartenderStaminaUpgradeClicked() => bartenderStamina.Button.TriggerClick(UpgradeBartenderStamina);
        private void BartenderSpeedUpgradeClicked() => bartenderSpeed.Button.TriggerClick(UpgradeBartenderSpeed);
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
