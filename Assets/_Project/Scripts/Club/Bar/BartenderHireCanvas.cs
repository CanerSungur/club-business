using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BartenderHireCanvas : MonoBehaviour
    {
        private Bar _bar;

        private CustomButton _closeButton, _emptySpaceButton;
        public static bool IsOpen { get; private set; }

        #region ANIMATION
        private Animator _animator;
        private readonly int _openID = Animator.StringToHash("Open");
        private readonly int _closeID = Animator.StringToHash("Close");
        #endregion

        [Header("-- SETUP --")]
        [SerializeField] private UpgradeCanvasItem bartenderHire;

        public void Init(Bar bar)
        {
            if (_animator == null)
            {
                _bar = bar;
                _animator = GetComponent<Animator>();
                _closeButton = transform.GetChild(0).GetChild(0).GetComponent<CustomButton>();
                _emptySpaceButton = transform.GetChild(1).GetComponent<CustomButton>();

                bartenderHire.Init();
            }

            UpdateTexts();
            IsOpen = false;

            _closeButton.onClick.AddListener(CloseCanvasClicked);
            _emptySpaceButton.onClick.AddListener(CloseCanvasClicked);

            bartenderHire.Button.onClick.AddListener(BartenderHireUpgradeClicked);
            BarUpgradeEvents.OnOpenHireCanvas += EnableCanvas;
            BarUpgradeEvents.OnCloseHireCanvas += DisableCanvas;
        }

        private void OnDisable()
        {
            if (_bar == null) return;

            _closeButton.onClick.RemoveListener(CloseCanvasClicked);
            _emptySpaceButton.onClick.RemoveListener(CloseCanvasClicked);

            bartenderHire.Button.onClick.RemoveListener(BartenderHireUpgradeClicked);
            BarUpgradeEvents.OnOpenHireCanvas -= EnableCanvas;
            BarUpgradeEvents.OnCloseHireCanvas -= DisableCanvas;
        }

        #region UPDATERS
        private void UpdateTexts()
        {
            bartenderHire.LevelText.text = "";
            bartenderHire.CostText.text = Bar.BartenderHiredCost.ToString();

            CheckForMoneySufficiency();
        }
        private void CheckForMoneySufficiency()
        {
            bartenderHire.Button.interactable = DataManager.TotalMoney >= Bar.BartenderHiredCost && !Bar.BartenderHired;
        }
        #endregion

        #region UPGRADE FUNCTIONS
        private void CloseCanvas()
        {
            BarUpgradeEvents.OnCloseHireCanvas?.Invoke();
            PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
        }
        private void UpgradeBartenderHire()
        {
            BarUpgradeEvents.OnUpgradeBartenderHire?.Invoke();
            BarUpgradeEvents.OnCloseHireCanvas?.Invoke();
            PlayerEvents.OnClosedUpgradeCanvas?.Invoke();

            _bar.BartenderIsHired();
        }
        #endregion

        #region CLICK TRIGGER FUNCTIONS
        private void CloseCanvasClicked()
        {
            _closeButton.interactable = _emptySpaceButton.interactable = false;
            _closeButton.TriggerClick(CloseCanvas);
        }
        private void BartenderHireUpgradeClicked() => bartenderHire.Button.TriggerClick(UpgradeBartenderHire);
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
