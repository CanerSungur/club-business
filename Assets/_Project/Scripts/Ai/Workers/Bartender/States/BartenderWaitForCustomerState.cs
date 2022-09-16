using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BartenderWaitForCustomerState : BartenderBaseState
    {
        private Bartender _bartender;
        private float _timer;
        private float _counter = 3f;

        public override void EnterState(BartenderStateManager bartenderStateManager)
        {
            Debug.Log("Waiting For Customer!");

            if (_bartender == null)
                _bartender = bartenderStateManager.Bartender;

            _bartender.OnWaitForCustomer?.Invoke();

            _timer = _counter;
        }

        public override void ExitState(BartenderStateManager bartenderStateManager)
        {
            
        }

        public override void UpdateState(BartenderStateManager bartenderStateManager)
        {
            if (!_bartender.IsWastingTime)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f && QueueManager.BarQueue.QueueActivator.CanBartenderGiveDrink)
                {
                    bartenderStateManager.SwitchState(bartenderStateManager.PourDrinkState);
                    _timer = _counter;
                }
            }
        }
    }
}
