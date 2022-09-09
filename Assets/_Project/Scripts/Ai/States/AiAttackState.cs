using UnityEngine;
using ZestGames;
using ZestCore.Ai;

namespace ClubBusiness
{
    public class AiAttackState : AiBaseState
    {
        private Ai _ai;
        private Ai _targetAi;
        private bool _reachedTarget, _startedArguing, _startedFighting, _isMoving;

        private readonly float _argueDuration, _fightDuration;
        private float _argueTimer, _fightTimer;

        public override void EnterState(AiStateManager aiStateManager)
        {
            aiStateManager.SwitchStateType(Enums.AiStateType.Attack);

            if (_ai == null)
                _ai = aiStateManager.Ai;

            _reachedTarget = _startedArguing = _startedFighting = _isMoving = false;
            _argueTimer = _argueDuration;
            _fightTimer = _fightDuration;
            if (CustomerManager.CustomersOnDanceFloor.Count > 1)
            {
                for (int i = 0; i < CustomerManager.CustomersOnDanceFloor.Count; i++)
                {
                    if (CustomerManager.CustomersOnDanceFloor[i] != _ai)
                        _targetAi = CustomerManager.CustomersOnDanceFloor[Random.Range(0, CustomerManager.CustomersOnDanceFloor.Count)];
                }
            }
            else
                Debug.Log("No other customer to fight!");
        }

        public override void UpdateState(AiStateManager aiStateManager)
        {
            if (!_reachedTarget)
            {
                if (Operation.IsTargetReached(_ai.transform, _targetAi.transform.position, 1f))
                {
                    _reachedTarget = true;
                    _isMoving = false;
                    _ai.OnIdle?.Invoke();

                    // start arguing
                    _startedArguing = true;
                    _ai.OnStartArguing?.Invoke();

                    _targetAi.StateManager.DefenseState.SetAttacker(_ai);
                    _targetAi.StateManager.SwitchState(_targetAi.StateManager.DefenseState);
                }
                else
                {
                    if (!_isMoving)
                    {
                        _isMoving = true;
                        _ai.OnMove?.Invoke();
                    }

                    Navigation.MoveTransform(_ai.transform, _targetAi.transform.position, _ai.RunSpeed);
                    Navigation.MoveTransform(_ai.transform, _targetAi.transform.position);
                }
            }
            else
            {
                Argue();
                Fight();
            }
        }

        private void Argue()
        {
            if (_startedArguing)
            {
                _argueTimer -= Time.deltaTime;
                if (_argueTimer <= 0f && !_startedFighting)
                {
                    _startedFighting = true;
                    _ai.OnStartFighting?.Invoke();

                    _targetAi.StateManager.DefenseState.StartFighting();
                }
            }
        }
        private void Fight()
        {
            if (_startedFighting)
            {
                _fightTimer -= Time.deltaTime;
                if (_fightTimer <= 0f)
                {
                    _ai.OnStopFighting?.Invoke();
                    _ai.StateManager.SwitchState(_ai.StateManager.WanderState);
                    _targetAi.StateManager.DefenseState.FinishFight();
                }
            }
        }
    }
}
