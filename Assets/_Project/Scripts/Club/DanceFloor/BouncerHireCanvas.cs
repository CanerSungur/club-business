using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BouncerHireCanvas : MonoBehaviour
    {
        private DanceFloor _danceFloor;

        private CustomButton _closeButton, _emptySpaceButton;
        public static bool IsOpen { get; private set; }

        #region ANIMATION
        private Animator _animator;
        private readonly int _openID = Animator.StringToHash("Open");
        private readonly int _closeID = Animator.StringToHash("Close");
        #endregion

        [Header("-- SETUP --")]
        [SerializeField] private UpgradeCanvasItem bouncerHire;

        public void Init(DanceFloor danceFloor)
        {
            if (_animator == null)
            {
                _danceFloor = danceFloor;
                _animator = GetComponent<Animator>();
                _closeButton = transform.GetChild(0).GetChild(0).GetComponent<CustomButton>();
                _emptySpaceButton = transform.GetChild(1).GetComponent<CustomButton>();

                bouncerHire.Init();
            }

            UpdateTexts();
            IsOpen = false;

            _closeButton.onClick.AddListener(CloseCanvasClicked);
            _emptySpaceButton.onClick.AddListener(CloseCanvasClicked);

            bouncerHire.Button.onClick.AddListener(BouncerHireUpgradeClicked);
            DanceFloorUpgradeEvents.OnOpenHireCanvas += EnableCanvas;
            DanceFloorUpgradeEvents.OnCloseHireCanvas += DisableCanvas;
        }

        private void OnDisable()
        {
            if (_danceFloor == null) return;

            _closeButton.onClick.RemoveListener(CloseCanvasClicked);
            _emptySpaceButton.onClick.RemoveListener(CloseCanvasClicked);

            bouncerHire.Button.onClick.RemoveListener(BouncerHireUpgradeClicked);
            DanceFloorUpgradeEvents.OnOpenHireCanvas -= EnableCanvas;
            DanceFloorUpgradeEvents.OnCloseHireCanvas -= DisableCanvas;
        }

        #region UPDATERS
        private void UpdateTexts()
        {
            bouncerHire.LevelText.text = "";
            bouncerHire.CostText.text = DanceFloor.BouncerHiredCost.ToString();

            CheckForMoneySufficiency();
        }
        private void CheckForMoneySufficiency()
        {
            bouncerHire.Button.interactable = DataManager.TotalMoney >= DanceFloor.BouncerHiredCost && !DanceFloor.BouncerHired;
        }
        #endregion

        #region UPGRADE FUNCTIONS
        private void CloseCanvas()
        {
            DanceFloorUpgradeEvents.OnCloseHireCanvas?.Invoke();
            PlayerEvents.OnClosedUpgradeCanvas?.Invoke();
        }
        private void UpgradeBouncerHire()
        {
            DanceFloorUpgradeEvents.OnUpgradeBouncerHire?.Invoke();
            DanceFloorUpgradeEvents.OnCloseHireCanvas?.Invoke();
            PlayerEvents.OnClosedUpgradeCanvas?.Invoke();

            _danceFloor.BouncerIsHired();
        }
        #endregion

        #region CLICK TRIGGER FUNCTIONS
        private void CloseCanvasClicked()
        {
            _closeButton.interactable = _emptySpaceButton.interactable = false;
            _closeButton.TriggerClick(CloseCanvas);
        }
        private void BouncerHireUpgradeClicked() => bouncerHire.Button.TriggerClick(UpgradeBouncerHire);
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
