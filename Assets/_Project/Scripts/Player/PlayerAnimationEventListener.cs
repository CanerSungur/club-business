using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class PlayerAnimationEventListener : MonoBehaviour
    {
        private PlayerAnimationController _animationController;

        public void Init(PlayerAnimationController animationController)
        {
            if (_animationController == null)
                _animationController = animationController;
        }

        public void SelectRandomAskForDrinkAnim()
        {
            
        }
    }
}
