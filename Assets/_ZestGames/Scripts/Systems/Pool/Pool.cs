using UnityEngine;

namespace ZestGames
{
    [System.Serializable]
    public class Pool
    {
        public Enums.PoolStamp PoolStamp;
        public GameObject[] Prefabs;
        public int Size;
    }
}
