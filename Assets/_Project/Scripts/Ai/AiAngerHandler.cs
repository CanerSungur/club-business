using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class AiAngerHandler : MonoBehaviour
    {
        private Ai _ai;
        private Enums.AiMood _currentMood;
        public Enums.AiMood CurrentMood => _currentMood;

        private readonly float _happinessLimit = 100f;
        private readonly float _angerLimit = -100f;
        private readonly float _angerIncrement = 25f;
        private readonly float _happinessIncrement = 10f;
        private float _currentMoodRate;

        public void Init(Ai ai)
        {
            if (_ai == null)
                _ai = ai;

            _currentMoodRate = 0;
            UpdateMood();
        }

        private void UpdateMood()
        {
            if (_currentMoodRate <= _happinessLimit && _currentMoodRate > 60)
                _currentMood = Enums.AiMood.VeryHappy;
            else if (_currentMoodRate <= 60 && _currentMoodRate > 20)
                _currentMood = Enums.AiMood.Happy;
            else if (_currentMoodRate <= 20 && _currentMoodRate > -10f)
                _currentMood = Enums.AiMood.Neutral;
            else if (_currentMoodRate <= -10f && _currentMoodRate > -30f)
                _currentMood = Enums.AiMood.Frustrated;
            else if (_currentMoodRate <= -30f && _currentMoodRate > -70f)
                _currentMood = Enums.AiMood.Angry;
            else
                _currentMood = Enums.AiMood.Furious;
        }

        #region PUBLICS
        public void GetAngrier()
        {
            _currentMoodRate -= _angerIncrement;

            Enums.AiMood previousMood = _currentMood;
            UpdateMood();
            if (_ai != null && previousMood != _currentMood)
                _ai.OnMoodChange?.Invoke(_currentMood);


            if (_currentMoodRate <= _angerLimit)
            {
                // Leave the club
                _ai.StateManager.SwitchState(_ai.StateManager.LeaveClubState);
                Debug.Log("LEAVE CLUB!");
            }
        }
        public void GetHappier()
        {
            _currentMoodRate += _happinessIncrement;

            Enums.AiMood previousMood = _currentMood;
            UpdateMood();
            if (_ai != null && previousMood != _currentMood)
                _ai.OnMoodChange?.Invoke(_currentMood);

            if (_currentMoodRate >= _happinessLimit)
                _currentMoodRate = _happinessLimit;
        }
        #endregion
    }
}
