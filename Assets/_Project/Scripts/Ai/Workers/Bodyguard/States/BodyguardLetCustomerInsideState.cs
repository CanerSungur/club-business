using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BodyguardLetCustomerInsideState : BodyguardBaseState
    {
        private Bodyguard _bodyguard;

        public override void EnterState(BodyguardStateManager bodyguardStateManager)
        {
            Debug.Log("Lettin Customer In");
            if (_bodyguard == null)
                _bodyguard = bodyguardStateManager.Bodyguard;

            _bodyguard.OnLetIn?.Invoke();
        }

        public override void ExitState(BodyguardStateManager bodyguardStateManager)
        {
            
        }

        public override void UpdateState(BodyguardStateManager bodyguardStateManager)
        {
            
        }
    }
}
