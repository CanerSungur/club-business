using UnityEngine;

namespace ClubBusiness
{
    public class BodyguardTrigger : MonoBehaviour
    {
        public Bodyguard Bodyguard { get; private set; }

        public void Init(Bodyguard bodyguard)
        {
            if (Bodyguard == null)
                Bodyguard = bodyguard;
        }
    }
}
