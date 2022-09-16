using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class BartenderPourDrinkState : BartenderBaseState
    {
        private Bartender _bartender;
        private float _timer, _finishPouringTimer;
        private bool _canPourDrink;

        public override void EnterState(BartenderStateManager bartenderStateManager)
        {
            if (_bartender == null)
                _bartender = bartenderStateManager.Bartender;

            _timer = _finishPouringTimer = 2f;
            _canPourDrink = false;
        }

        public override void ExitState(BartenderStateManager bartenderStateManager)
        {
            
        }

        public override void UpdateState(BartenderStateManager bartenderStateManager)
        {
            if (!_bartender.IsPouringDrink && QueueManager.BarQueue.QueueActivator.CanBartenderGiveDrink)
            {
                _bartender.IsPouringDrink = true;
                _bartender.OnStartPouringDrink?.Invoke();
            }
            
            if (!QueueManager.BarQueue.QueueActivator.CanBartenderGiveDrink && _bartender.IsPouringDrink)
            {
                _bartender.IsPouringDrink = false;
                _bartender.OnStopPouringDrink?.Invoke();
                bartenderStateManager.SwitchState(bartenderStateManager.WaitForCustomerState);
            }

            if (!_canPourDrink && QueueManager.BarQueue.QueueActivator.CanBartenderGiveDrink)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    _timer = _finishPouringTimer;
                    _canPourDrink = true;
                    PourDrink();
                }
            }
        }

        private void PourDrink()
        {
            _bartender.OnFinishDrink?.Invoke();
            Beer beer = ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.Beer, Bar.BeerSpawnTransform.position, Quaternion.identity).GetComponent<Beer>();
            beer.Init(QueueManager.BarQueue.AisInQueue[0]);

            if (_bartender.IsWastingTime)
                _bartender.StateManager.SwitchState(_bartender.StateManager.WasteTimeState);
            else
                _bartender.StateManager.SwitchState(_bartender.StateManager.WaitForCustomerState);
        }
    }
}
