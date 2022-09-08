using ClubBusiness;
using UnityEngine;

namespace ZestGames
{
    public class AiStateManager : MonoBehaviour
    {
        private Ai _ai;
        private Enums.AiStateType _currentStateType;
        private AiBaseState _currentState;

        #region STATES
        public AiIdleState IdleState = new AiIdleState();
        public AiWanderState WanderState = new AiWanderState();
        public AiGetIntoClubState GetIntoClubState = new AiGetIntoClubState();
        public AiDanceState DanceState = new AiDanceState();
        public AiBuyDrinkState BuyDrinkState = new AiBuyDrinkState();
        public AiGoToToiletState GoToToiletState = new AiGoToToiletState();
        public AiGetIntoToiletQueueState GetIntoToiletQueueState = new AiGetIntoToiletQueueState();
        #endregion

        #region PROPERTIES
        public Ai Ai => _ai;
        public Enums.AiStateType CurrentStateType => _currentStateType;
        #endregion

        public void Init(Ai ai)
        {
            if (_ai == null)
                _ai = ai;

            if (_ai.CurrentLocation == Enums.AiLocation.Outside)
            {
                _currentState = GetIntoClubState;
                _currentState.EnterState(this);
            }
            else if (_ai.CurrentLocation == Enums.AiLocation.Inside)
            {
                _currentState = IdleState;
                _currentState.EnterState(this);
            }

            //_currentState = IdleState;
            //_currentState.EnterState(this);
        }

        private void Update()
        {
            if (_ai == null) return;
            _currentState.UpdateState(this);
        }

        #region PUBLICS
        public void SwitchState(AiBaseState state)
        {
            _currentState = state;
            state.EnterState(this);
        }
        public void SwitchStateType(Enums.AiStateType stateType)
        {
            _currentStateType = stateType;
        }
        #endregion
    }
}
