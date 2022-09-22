using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BouncerStateManager : MonoBehaviour
    {
        private Bouncer _bouncer;
        private BouncerBaseState _currentState;

        #region STATES
        public BouncerWaitForFightState WaitForFightState = new BouncerWaitForFightState();
        public BouncerGoWaitingState GoWaitingState = new BouncerGoWaitingState();
        public BouncerBreakFightState BreakFightState = new BouncerBreakFightState();
        public BouncerWasteTimeState WasteTimeState = new BouncerWasteTimeState();
        #endregion

        public Bouncer Bouncer => _bouncer;

        public void Init(Bouncer bouncer)
        {
            if (_bouncer == null)
                _bouncer = bouncer;

            _currentState = WaitForFightState;
            _currentState.EnterState(this);
        }

        private void Update()
        {
            if (_currentState == null || _bouncer == null || GameManager.GameState != Enums.GameState.Started) return;
            _currentState.UpdateState(this);
        }

        #region PUBLICS
        public void SwitchState(BouncerBaseState state)
        {
            _currentState = state;
            state.EnterState(this);
        }
        #endregion
    }
}
