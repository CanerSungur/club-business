using UnityEngine;
using ZestCore.Utility;
using ZestGames;

namespace ClubBusiness
{
    public class AreaManager : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private Collider[] idleAreas;
        [SerializeField] private Collider danceArea;

        #region STATICS
        private static Collider[] _idleAreas;
        private static Collider _danceArea;
        #endregion

        public void Init(GameManager gameManager)
        {
            _idleAreas = idleAreas;
            _danceArea = danceArea;
        }

        #region PUBLIC STATICS
        public static Vector3 GetRandomIdleAreaPosition()
        {
            Collider idleArea = _idleAreas[Random.Range(0, _idleAreas.Length)];
            return RNG.RandomPointInBounds(idleArea.bounds);
        }
        public static Vector3 GetRandomDanceAreaPosition() => RNG.RandomPointInBounds(_danceArea.bounds);
        #endregion
    }
}
