using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class CleanerAnimationController : MonoBehaviour
    {
        private Cleaner _cleaner;
        private Animator _animator;

        #region ANIMATION VARIABLES
        private readonly int _waitID = Animator.StringToHash("Wait");
        private readonly int _walkID = Animator.StringToHash("Walk");
        private readonly int _wasteTimeID = Animator.StringToHash("WasteTime");
        private readonly int _startCleaningID = Animator.StringToHash("StartCleaning");
        private readonly int _stopCleaningID = Animator.StringToHash("StopCleaning");
        private readonly int _leanIndexID = Animator.StringToHash("LeanIndex");
        private readonly int _cleanSpeedID = Animator.StringToHash("CleanSpeed");
        #endregion

        #region PROPERTIES
        public Cleaner Cleaner => _cleaner;
        public Animator Animator => _animator;
        #endregion

        public void Init(Cleaner cleaner)
        {
            if (_cleaner == null)
            {
                _cleaner = cleaner;
                _animator = GetComponent<Animator>();
            }

            UpdateCleaningSpeed();

            _cleaner.OnWait += Wait;
            _cleaner.OnGoWaiting += Walk;
            _cleaner.OnGoCleaning += Walk;
            _cleaner.OnWasteTime += WasteTime;
            _cleaner.OnStartCleaning += StartCleaning;
            _cleaner.OnStopCleaning += StopCleaning;
            _cleaner.OnGetWarned += GetWarned;

            ToiletEvents.OnSetCurrentCleanerSpeed += UpdateCleaningSpeed;
        }

        private void OnDisable()
        {
            if (_cleaner == null) return;

            _cleaner.OnWait -= Wait;
            _cleaner.OnGoWaiting -= Walk;
            _cleaner.OnGoCleaning -= Walk;
            _cleaner.OnWasteTime -= WasteTime;
            _cleaner.OnStartCleaning -= StartCleaning;
            _cleaner.OnStopCleaning -= StopCleaning;
            _cleaner.OnGetWarned -= GetWarned;

            ToiletEvents.OnSetCurrentCleanerSpeed -= UpdateCleaningSpeed;
        }

        private void UpdateCleaningSpeed() => _animator.SetFloat(_cleanSpeedID, Toilet.CleanerSpeed);
        private void Wait()
        {
            SelectRandomLean();

            _animator.SetBool(_walkID, false);
            _animator.SetBool(_waitID, true);
            _animator.SetBool(_wasteTimeID, false);
        }
        private void Walk()
        {
            _animator.SetBool(_walkID, true);
            _animator.SetBool(_waitID, false);
            _animator.SetBool(_wasteTimeID, false);
        }
        private void SelectRandomLean() => _animator.SetInteger(_leanIndexID, Random.Range(1, 5));
        private void WasteTime() => _animator.SetBool(_wasteTimeID, true);
        private void StartCleaning() => _animator.SetTrigger(_startCleaningID);
        private void StopCleaning() => _animator.SetTrigger(_stopCleaningID);
        private void GetWarned() => _animator.SetBool(_wasteTimeID, false);

        #region ANIMATION EVENT FUNCTIONS
        public void FixHappened()
        {
            _cleaner.StateManager.CleanState.CurrentBrokenToilet.OnFix?.Invoke();
        }
        #endregion
    }
}
