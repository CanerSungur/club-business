using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class CleanerHireCanvas : MonoBehaviour
    {
        private Toilet _toilet;

        private CustomButton _closeButton, _emptySpaceButton;
        public static bool IsOpen { get; private set; }

        #region ANIMATION
        private Animator _animator;
        private readonly int _openID = Animator.StringToHash("Open");
        private readonly int _closeID = Animator.StringToHash("Close");
        #endregion

        [Header("-- SETUP --")]
        [SerializeField] private UpgradeCanvasItem cleanerHire;

        public void Init(Toilet toilet)
        {
            if (_animator == null)
            {
                _toilet = toilet;
                _animator = GetComponent<Animator>();
                _closeButton = transform.GetChild(0).GetChild(0).GetComponent<CustomButton>();
                _emptySpaceButton = transform.GetChild(1).GetComponent<CustomButton>();

                cleanerHire.Init();
            }

            UpdateTexts();
            IsOpen = false;

            _closeButton.onClick.AddListener(CloseCanvasClicked);
            _emptySpaceButton.onClick.AddListener(CloseCanvasClicked);

            cleanerHire.Button.onClick.AddListener(CleanerHireUpgradeClicked);
            ToiletUpgradeEvents.OnOpenHireCanvas += EnableCanvas;
            ToiletUpgradeEvents.OnCloseHireCanvas += DisableCanvas;
        }

        private void OnDisable()
        {
            if (_toilet == null) return;

            _closeButton.onClick.RemoveListener(CloseCanvasClicked);
            _emptySpaceButton.onClick.RemoveListener(CloseCanvasClicked);

            cleanerHire.Button.onClick.RemoveListener(CleanerHireUpgradeClicked);
            ToiletUpgradeEvents.OnOpenHireCanvas -= EnableCanvas;
            ToiletUpgradeEvents.OnCloseHireCanvas -= DisableCanvas;
        }

        #region UPDATERS
        private void UpdateTexts()
        {
            cleanerHire.LevelText.text = "";
            cleanerHire.CostText.text = Toilet.CleanerHiredCost.ToString();

            CheckForMoneySufficiency();
        }
        private void CheckForMoneySufficiency()
        {
            cleanerHire.Button.interactable = DataManager.TotalMoney >= Toilet.CleanerHiredCost && !Toilet.CleanerHired;
        }
        #endregion

        #region UPGRADE FUNCTIONS
        private void CloseCanvas()
        {
            ToiletUpgradeEvents.OnCloseHireCanvas?.Invoke();
            PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
        }
        private void UpgradeCleanerHire()
        {
            ToiletUpgradeEvents.OnUpgradeCleanerHire?.Invoke();
            ToiletUpgradeEvents.OnCloseHireCanvas?.Invoke();
            PlayerEvents.OnClosedUpgradeCanvas?.Invoke();

            _toilet.CleanerIsHired();
        }
        #endregion

        #region CLICK TRIGGER FUNCTIONS
        private void CloseCanvasClicked()
        {
            _closeButton.interactable = _emptySpaceButton.interactable = false;
            _closeButton.TriggerClick(CloseCanvas);
        }
        private void CleanerHireUpgradeClicked() => cleanerHire.Button.TriggerClick(UpgradeCleanerHire);
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
