using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BartenderStateManager : MonoBehaviour
    {
        private Bartender _bartender;
        private BartenderBaseState _currentState;

        #region STATES
        public BartenderWaitForCustomerState WaitForCustomerState = new BartenderWaitForCustomerState();
        public BartenderPourDrinkState PourDrinkState = new BartenderPourDrinkState();
        public BartenderWasteTimeState WasteTimeState = new BartenderWasteTimeState();
        #endregion

        public Bartender Bartender => _bartender;

        public void Init(Bartender bartender)
        {
            if (_bartender == null)
                _bartender = bartender;

            _currentState = WaitForCustomerState;
            _currentState.EnterState(this);
        }

        private void Update()
        {
            if (_currentState == null || _bartender == null || GameManager.GameState != Enums.GameState.Started) return;
            _currentState.UpdateState(this);
        }

        #region PUBLICS
        public void SwitchState(BartenderBaseState state)
        {
            _currentState = state;
            state.EnterState(this);
        }
        #endregion
    }
}
