using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BartenderWaitForCustomerState : BartenderBaseState
    {
        private Bartender _bartender;

        public override void EnterState(BartenderStateManager bartenderStateManager)
        {
            //Debug.Log("Waiting For Customer!");

            if (_bartender == null)
                _bartender = bartenderStateManager.Bartender;

            _bartender.OnWaitForCustomer?.Invoke();
        }

        public override void ExitState(BartenderStateManager bartenderStateManager)
        {
            
        }

        public override void UpdateState(BartenderStateManager bartenderStateManager)
        {
            if (!_bartender.IsWastingTime && QueueManager.BarQueue.QueueActivator.CanBartenderGiveDrink)
                bartenderStateManager.SwitchState(bartenderStateManager.PourDrinkState);
        }
    }
}
