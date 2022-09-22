using UnityEngine;
using ZestCore.Ai;
using ZestCore.Utility;
using ZestGames;

namespace ClubBusiness
{
    public class BouncerBreakFightState : BouncerBaseState
    {
        private Bouncer _bouncer;
        private Ai _currentAttacker = null;
        private bool _reachedToFight, _isMoving;

        public override void EnterState(BouncerStateManager bouncerStateManager)
        {
            if (_bouncer == null)
                _bouncer = bouncerStateManager.Bouncer;

            if (DanceFloor.CanBouncerBreakFight)
            {
                _currentAttacker = DanceFloor.AttackerAi;
                _reachedToFight = _isMoving = false;
            }
            else
            {
                Debug.Log("No fights to break.");
                _currentAttacker = null;
                bouncerStateManager.SwitchState(bouncerStateManager.GoWaitingState);
            }
        }

        public override void ExitState(BouncerStateManager bouncerStateManager)
        {

        }

        public override void UpdateState(BouncerStateManager bouncerStateManager)
        {
            if (!_reachedToFight)
            {
                if (Operation.IsTargetReached(_bouncer.transform, _currentAttacker.transform.position, 0.1f))
                {
                    Debug.Log("Fight is Broken!");
                    _reachedToFight = true;

                    _isMoving = false;
                    _bouncer.OnBreakFight?.Invoke();
                 
                    DanceFloor.AttackerAi.OnStopArguing?.Invoke();
                    DanceFloor.AttackerAi.OnStopFighting?.Invoke();
                    DanceFloor.AttackerAi.StateManager.SwitchState(DanceFloor.AttackerAi.StateManager.DanceState);

                    DanceFloor.DefenderAi.OnStopArguing?.Invoke();
                    DanceFloor.DefenderAi.StateManager.SwitchState(DanceFloor.DefenderAi.StateManager.DanceState);

                    ClubEvents.OnEveryoneGetHappier?.Invoke();
                    ClubEvents.OnAFightEnded?.Invoke();

                    Delayer.DoActionAfterDelay(_bouncer, 5f, () => {
                        if (DanceFloor.CanBouncerBreakFight && !_bouncer.IsWastingTime)
                            bouncerStateManager.SwitchState(bouncerStateManager.BreakFightState);
                        else
                            bouncerStateManager.SwitchState(bouncerStateManager.GoWaitingState);
                    });
                }
                else
                {
                    Navigation.MoveTransform(_bouncer.transform, _currentAttacker.transform.position, _bouncer.MovementSpeed);
                    Navigation.LookAtTarget(_bouncer.transform, _currentAttacker.transform.position);

                    if (!_isMoving)
                    {
                        _isMoving = true;
                        _bouncer.OnGoBreakingFight?.Invoke();
                    }
                }
            }
        }
    }
}
