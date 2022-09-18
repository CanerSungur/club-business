using UnityEngine;

namespace ClubBusiness
{
    public class CleanerWasteTimeState : CleanerBaseState
    {
        private Cleaner _cleaner;

        public override void EnterState(CleanerStateManager cleanerStateManager)
        {
            Debug.Log("Wasting Time...");

            if (_cleaner == null)
                _cleaner = cleanerStateManager.Cleaner;

            _cleaner.OnWasteTime?.Invoke();
        }

        public override void ExitState(CleanerStateManager cleanerStateManager)
        {
            
        }

        public override void UpdateState(CleanerStateManager cleanerStateManager)
        {
            
        }
    }
}
