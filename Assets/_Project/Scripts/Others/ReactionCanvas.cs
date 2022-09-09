using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class ReactionCanvas : MonoBehaviour
    {
        private Ai _ai;
        private Animator _animator;

        #region ANIMATION PARAMETERS
        private readonly int _veryHappyID = Animator.StringToHash("VeryHappy");
        private readonly int _happyID = Animator.StringToHash("Happy");
        private readonly int _frustratedID = Animator.StringToHash("Frustrated");
        private readonly int _angryID = Animator.StringToHash("Angry");
        private readonly int _furiousID = Animator.StringToHash("Furious");
        #endregion

        [Header("-- NEED SETUP --")]
        [SerializeField] private GameObject needCanvas;
        [SerializeField] private GameObject needDrink;
        [SerializeField] private GameObject needToPiss;
        [SerializeField] private GameObject needDancing;

        [Header("-- ACTION SETUP --")]
        [SerializeField] private GameObject actionCanvas;
        [SerializeField] private GameObject drinking;
        [SerializeField] private GameObject dancing;
        [SerializeField] private GameObject pissing;
        [SerializeField] private GameObject arguing;

        [Header("-- MOOD SETUP --")]
        [SerializeField] private GameObject moodCanvas;
        [SerializeField] private GameObject veryHappy;
        [SerializeField] private GameObject happy;
        [SerializeField] private GameObject frustrated;
        [SerializeField] private GameObject angry;
        [SerializeField] private GameObject furious;

        public void Init(Ai ai)
        {
            if (_ai == null)
            {
                _ai = ai;
                _animator = GetComponent<Animator>();
            }

            _ai.OnMoodChange += HandleMoodChange;
        }

        private void OnDisable()
        {
            if (_ai == null) return;
            _ai.OnMoodChange -= HandleMoodChange;
        }

        private void HandleMoodChange(Enums.AiMood mood)
        {
            switch (mood)
            {
                case Enums.AiMood.VeryHappy:
                    EnableVeryHappy();
                    break;
                case Enums.AiMood.Happy:
                    EnableHappy();
                    break;
                case Enums.AiMood.Neutral:
                    break;
                case Enums.AiMood.Frustrated:
                    EnableFrustrated();
                    break;
                case Enums.AiMood.Angry:
                    EnableAngry();
                    break;
                case Enums.AiMood.Furious:
                    EnableFurious();
                    break;
                default:
                    break;
            }
        }

        private void EnableItem()
        {

        }

        #region NEED PUBLICS
        public void EnableNeedDrink()
        {

        }
        public void EnableNeedToPiss()
        {

        }
        public void EnableNeedDancing()
        {

        }
        #endregion

        #region ACTION PUBLICS
        public void EnableDrinking()
        {
            //moodCanvas.SetActive(false);
            //actionCanvas.SetActive(true);
            
            //drinking.SetActive(true);

            //dancing.SetActive(false);
            //pissing.SetActive(false);
            //arguing.SetActive(false);
        }
        public void EnableDancing()
        {
            //moodCanvas.SetActive(false);
            //actionCanvas.SetActive(true);
            
            //dancing.SetActive(true);

            //drinking.SetActive(false);
            //pissing.SetActive(false);
            //arguing.SetActive(false);
        }
        public void EnablePissing()
        {
            //moodCanvas.SetActive(false);
            //actionCanvas.SetActive(true);
            
            //pissing.SetActive(true);

            //dancing.SetActive(false);
            //drinking.SetActive(false);
            //arguing.SetActive(false);
        }
        public void EnableArguing()
        {
            //moodCanvas.SetActive(false);
            //actionCanvas.SetActive(true);
            
            //arguing.SetActive(true);

            //dancing.SetActive(false);
            //pissing.SetActive(false);
            //drinking.SetActive(false);
        }
        #endregion

        #region MOOD PUBLICS
        private void EnableVeryHappy()
        {
            _animator.SetTrigger(_veryHappyID);
        }
        private void EnableHappy()
        {
            _animator.SetTrigger(_happyID);
        }
        public void EnableFrustrated()
        {
            _animator.SetTrigger(_frustratedID);
        }
        public void EnableAngry()
        {
            _animator.SetTrigger(_angryID);
        }
        public void EnableFurious()
        {
            _animator.SetTrigger(_furiousID);
        }
        #endregion
    }
}
