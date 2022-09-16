using UnityEngine;

namespace ClubBusiness
{
    public class BartenderAnimationController : MonoBehaviour
    {
        private Bartender _bartender;
        private Animator _animator;

        #region ANIMATION VARIABLES
        private readonly int _pourDrinkID = Animator.StringToHash("PourDrink");
        private readonly int _wasteTimeID = Animator.StringToHash("WasteTime");
        #endregion

        public void Init(Bartender bartender)
        {
            if (_bartender == null)
            {
                _bartender = bartender;
                _animator = GetComponent<Animator>();
            }

            _bartender.OnWaitForCustomer += WaitForCustomers;
            _bartender.OnStartPouringDrink += StartPouringDrink;
            _bartender.OnStopPouringDrink += StopPouringDrink;
            _bartender.OnWasteTime += WasteTime;
        }

        private void OnDisable()
        {
            if (_bartender == null) return;

            _bartender.OnWaitForCustomer -= WaitForCustomers;
            _bartender.OnStartPouringDrink -= StartPouringDrink;
            _bartender.OnStopPouringDrink -= StopPouringDrink;
            _bartender.OnWasteTime -= WasteTime;
        }

        #region EVENT HANDLER FUNCTIONS
        private void WaitForCustomers() => _animator.SetBool(_wasteTimeID, false);
        private void WasteTime() => _animator.SetBool(_wasteTimeID, true);
        private void StartPouringDrink() => _animator.SetBool(_pourDrinkID, true);
        private void StopPouringDrink() => _animator.SetBool(_pourDrinkID, false);
        #endregion
    }
}
