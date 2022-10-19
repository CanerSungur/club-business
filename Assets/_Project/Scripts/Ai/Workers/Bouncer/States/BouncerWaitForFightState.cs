using UnityEngine;

namespace ClubBusiness
{
    public class BouncerWaitForFightState : BouncerBaseState
    {
        private Bouncer _bouncer;
        private float _timer;
        private float _counter = 3f;

        public override void EnterState(BouncerStateManager bouncerStateManager)
        {
            //Debug.Log("Wait State");

            if (_bouncer == null)
                _bouncer = bouncerStateManager.Bouncer;

            _bouncer.OnWaitForFight?.Invoke();
            _timer = _counter;
        }

        public override void ExitState(BouncerStateManager bouncerStateManager)
        {
            
        }

        public override void UpdateState(BouncerStateManager bouncerStateManager)
        {
            if (!_bouncer.IsWastingTime)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f && DanceFloor.CanBouncerBreakFight)
                {
                    bouncerStateManager.SwitchState(bouncerStateManager.BreakFightState);
                    _timer = _counter;
                }
            }
        }
    }
}
