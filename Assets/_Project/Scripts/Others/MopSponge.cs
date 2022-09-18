using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class MopSponge : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private Transform userTransform;

        private void LateUpdate()
        {
            transform.up = Vector3.up;
            transform.forward = userTransform.forward;
        }
    }
}
