using UnityEngine;

namespace ClubBusiness
{
    public class BouncerTrigger : MonoBehaviour
    {
        public Bouncer Bouncer { get; private set; }

        public void Init(Bouncer bouncer)
        {
            if (Bouncer == null)
                Bouncer = bouncer;
        }
    }
}
