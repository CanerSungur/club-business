using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class AiTrigger : MonoBehaviour
    {
        public Ai Ai { get; private set; }
        public bool PlayerIsInTrigger { get; set; }

        public void Init(Ai ai)
        {
            if (Ai == null)
                Ai = ai;

            PlayerIsInTrigger = false;
        }
    }
}
