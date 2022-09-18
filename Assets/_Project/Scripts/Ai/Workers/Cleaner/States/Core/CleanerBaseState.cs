namespace ClubBusiness
{
    public abstract class CleanerBaseState
    {
        public abstract void EnterState(CleanerStateManager cleanerStateManager);
        public abstract void UpdateState(CleanerStateManager cleanerStateManager);
        public abstract void ExitState(CleanerStateManager cleanerStateManager);
    }
}
