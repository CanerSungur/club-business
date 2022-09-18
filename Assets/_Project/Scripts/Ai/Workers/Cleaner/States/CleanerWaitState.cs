using UnityEngine;

namespace ClubBusiness
{
    public class CleanerWaitState : CleanerBaseState
    {
        private Cleaner _cleaner;
        private float _timer;
        private float _counter = 3f;

        public override void EnterState(CleanerStateManager cleanerStateManager)
        {
            Debug.Log("Wait State");

            if (_cleaner == null)
                _cleaner = cleanerStateManager.Cleaner;

            _cleaner.OnWait?.Invoke();
            _timer = _counter;
        }

        public override void ExitState(CleanerStateManager cleanerStateManager)
        {
            
        }

        public override void UpdateState(CleanerStateManager cleanerStateManager)
        {
            if (!_cleaner.IsWastingTime)
            {
                Debug.Log(Toilet.CanCleanerFixToilet);

                _timer -= Time.deltaTime;
                if (_timer <= 0f && Toilet.CanCleanerFixToilet)
                {
                    Debug.Log("ready to fix");
                    cleanerStateManager.SwitchState(cleanerStateManager.CleanState);
                    _timer = _counter;
                }
            }
        }
    }
}
