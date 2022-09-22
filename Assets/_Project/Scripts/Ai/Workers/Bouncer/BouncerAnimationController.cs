using System;
using UnityEngine;

namespace ClubBusiness
{
    public class BouncerAnimationController : MonoBehaviour
    {
        private Bouncer _bouncer;
        private Animator _animator;

        #region ANIMATION VARIABLES
        private readonly int _waitID = Animator.StringToHash("Wait");
        private readonly int _walkID = Animator.StringToHash("Walk");
        private readonly int _wasteTimeID = Animator.StringToHash("WasteTime");
        private readonly int _breakFightID = Animator.StringToHash("BreakFight");
        #endregion

        public void Init(Bouncer bouncer)
        {
            if (_bouncer == null)
            {
                _bouncer = bouncer;
                _animator = GetComponent<Animator>();

                _bouncer.OnWaitForFight += Wait;
                _bouncer.OnGoWaitingForFight += Walk;
                _bouncer.OnGoBreakingFight += Walk;
                _bouncer.OnWasteTime += WasteTime;
                _bouncer.OnBreakFight += BreakFight;
                _bouncer.OnGetWarned += GetWarned;
            }
        }

        private void OnDisable()
        {
            if (_bouncer == null) return;

            _bouncer.OnWaitForFight -= Wait;
            _bouncer.OnGoWaitingForFight -= Walk;
            _bouncer.OnGoBreakingFight -= Walk;
            _bouncer.OnWasteTime -= WasteTime;
            _bouncer.OnBreakFight -= BreakFight;
            _bouncer.OnGetWarned -= GetWarned;
        }

        private void Wait()
        {
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
        private void WasteTime() => _animator.SetBool(_wasteTimeID, true);
        private void BreakFight()
        {
            _animator.SetBool(_walkID, false);
            _animator.SetBool(_waitID, true);
            _animator.SetTrigger(_breakFightID);
        }

        private void GetWarned() => _animator.SetBool(_wasteTimeID, false);
    }
}
