using UnityEngine;
using ZestCore.Utility;
using ZestGames;

namespace ClubBusiness
{
    public class AreaManager : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private Collider[] idleAreas;

        #region STATICS
        private static Collider[] _idleAreas;
        #endregion

        public void Init(GameManager gameManager)
        {
            _idleAreas = idleAreas;
        }

        #region PUBLIC STATICS
        public static Vector3 GetRandomIdleAreaPosition()
        {
            Collider idleArea = _idleAreas[Random.Range(0, _idleAreas.Length)];
            return RNG.RandomPointInBounds(idleArea.bounds);
        }
        #endregion
    }
}
