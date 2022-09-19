using UnityEngine;

namespace ClubBusiness
{
    public class CustomerWaypoints : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private Transform[] waypoints;
        public static Transform[] Waypoints { get; private set; }

        private void Start()
        {
            Waypoints = waypoints;
        }
    }
}
