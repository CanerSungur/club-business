
namespace ClubBusiness
{
    public abstract class BodyguardBaseState
    {
        public abstract void EnterState(BodyguardStateManager bodyguardStateManager);
        public abstract void UpdateState(BodyguardStateManager bodyguardStateManager);
        public abstract void ExitState(BodyguardStateManager bodyguardStateManager);
    }
}
