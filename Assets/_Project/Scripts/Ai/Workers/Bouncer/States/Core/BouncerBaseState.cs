namespace ClubBusiness
{
    public abstract class BouncerBaseState
    {
        public abstract void EnterState(BouncerStateManager bouncerStateManager);
        public abstract void UpdateState(BouncerStateManager bouncerStateManager);
        public abstract void ExitState(BouncerStateManager bouncerStateManager);

    }
}
