using UnityEngine;
using ZestCore.Utility;

namespace ZestGames
{
    public class UpgradeCanvas : MonoBehaviour
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
        [SerializeField] private UpgradeCanvasItem hireBodyguard;
        [SerializeField] private UpgradeCanvasItem bodyguardStamina;

        public void Init(UiManager uiManager)
        {
            if (!_animator)
            {
                _animator = GetComponent<Animator>();
                if (_currentType == Type.Idle)
                {
                    _closeButton = transform.GetChild(0).GetChild(0).GetComponent<CustomButton>();
                    _emptySpaceButton = transform.GetChild(1).GetComponent<CustomButton>();
                }

                hireBodyguard.Init();
                bodyguardStamina.Init();
            }

            Delayer.DoActionAfterDelay(this, 0.5f, UpdateTexts);

            IsOpen = false;

            if (_currentType == Type.Idle)
            {
                _closeButton.onClick.AddListener(CloseCanvasClicked);
                _emptySpaceButton.onClick.AddListener(CloseCanvasClicked);
            }

            hireBodyguard.Button.onClick.AddListener(MovementSpeedUpgradeClicked);
            bodyguardStamina.Button.onClick.AddListener(MoneyValueUpgradeClicked);

            PlayerUpgradeEvents.OnUpdateUpgradeTexts += UpdateTexts;

            PlayerUpgradeEvents.OnOpenCanvas += EnableCanvas;
            PlayerUpgradeEvents.OnCloseCanvas += DisableCanvas;
        }

        private void OnDisable()
        {
            if (_currentType == Type.Idle)
            {
                _closeButton.onClick.RemoveListener(CloseCanvasClicked);
                _emptySpaceButton.onClick.RemoveListener(CloseCanvasClicked);
            }

            hireBodyguard.Button.onClick.RemoveListener(MovementSpeedUpgradeClicked);
            bodyguardStamina.Button.onClick.RemoveListener(MoneyValueUpgradeClicked);

            PlayerUpgradeEvents.OnUpdateUpgradeTexts -= UpdateTexts;

            PlayerUpgradeEvents.OnOpenCanvas -= EnableCanvas;
            PlayerUpgradeEvents.OnCloseCanvas -= DisableCanvas;
        }

        #region UPDATERS
        private void UpdateTexts()
        {
            #region LIMITED MOVEMENT SPEED
            //if (DataManager.MovementSpeedLevel >= 13)
            //{
            //    movementSpeed.Button.gameObject.SetActive(false);
            //    movementSpeed.LevelText.text = "MAX LEVEL!";
            //}
            //else
            //{
            //    movementSpeed.Button.gameObject.SetActive(true);
            //    movementSpeed.LevelText.text = $"Level {DataManager.MovementSpeedLevel}";
            //    movementSpeed.CostText.text = DataManager.MovementSpeedCost.ToString();
            //}
            #endregion

            if (_currentType == Type.Idle)
                hireBodyguard.LevelText.text = $"Level {DataManager.MovementSpeedLevel}";
            else
                hireBodyguard.LevelText.text = DataManager.MovementSpeedLevel.ToString();
            hireBodyguard.CostText.text = DataManager.MovementSpeedCost.ToString();

            if (_currentType == Type.Idle)
                bodyguardStamina.LevelText.text = $"Level {DataManager.MoneyValueLevel}";
            else
                bodyguardStamina.LevelText.text = DataManager.MoneyValueLevel.ToString();
            bodyguardStamina.CostText.text = DataManager.MoneyValueCost.ToString();

            CheckForMoneySufficiency();
        }

        private void CheckForMoneySufficiency()
        {
            hireBodyguard.Button.interactable = DataManager.TotalMoney >= DataManager.MovementSpeedCost;
            bodyguardStamina.Button.interactable = DataManager.TotalMoney >= DataManager.MoneyValueCost;
            //bodyguardSpeed.Button.interactable = 
        }
        #endregion

        #region UPGRADE FUNCTIONS
        private void CloseCanvas()
        {
            PlayerUpgradeEvents.OnCloseCanvas?.Invoke();
            PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
        }
        private void UpgradeMovementSpeed() => PlayerUpgradeEvents.OnUpgradeMovementSpeed?.Invoke();
        private void UpgradeMoneyValue() => PlayerUpgradeEvents.OnUpgradeMoneyValue?.Invoke();
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
        private void MovementSpeedUpgradeClicked() => hireBodyguard.Button.TriggerClick(UpgradeMovementSpeed);
        private void MoneyValueUpgradeClicked() => bodyguardStamina.Button.TriggerClick(UpgradeMoneyValue);
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
