using UnityEngine;

namespace ClubBusiness
{
    public class Bar : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private Transform beerSpawnTransform;

        public static Transform BeerSpawnTransform { get; private set; }

        private void Start()
        {
            BeerSpawnTransform = beerSpawnTransform;
        }
    }
}
