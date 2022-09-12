using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BodyguardStateManager : MonoBehaviour
    {
        private Bodyguard _bodyguard;
        private BodyguardBaseState _currentState;

        #region STATES
        public BodyguardWaitForCustomerState WaitForCustomerState = new BodyguardWaitForCustomerState();
        public BodyguardLetCustomerInsideState LetCustomerInsideState = new BodyguardLetCustomerInsideState();
        public BodyguardWasteTimeState WaitTimeState = new BodyguardWasteTimeState();
        #endregion

        public Bodyguard Bodyguard => _bodyguard;

        public void Init(Bodyguard bodyguard)
        {
            if (_bodyguard == null)
                _bodyguard = bodyguard;

            _currentState = WaitForCustomerState;
            _currentState.EnterState(this);
        }

        private void Update()
        {
            if (_bodyguard == null) return;
            _currentState.UpdateState(this);
        }

        #region PUBLICS
        public void SwitchState(BodyguardBaseState state)
        {
            _currentState = state;
            state.EnterState(this);
        }
        #endregion
    }
}
