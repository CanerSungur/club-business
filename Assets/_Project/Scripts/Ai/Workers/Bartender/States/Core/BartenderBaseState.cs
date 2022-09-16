namespace ClubBusiness
{
    public abstract class BartenderBaseState
    {
        public abstract void EnterState(BartenderStateManager bartenderStateManager);
        public abstract void UpdateState(BartenderStateManager bartenderStateManager);
        public abstract void ExitState(BartenderStateManager bartenderStateManager);
    }
}
