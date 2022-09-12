using UnityEngine;

namespace ClubBusiness
{
    public class BodyguardStats : MonoBehaviour
    {
        private Bodyguard _bodyguard;


        public void Init(Bodyguard bodyguard)
        {
            if (_bodyguard == null)
                _bodyguard = bodyguard;
        }
    }
}
