using UnityEngine;

namespace ClubBusiness
{
    public class BartenderTrigger : MonoBehaviour
    {
        public Bartender Bartender { get; private set; }

        public void Init(Bartender bartender)
        {
            if (Bartender == null)
                Bartender = bartender;
        }
    }
}
