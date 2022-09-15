using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BodyguardWasteTimeState : BodyguardBaseState
    {
        private Bodyguard _bodyguard;

        public override void EnterState(BodyguardStateManager bodyguardtateManager)
        {
            Debug.Log("Wasting Time");

            if (_bodyguard == null)
                _bodyguard = bodyguardtateManager.Bodyguard;

            _bodyguard.OnWasteTime?.Invoke();
        }

        public override void ExitState(BodyguardStateManager bodyguardtateManager)
        {

        }

        public override void UpdateState(BodyguardStateManager bodyguardtateManager)
        {

        }
    }
}
