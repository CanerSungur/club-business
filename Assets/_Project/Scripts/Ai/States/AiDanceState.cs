using UnityEngine;
using ZestCore.Ai;
using ZestGames;

namespace ClubBusiness
{
    public class AiDanceState : AiBaseState
    {
        private Ai _ai;

        private Vector3 _dancingPosition;
        private bool _reachedToDancingPosition;

        private readonly float _danceDuration = 10f;
        private float _timer;

        public override void EnterState(AiStateManager aiStateManager)
        {
            aiStateManager.SwitchStateType(Enums.AiStateType.Dance);

            if (_ai == null)
                _ai = aiStateManager.Ai;

            _dancingPosition = AreaManager.GetRandomDanceAreaPosition();
            _reachedToDancingPosition = false;
            _timer = _danceDuration;
            _ai.OnMove?.Invoke();

            CustomerManager.AddCustomerOnDanceFloor(_ai);
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            if (!_reachedToDancingPosition)
            {
                if (Operation.IsTargetReached(_ai.transform, _dancingPosition, 0.02f))
                {
                    _reachedToDancingPosition = true;
                    if (!_ai.IsDancing)
                        _ai.OnStartDancing?.Invoke();
                }
                else
                {
                    Navigation.MoveTransform(_ai.transform, _dancingPosition, _ai.WalkSpeed);
                    Navigation.LookAtTarget(_ai.transform, _dancingPosition);
                }
            }
            else
            {
                // dancing right here

                _timer -= Time.deltaTime;
                if (_timer <= 0f && _ai.IsDancing)
                {
                    CustomerManager.RemoveCustomerFromDanceFloor(_ai);
                    _ai.OnStopDancing?.Invoke();
                    aiStateManager.SwitchState(aiStateManager.IdleState);
                }
            }
        }
    }
}
