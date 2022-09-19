using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class CleanerStateManager : MonoBehaviour
    {
        private Cleaner _cleaner;
        private CleanerBaseState _currentState;

        #region STATES
        public CleanerWaitState WaitState = new CleanerWaitState();
        public CleanerGoWaitingState GoWaitingState = new CleanerGoWaitingState();
        public CleanerCleanState CleanState = new CleanerCleanState();
        public CleanerWasteTimeState WasteTimeState = new CleanerWasteTimeState();
        public CleanerEnterToiletState EnterToiletState = new CleanerEnterToiletState();
        public CleanerExitToiletState ExitToiletState = new CleanerExitToiletState();
        #endregion

        public Cleaner Cleaner => _cleaner;

        public void Init(Cleaner cleaner)
        {
            if (_cleaner == null)
                _cleaner = cleaner;

            _currentState = WaitState;
            _currentState.EnterState(this);
        }

        private void Update()
        {
            if (_currentState == null || _cleaner == null || GameManager.GameState != Enums.GameState.Started) return;
            _currentState.UpdateState(this);
        }

        #region PUBLICS
        public void SwitchState(CleanerBaseState state)
        {
            _currentState = state;
            state.EnterState(this);
        }
        #endregion
    }
}
