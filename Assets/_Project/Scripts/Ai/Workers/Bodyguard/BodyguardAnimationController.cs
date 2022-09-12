using UnityEngine;

namespace ClubBusiness
{
    public class BodyguardAnimationController : MonoBehaviour
    {
        private Bodyguard _bodyguard;
        private Animator _animator;

        #region ANIMATION VARIABLES
        private readonly int _letInID = Animator.StringToHash("LetIn");
        private readonly int _wasteTimeID = Animator.StringToHash("WasteTime");
        #endregion

        public void Init(Bodyguard bodyguard)
        {
            if (_bodyguard == null)
            {
                _bodyguard = bodyguard;
                _animator = GetComponent<Animator>();
            }

            _bodyguard.OnWaitForCustomer += WaitForCustomers;
            _bodyguard.OnLetIn += LetCustomerIn;
            _bodyguard.OnWasteTime += WasteTime;
        }

        private void OnDisable()
        {
            if (_bodyguard == null) return;

            _bodyguard.OnWaitForCustomer -= WaitForCustomers;
            _bodyguard.OnLetIn -= LetCustomerIn;
            _bodyguard.OnWasteTime -= WasteTime;
        }

        #region EVENT HANDLER FUNCTIONS
        private void WaitForCustomers() => _animator.SetBool(_wasteTimeID, false);
        private void WasteTime() => _animator.SetBool(_wasteTimeID, true);
        private void LetCustomerIn() => _animator.SetTrigger(_letInID);
        #endregion
    }
}
