using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class GateUpgradeCanvas : MonoBehaviour
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
        [SerializeField] private UpgradeCanvasItem bodyguardHire;
        [SerializeField] private UpgradeCanvasItem bodyguardStamina;
        [SerializeField] private UpgradeCanvasItem bodyguardSpeed;

        public void Init(Gate gate)
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
                if (_currentType == Type.Idle)
                {
                    _closeButton = transform.GetChild(0).GetChild(0).GetComponent<CustomButton>();
                    _emptySpaceButton = transform.GetChild(1).GetComponent<CustomButton>();
                }

                bodyguardHire.Init();
                bodyguardStamina.Init();
                bodyguardSpeed.Init();
            }

            //Delayer.DoActionAfterDelay(this, 0.5f, UpdateTexts);
            UpdateTexts();

            IsOpen = false;

            if (_currentType == Type.Idle)
            {
                _closeButton.onClick.AddListener(CloseCanvasClicked);
                _emptySpaceButton.onClick.AddListener(CloseCanvasClicked);
            }

            bodyguardHire.Button.onClick.AddListener(BodyguardHireUpgradeClicked);
            bodyguardStamina.Button.onClick.AddListener(BodyguardStaminaUpgradeClicked);
            bodyguardSpeed.Button.onClick.AddListener(BodyguardSpeedUpgradeClicked);

            GateUpgradeEvents.OnUpdateUpgradeTexts += UpdateTexts;
            
            GateUpgradeEvents.OnOpenCanvas += EnableCanvas;
            GateUpgradeEvents.OnCloseCanvas += DisableCanvas;
        }

        private void OnDisable()
        {
            if (_currentType == Type.Idle)
            {
                _closeButton.onClick.RemoveListener(CloseCanvasClicked);
                _emptySpaceButton.onClick.RemoveListener(CloseCanvasClicked);
            }

            bodyguardHire.Button.onClick.RemoveListener(BodyguardHireUpgradeClicked);
            bodyguardStamina.Button.onClick.RemoveListener(BodyguardStaminaUpgradeClicked);
            bodyguardSpeed.Button.onClick.RemoveListener(BodyguardSpeedUpgradeClicked);

            GateUpgradeEvents.OnUpdateUpgradeTexts -= UpdateTexts;

            GateUpgradeEvents.OnOpenCanvas -= EnableCanvas;
            GateUpgradeEvents.OnCloseCanvas -= DisableCanvas;
        }

        #region UPDATERS
        private void UpdateTexts()
        {
            #region BODYGUARD HIRE
            if (Gate.BodyguardHired)
            {
                bodyguardHire.Button.gameObject.SetActive(false);
                bodyguardHire.LevelText.text = "ALREADY HIRED!";
            }
            else
            {
                bodyguardHire.Button.gameObject.SetActive(true);
                bodyguardHire.LevelText.text = "";
                bodyguardHire.CostText.text = Gate.BodyguardHiredCost.ToString();
            }
            #endregion

            if (_currentType == Type.Idle)
                bodyguardStamina.LevelText.text = $"Level {Gate.BodyguardStaminaLevel}";
            else
                bodyguardStamina.LevelText.text = Gate.BodyguardStaminaLevel.ToString();
            
            bodyguardStamina.CostText.text = Gate.BodyguardStaminaCost.ToString();

            if (_currentType == Type.Idle)
                bodyguardSpeed.LevelText.text = $"Level {Gate.BodyguardSpeedLevel}";
            else
                bodyguardSpeed.LevelText.text = Gate.BodyguardSpeedLevel.ToString();

            bodyguardSpeed.CostText.text = Gate.BodyguardSpeedCost.ToString();

            CheckForMoneySufficiency();
        }

        private void CheckForMoneySufficiency()
        {
            bodyguardHire.Button.interactable = DataManager.TotalMoney >= Gate.BodyguardHiredCost && !Gate.BodyguardHired;
            bodyguardStamina.Button.interactable = DataManager.TotalMoney >= Gate.BodyguardStaminaCost && Gate.BodyguardHired;
            bodyguardSpeed.Button.interactable = DataManager.TotalMoney >= Gate.BodyguardSpeedCost && Gate.BodyguardHired;
        }
        #endregion

        #region UPGRADE FUNCTIONS
        private void CloseCanvas()
        {
            GateUpgradeEvents.OnCloseCanvas?.Invoke();
            PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
        }
        private void UpgradeBodyguardHire() => GateUpgradeEvents.OnUpgradeBodyguardHire?.Invoke();
        private void UpgradeBodyguardStamina() => GateUpgradeEvents.OnUpgradeBodyguardStamina?.Invoke();
        private void UpgradeBodyguardSpeed() => GateUpgradeEvents.OnUpgradeBodyguardSpeed?.Invoke();
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
        private void BodyguardHireUpgradeClicked() => bodyguardHire.Button.TriggerClick(UpgradeBodyguardHire);
        private void BodyguardStaminaUpgradeClicked() => bodyguardStamina.Button.TriggerClick(UpgradeBodyguardStamina);
        private void BodyguardSpeedUpgradeClicked() => bodyguardSpeed.Button.TriggerClick(UpgradeBodyguardSpeed);
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
