using UnityEngine;

namespace ClubBusiness
{
    public class CleanerTrigger : MonoBehaviour
    {
        public Cleaner Cleaner { get; private set; }

        public void Init(Cleaner cleaner)
        {
            if (Cleaner == null)
                Cleaner = cleaner;
        }
    }
}
