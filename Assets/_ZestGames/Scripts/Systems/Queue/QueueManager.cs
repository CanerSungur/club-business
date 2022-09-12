using ClubBusiness;
using UnityEngine;

namespace ZestGames
{
    public class QueueManager : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private GateQueue gateQueue;
        [SerializeField] private BarQueue barQueue;
        [SerializeField] private ToiletQueue toiletQueue;

        #region STATIC QUEUES
        public static GateQueue GateQueue { get; private set; }
        public static BarQueue BarQueue { get; private set; }
        public static ToiletQueue ToiletQueue { get; private set; }
        #endregion
        
        public void Init(GameManager gameManager)
        {
            GateQueue = gateQueue;
            BarQueue = barQueue;
            ToiletQueue = toiletQueue;
        }
    }
}
