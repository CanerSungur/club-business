using ClubBusiness;
using UnityEngine;

namespace ZestGames
{
    public class QueueManager : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private ExampleQueue exampleQueue;
        [SerializeField] private BarQueue barQueue;
        [SerializeField] private ToiletQueue toiletQueue;

        #region STATIC QUEUES
        public static ExampleQueue ExampleQueue { get; private set; }
        public static BarQueue BarQueue { get; private set; }
        public static ToiletQueue ToiletQueue { get; private set; }
        #endregion
        
        public void Init(GameManager gameManager)
        {
            ExampleQueue = exampleQueue;
            BarQueue = barQueue;
            ToiletQueue = toiletQueue;
        }
    }
}
