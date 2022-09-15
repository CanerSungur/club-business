using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BodyguardWaitForCustomerState : BodyguardBaseState
    {
        private Bodyguard _bodyguard;
        private float _timer;
        private float _counter = 3f;

        public override void EnterState(BodyguardStateManager bodyguardStateManager)
        {
            Debug.Log("Waiting For Customer!");

            if (_bodyguard == null)
                _bodyguard = bodyguardStateManager.Bodyguard;

            _bodyguard.OnWaitForCustomer?.Invoke();

            _timer = _counter;
        }

        public override void ExitState(BodyguardStateManager bodyguardStateManager)
        {

        }

        public override void UpdateState(BodyguardStateManager bodyguardStateManager)
        {
            if (!_bodyguard.IsWastingTime)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f && QueueManager.GateQueue.QueueActivator.CanBodyguardTakeSomeoneIn)
                {
                    bodyguardStateManager.SwitchState(bodyguardStateManager.LetCustomerInsideState);
                    _timer = _counter;
                }
            }
        }
    }
}
