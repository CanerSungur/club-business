using UnityEngine;
using ClubBusiness;

namespace ZestGames
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Player _player;
        private Animator _animator;
        private PlayerAnimationEventListener _animationEventListener;

        #region ANIMATION PARAMETERS
        private readonly int _moveID = Animator.StringToHash("Move");
        private readonly int _dieID = Animator.StringToHash("Die");
        private readonly int _winID = Animator.StringToHash("Win");
        private readonly int _loseID = Animator.StringToHash("Lose");
        private readonly int _cheerID = Animator.StringToHash("Cheer");
        private readonly int _cheerIndexID = Animator.StringToHash("CheerIndex");
        private readonly int _letInID = Animator.StringToHash("LetIn");
        private readonly int _bartendingID = Animator.StringToHash("Bartending");
        private readonly int _warnWorkerID = Animator.StringToHash("WarnWorker");
        #endregion

        public void Init(Player player)
        {
            if (_player == null)
            {
                _player = player;
                _animator = transform.GetChild(0).GetComponent<Animator>();
                _animationEventListener = GetComponentInChildren<PlayerAnimationEventListener>();
                _animationEventListener.Init(this);
            }

            PlayerEvents.OnMove += Move;
            PlayerEvents.OnIdle += Idle;
            PlayerEvents.OnDie += Die;
            PlayerEvents.OnWin += Win;
            PlayerEvents.OnLose += Lose;
            PlayerEvents.OnCheer += Cheer;
            PlayerEvents.OnStartLettingPeopleIn += StartLettingIn;
            PlayerEvents.OnStopLettingPeopleIn += StopLettingIn;
            PlayerEvents.OnStartFillingDrinks += StartBartending;
            PlayerEvents.OnStopFillingDrinks += StopBartending;
            PlayerEvents.OnWarnWorker += WarnWorker;
        }

        private void OnDisable()
        {
            if (_player == null) return;

            PlayerEvents.OnMove -= Move;
            PlayerEvents.OnIdle -= Idle;
            PlayerEvents.OnDie -= Die;
            PlayerEvents.OnWin -= Win;
            PlayerEvents.OnLose -= Lose;
            PlayerEvents.OnCheer -= Cheer;
            PlayerEvents.OnStartLettingPeopleIn -= StartLettingIn;
            PlayerEvents.OnStopLettingPeopleIn -= StopLettingIn;
            PlayerEvents.OnStartFillingDrinks -= StartBartending;
            PlayerEvents.OnStopFillingDrinks -= StopBartending;
            PlayerEvents.OnWarnWorker -= WarnWorker;
        }

        private void WarnWorker()
        {
            _animator.SetTrigger(_warnWorkerID);
        }
        private void StartBartending()
        {
            _animator.SetBool(_bartendingID, true);
        }
        private void StopBartending()
        {
            _animator.SetBool(_bartendingID, false);
        }
        private void StartLettingIn()
        {
            _animator.SetBool(_letInID, true);
        }
        private void StopLettingIn()
        {
            _animator.SetBool(_letInID, false);
        }

        #region BASIC ANIM FUNCTIONS
        private void Idle() => _animator.SetBool(_moveID, false);
        private void Move() => _animator.SetBool(_moveID, true);
        private void Die() => _animator.SetTrigger(_dieID);
        private void Win() => _animator.SetTrigger(_winID);
        private void Lose() => _animator.SetTrigger(_loseID);
        private void SelectRandomCheer() => _animator.SetInteger(_cheerIndexID, Random.Range(1, 5));
        private void Cheer()
        {
            SelectRandomCheer();
            _animator.SetTrigger(_cheerID);
        }
        #endregion
    }
}
