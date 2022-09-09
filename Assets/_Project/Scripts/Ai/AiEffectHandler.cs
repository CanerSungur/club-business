using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class AiEffectHandler : MonoBehaviour
    {
        private Ai _ai;
        private FightSmoke _fightSmoke;

        public void Init(Ai ai)
        {
            if (_ai == null)
            {
                _ai = ai;
                _fightSmoke = GetComponentInChildren<FightSmoke>();
                _fightSmoke.Init(_ai);
            }
        }
    }
}
