using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BodyguardLetCustomerInsideState : BodyguardBaseState
    {
        private Bodyguard _bodyguard;
        private float _timer;
        private bool _canLetIn;

        public override void EnterState(BodyguardStateManager bodyguardStateManager)
        {
            //Debug.Log("Lettin Customer In");
            if (_bodyguard == null)
                _bodyguard = bodyguardStateManager.Bodyguard;

            _timer = Gate.BodyguardLetInDuration;
            _canLetIn = false;
        }

        public override void ExitState(BodyguardStateManager bodyguardStateManager)
        {
            
        }

        public override void UpdateState(BodyguardStateManager bodyguardStateManager)
        {
            if (!_canLetIn && QueueManager.GateQueue.QueueActivator.CanBodyguardTakeSomeoneIn)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    _timer = Gate.BodyguardLetInDuration;
                    _canLetIn = true;
                    LetCustomerIn();
                }
            }
        }

        private void LetCustomerIn()
        {
            _bodyguard.OnLetIn?.Invoke();
            PlayerEvents.OnEmptyNextInGateQueue?.Invoke();
            if (_bodyguard.IsWastingTime)
                _bodyguard.StateManager.SwitchState(_bodyguard.StateManager.WasteTimeState);
            else
                _bodyguard.StateManager.SwitchState(_bodyguard.StateManager.WaitForCustomerState);
        }
    }
}
