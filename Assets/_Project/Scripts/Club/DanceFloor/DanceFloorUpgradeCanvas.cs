using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class DanceFloorUpgradeCanvas : MonoBehaviour
    {
        private DanceFloor _danceFloor;
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
        //[SerializeField] private UpgradeCanvasItem bouncerHire;
        [SerializeField] private UpgradeCanvasItem bouncerStamina;
        [SerializeField] private UpgradeCanvasItem bouncerPower;

        public void Init(DanceFloor danceFloor)
        {
            if (_animator == null)
            {
                _danceFloor = danceFloor;
                _animator = GetComponent<Animator>();
                if (_currentType == Type.Idle)
                {
                    _closeButton = transform.GetChild(0).GetChild(0).GetComponent<CustomButton>();
                    _emptySpaceButton = transform.GetChild(1).GetComponent<CustomButton>();
                }

                //bouncerHire.Init();
                bouncerStamina.Init();
                bouncerPower.Init();
            }

            //Delayer.DoActionAfterDelay(this, 0.5f, UpdateTexts);
            UpdateTexts();

            IsOpen = false;

            if (_currentType == Type.Idle)
            {
                _closeButton.onClick.AddListener(CloseCanvasClicked);
                _emptySpaceButton.onClick.AddListener(CloseCanvasClicked);
            }

            //bouncerHire.Button.onClick.AddListener(BouncerHireUpgradeClicked);
            bouncerStamina.Button.onClick.AddListener(BouncerStaminaUpgradeClicked);
            bouncerPower.Button.onClick.AddListener(BouncerPowerUpgradeClicked);

            DanceFloorUpgradeEvents.OnUpdateUpgradeTexts += UpdateTexts;

            DanceFloorUpgradeEvents.OnOpenCanvas += EnableCanvas;
            DanceFloorUpgradeEvents.OnCloseCanvas += DisableCanvas;
        }

        private void OnDisable()
        {
            if (_danceFloor == null) return;

            if (_currentType == Type.Idle)
            {
                _closeButton.onClick.RemoveListener(CloseCanvasClicked);
                _emptySpaceButton.onClick.RemoveListener(CloseCanvasClicked);
            }

            //bouncerHire.Button.onClick.RemoveListener(BouncerHireUpgradeClicked);
            bouncerStamina.Button.onClick.RemoveListener(BouncerStaminaUpgradeClicked);
            bouncerPower.Button.onClick.RemoveListener(BouncerPowerUpgradeClicked);

            DanceFloorUpgradeEvents.OnUpdateUpgradeTexts -= UpdateTexts;

            DanceFloorUpgradeEvents.OnOpenCanvas -= EnableCanvas;
            DanceFloorUpgradeEvents.OnCloseCanvas -= DisableCanvas;
        }

        #region UPDATERS
        private void UpdateTexts()
        {
            #region BOUNCER HIRE
            //if (DanceFloor.BouncerHired)
            //{
            //    bouncerHire.Button.gameObject.SetActive(false);
            //    bouncerHire.LevelText.text = "HIRED!";
            //}
            //else
            //{
            //    bouncerHire.Button.gameObject.SetActive(true);
            //    bouncerHire.LevelText.text = "";
            //    bouncerHire.CostText.text = DanceFloor.BouncerHiredCost.ToString();
            //}
            #endregion

            #region BOUNCER STAMINA
            if (DanceFloor.BouncerStaminaLevel >= DanceFloor.BouncerStaminaLevelCap)
            {
                bouncerStamina.Button.gameObject.SetActive(false);
                bouncerStamina.LevelText.text = "MAX LEVEL!";
            }
            else
            {
                bouncerStamina.Button.gameObject.SetActive(true);
                if (_currentType == Type.Idle)
                    bouncerStamina.LevelText.text = $"Level {DanceFloor.BouncerStaminaLevel}";
                else
                    bouncerStamina.LevelText.text = DanceFloor.BouncerStaminaLevel.ToString();
                bouncerStamina.CostText.text = DanceFloor.BouncerStaminaCost.ToString();
            }
            #endregion

            #region BOUNCER POWER
            if (DanceFloor.BouncerPowerLevel >= DanceFloor.BouncerPowerLevelCap)
            {
                bouncerPower.Button.gameObject.SetActive(false);
                bouncerPower.LevelText.text = "MAX LEVEL!";
            }
            else
            {
                bouncerPower.Button.gameObject.SetActive(true);
                if (_currentType == Type.Idle)
                    bouncerPower.LevelText.text = $"Level {DanceFloor.BouncerPowerLevel}";
                else
                    bouncerPower.LevelText.text = DanceFloor.BouncerPowerLevel.ToString();
                bouncerPower.CostText.text = DanceFloor.BouncerPowerCost.ToString();
            }
            #endregion

            CheckForMoneySufficiency();
        }

        private void CheckForMoneySufficiency()
        {
            //bouncerHire.Button.interactable = DataManager.TotalMoney >= DanceFloor.BouncerHiredCost && !DanceFloor.BouncerHired;
            bouncerStamina.Button.interactable = DataManager.TotalMoney >= DanceFloor.BouncerStaminaCost && DanceFloor.BouncerHired && DanceFloor.BouncerStaminaLevel < DanceFloor.BouncerStaminaLevelCap;
            bouncerPower.Button.interactable = DataManager.TotalMoney >= DanceFloor.BouncerPowerCost && DanceFloor.BouncerHired && DanceFloor.BouncerPowerLevel < DanceFloor.BouncerPowerLevelCap;
        }
        #endregion

        #region UPGRADE FUNCTIONS
        private void CloseCanvas()
        {
            DanceFloorUpgradeEvents.OnCloseCanvas?.Invoke();
            PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
        }
        //private void UpgradeBouncerHire() => DanceFloorUpgradeEvents.OnUpgradeBouncerHire?.Invoke();
        private void UpgradeBouncerStamina() => DanceFloorUpgradeEvents.OnUpgradeBouncerStamina?.Invoke();
        private void UpgradeBouncerPower() => DanceFloorUpgradeEvents.OnUpgradeBouncerPower?.Invoke();
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
        //private void BouncerHireUpgradeClicked() => bouncerHire.Button.TriggerClick(UpgradeBouncerHire);
        private void BouncerStaminaUpgradeClicked() => bouncerStamina.Button.TriggerClick(UpgradeBouncerStamina);
        private void BouncerPowerUpgradeClicked() => bouncerPower.Button.TriggerClick(UpgradeBouncerPower);
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
