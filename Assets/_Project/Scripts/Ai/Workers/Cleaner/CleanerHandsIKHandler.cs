using UnityEngine;

namespace ClubBusiness
{
    public class CleanerHandsIKHandler : MonoBehaviour
    {
        private CleanerAnimationController _animationController;

        [Header("-- RIGHT HAND IK SETUP --")]
        [SerializeField] private Transform _rightHandObject = null;
        [SerializeField] private Transform rightHandHint;
        [SerializeField, Range(0f, 1f)] private float rightHandWeight;

        [Header("-- LEFT HAND IK SETUP --")]
        [SerializeField] private Transform _leftHandObject;
        [SerializeField] private Transform leftHandHint;
        [SerializeField, Range(0f, 1f)] private float leftHandWeight;

        public void Init(CleanerAnimationController animationController)
        {
            if (_animationController == null)
                _animationController = animationController;

            StopIK();

            _animationController.Cleaner.OnStartCleaning += StartIK;
            _animationController.Cleaner.OnStopCleaning += StopIK;
        }

        private void OnDisable()
        {
            if (_animationController == null) return;

            _animationController.Cleaner.OnStartCleaning -= StartIK;
            _animationController.Cleaner.OnStopCleaning -= StopIK;
        }

        private void StartIK() => rightHandWeight = 1f;
        private void StopIK() => rightHandWeight = 0f;

        private void OnAnimatorIK()
        {
            if (_animationController.Animator && _rightHandObject && _leftHandObject)
            {
                #region RIGHT HAND IK
                _animationController.Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
                _animationController.Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
                _animationController.Animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandObject.position);
                _animationController.Animator.SetIKRotation(AvatarIKGoal.RightHand, _rightHandObject.rotation);

                if (rightHandHint)
                {
                    _animationController.Animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightHandWeight);
                    _animationController.Animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightHandHint.position);
                }
                #endregion

                #region LEFT HAND IK
                _animationController.Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                _animationController.Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                _animationController.Animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandObject.position);
                _animationController.Animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandObject.rotation);

                if (leftHandHint)
                {
                    _animationController.Animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, leftHandWeight);
                    _animationController.Animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftHandHint.position);
                }
                #endregion
            }
        }
    }
}
