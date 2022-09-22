using UnityEngine;

namespace ClubBusiness
{
    public class BouncerWasteTimeState : BouncerBaseState
    {
        private Bouncer _bouncer;

        public override void EnterState(BouncerStateManager bouncerStateManager)
        {
            if (_bouncer == null)
                _bouncer = bouncerStateManager.Bouncer;

            _bouncer.OnWasteTime?.Invoke();
        }

        public override void ExitState(BouncerStateManager bouncerStateManager)
        {
            
        }

        public override void UpdateState(BouncerStateManager bouncerStateManager)
        {
            
        }
    }
}
