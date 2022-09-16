using UnityEngine;

namespace ClubBusiness
{
    public class BartenderWasteTimeState : BartenderBaseState
    {
        private Bartender _bartender;

        public override void EnterState(BartenderStateManager bartenderStateManager)
        {
            Debug.Log("Wasting Time");

            if (_bartender == null)
                _bartender = bartenderStateManager.Bartender;

            _bartender.OnStopPouringDrink?.Invoke();
            _bartender.IsPouringDrink = false;
            _bartender.OnWasteTime?.Invoke();
        }

        public override void ExitState(BartenderStateManager bartenderStateManager)
        {
            
        }

        public override void UpdateState(BartenderStateManager bartenderStateManager)
        {
            
        }
    }
}
