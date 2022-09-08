using UnityEngine;

namespace ZestGames
{
    [RequireComponent(typeof(Animator))]
    public class AiAnimationController : MonoBehaviour
    {
        private Ai _ai;
        private Animator _animator;

        #region ANIM PARAMETER SETUP
        private readonly int _moveID = Animator.StringToHash("Move");
        private readonly int _dieID = Animator.StringToHash("Die");
        private readonly int _winID = Animator.StringToHash("Win");
        private readonly int _loseID = Animator.StringToHash("Lose");
        private readonly int _startDancingID = Animator.StringToHash("StartDancing");
        private readonly int _stopDancingID = Animator.StringToHash("StopDancing");
        private readonly int _danceIndexID = Animator.StringToHash("DanceIndex");
        private readonly int _askForDrinkID = Animator.StringToHash("AskForDrink");
        private readonly int _askForDrinkIndexID = Animator.StringToHash("AskForDrinkIndex");
        private readonly int _drinkID = Animator.StringToHash("Drink");
        private readonly int _waitForToiletID = Animator.StringToHash("WaitForToilet");
        private readonly int _startPissingID = Animator.StringToHash("StartPissing");
        private readonly int _stopPissingID = Animator.StringToHash("StopPissing");
        #endregion

        public void Init(Ai ai)
        {
            if (_ai == null)
            {
                _ai = ai;
                _animator = GetComponent<Animator>();
            }

            _ai.OnIdle += Idle;
            _ai.OnMove += Move;
            _ai.OnDie += Die;
            _ai.OnWin += Win;
            _ai.OnLose += Lose;
            _ai.OnStartDancing += StartDancing;
            _ai.OnStopDancing += StopDancing;
            _ai.OnStartAskingForDrink += StartAskingForDrink;
            _ai.OnStopAskingForDrink += StopAskingForDrink;
            _ai.OnDrink += Drink;
            _ai.OnStartPissing += StartPissing;
            _ai.OnStopPissing += StopPissing;
            _ai.OnStartWaitingForToilet += StartWaitingForToilet;
            _ai.OnStopWaitingForToilet += StopWaitingForToilet;
        }

        private void OnDisable()
        {
            if (_ai == null) return;

            _ai.OnIdle -= Idle;
            _ai.OnMove -= Move;
            _ai.OnDie -= Die;
            _ai.OnWin -= Win;
            _ai.OnLose -= Lose;
            _ai.OnStartDancing -= StartDancing;
            _ai.OnStopDancing -= StopDancing;
            _ai.OnStartAskingForDrink -= StartAskingForDrink;
            _ai.OnStopAskingForDrink -= StopAskingForDrink;
            _ai.OnDrink -= Drink;
            _ai.OnStartPissing -= StartPissing;
            _ai.OnStopPissing -= StopPissing;
            _ai.OnStartWaitingForToilet -= StartWaitingForToilet;
            _ai.OnStopWaitingForToilet -= StopWaitingForToilet;
        }

        private void StartPissing() => _animator.SetTrigger(_startPissingID);
        private void StopPissing() => _animator.SetTrigger(_stopPissingID);
        private void StartWaitingForToilet() => _animator.SetBool(_waitForToiletID, true);
        private void StopWaitingForToilet() => _animator.SetBool(_waitForToiletID, false);
        private void Drink() => _animator.SetTrigger(_drinkID);
        private void StartAskingForDrink()
        {
            SelectRandomAskForDrink();
            _animator.SetBool(_askForDrinkID, true);
        }

        private void StopAskingForDrink() => _animator.SetBool(_askForDrinkID, false);
        private void SelectRandomAskForDrink() => _animator.SetInteger(_askForDrinkIndexID, Random.Range(0, 4));
        private void SelectRandomDance() => _animator.SetInteger(_danceIndexID, Random.Range(0, 5));
        private void StartDancing()
        {
            SelectRandomDance();
            _animator.SetTrigger(_startDancingID);
        }
        private void StopDancing() => _animator.SetTrigger(_stopDancingID);
        #region BASIC ANIM FUNCTIONS
        private void Idle() => _animator.SetBool(_moveID, false);
        private void Move() => _animator.SetBool(_moveID, true);
        private void Die() => _animator.SetTrigger(_dieID);
        private void Win() => _animator.SetTrigger(_winID);
        private void Lose() => _animator.SetTrigger(_loseID);
        #endregion

        #region ANIMATION EVENT LISTENER
        public void SelectRandomAskForDrinkAnim()
        {
            SelectRandomAskForDrink();
        }
        public void DrinkingFinished()
        {
            _ai.StateManager.BuyDrinkState.FinishDrinking();
        }
        #endregion
    }
}
