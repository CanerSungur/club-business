using ClubBusiness;
using UnityEngine;

namespace ZestGames
{
    public class QueueManager : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private ExampleQueue exampleQueue;
        [SerializeField] private BarQueue barQueue;

        #region STATIC QUEUES
        public static ExampleQueue ExampleQueue { get; private set; }
        public static BarQueue BarQueue { get; private set; }
        #endregion
        
        public void Init(GameManager gameManager)
        {
            ExampleQueue = exampleQueue;
            BarQueue = barQueue;
        }
    }
}
