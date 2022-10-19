using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BodyguardHireCanvas : MonoBehaviour
    {
        private Gate _gate;

        private CustomButton _closeButton, _emptySpaceButton;
        public static bool IsOpen { get; private set; }

        #region ANIMATION
        private Animator _animator;
        private readonly int _openID = Animator.StringToHash("Open");
        private readonly int _closeID = Animator.StringToHash("Close");
        #endregion

        [Header("-- SETUP --")]
        [SerializeField] private UpgradeCanvasItem bodyguardHire;

        public void Init(Gate gate)
        {
            if (_animator == null)
            {
                _gate = gate;
                _animator = GetComponent<Animator>();
                _closeButton = transform.GetChild(0).GetChild(0).GetComponent<CustomButton>();
                _emptySpaceButton = transform.GetChild(1).GetComponent<CustomButton>();

                bodyguardHire.Init();
            }

            UpdateTexts();
            IsOpen = false;

            _closeButton.onClick.AddListener(CloseCanvasClicked);
            _emptySpaceButton.onClick.AddListener(CloseCanvasClicked);

            bodyguardHire.Button.onClick.AddListener(BodyguardHireUpgradeClicked);
            GateUpgradeEvents.OnOpenHireCanvas += EnableCanvas;
            GateUpgradeEvents.OnCloseHireCanvas += DisableCanvas;
        }

        private void OnDisable()
        {
            if (_gate == null) return;

            _closeButton.onClick.RemoveListener(CloseCanvasClicked);
            _emptySpaceButton.onClick.RemoveListener(CloseCanvasClicked);

            bodyguardHire.Button.onClick.RemoveListener(BodyguardHireUpgradeClicked);
            GateUpgradeEvents.OnOpenHireCanvas -= EnableCanvas;
            GateUpgradeEvents.OnCloseHireCanvas -= DisableCanvas;
        }

        #region UPDATERS
        private void UpdateTexts()
        {
            bodyguardHire.LevelText.text = "";
            bodyguardHire.CostText.text = Gate.BodyguardHiredCost.ToString();

            CheckForMoneySufficiency();
        }
        private void CheckForMoneySufficiency()
        {
            bodyguardHire.Button.interactable = DataManager.TotalMoney >= Gate.BodyguardHiredCost && !Gate.BodyguardHired;
        }
        #endregion

        #region UPGRADE FUNCTIONS
        private void CloseCanvas()
        {
            GateUpgradeEvents.OnCloseHireCanvas?.Invoke();
            PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
        }
        private void UpgradeBodyguardHire()
        {
            GateUpgradeEvents.OnUpgradeBodyguardHire?.Invoke();
            GateUpgradeEvents.OnCloseHireCanvas?.Invoke();
            PlayerEvents.OnClosedUpgradeCanvas?.Invoke();

            _gate.BodyguardIsHired();
        }
        #endregion

        #region CLICK TRIGGER FUNCTIONS
        private void CloseCanvasClicked()
        {
            _closeButton.interactable = _emptySpaceButton.interactable = false;
            _closeButton.TriggerClick(CloseCanvas);
        }
        private void BodyguardHireUpgradeClicked() => bodyguardHire.Button.TriggerClick(UpgradeBodyguardHire);
        #endregion

        #region ANIMATOR FUNCTIONS
        private void EnableCanvas()
        {
            AudioHandler.PlayAudio(Enums.AudioType.UpgradeMenu);

            _closeButton.interactable = _emptySpaceButton.interactable = true;

            _animator.SetTrigger(_openID);
            IsOpen = true;

            CheckForMoneySufficiency();
        }
        private void DisableCanvas()
        {
            AudioHandler.PlayAudio(Enums.AudioType.UpgradeMenu);

            _closeButton.interactable = _emptySpaceButton.interactable = false;

            _animator.SetTrigger(_closeID);
            IsOpen = false;
        }
        #endregion
    }
}
