using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BodyguardWaitForCustomerState : BodyguardBaseState
    {
        private Bodyguard _bodyguard;
        private float _currentStamina;

        public override void EnterState(BodyguardStateManager bodyguardStateManager)
        {
            Debug.Log("Waiting For Customer!");

            if (_bodyguard == null)
                _bodyguard = bodyguardStateManager.Bodyguard;

            if (_bodyguard.IsWastingTime)
            {
                _bodyguard.OnWaitForCustomer?.Invoke();
                _currentStamina = Gate.BodyguardStamina;
            }
        }

        public override void ExitState(BodyguardStateManager bodyguardStateManager)
        {
            
        }

        public override void UpdateState(BodyguardStateManager bodyguardStateManager)
        {
            if (!_bodyguard.IsWastingTime)
            {
                _currentStamina -= Time.deltaTime;

                if (_currentStamina <= 0f)
                {
                    _bodyguard.OnWasteTime?.Invoke();
                }
            }
        }
    }
}
