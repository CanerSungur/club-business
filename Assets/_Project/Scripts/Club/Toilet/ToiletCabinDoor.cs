using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class ToiletCabinDoor : MonoBehaviour
    {
        private ToiletItem _toiletItem;
        private Animator _animator;

        #region ANIMATION VARIABLES
        private readonly int _openID = Animator.StringToHash("Open");
        private readonly int _brokenID = Animator.StringToHash("Broken");
        #endregion

        public void Init(ToiletItem toiletItem)
        {
            if (_toiletItem == null)
            {
                _toiletItem = toiletItem;
                _animator = GetComponent<Animator>();
            }

            _toiletItem.OnBreak += Broken;
            _toiletItem.OnFixCompleted += NotBroken;
            _toiletItem.OnCustomerExit += Open;
        }

        private void OnDisable()
        {
            if (_toiletItem == null) return;

            _toiletItem.OnBreak -= Broken;
            _toiletItem.OnFixCompleted -= NotBroken;
            _toiletItem.OnCustomerExit -= Open;
        }

        private void Open() => _animator.SetTrigger(_openID);
        private void Broken() => _animator.SetBool(_brokenID, true);
        private void NotBroken() => _animator.SetBool(_brokenID, false);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Ai ai) && ai.StateManager.GoToToiletState.CurrentToiletItem == _toiletItem && !_toiletItem.IsBroken)
            {
                Open();
            }
        }
    }
}
