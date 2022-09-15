using UnityEngine;

namespace ClubBusiness
{
    public abstract class ToiletItem : MonoBehaviour
    {
        public Transform PointTransform { get; private set; }

        private void OnEnable()
        {
            PointTransform = transform.GetChild(0);
            Toilet.AddEmptyToiletItem(this);
        }

        #region PUBLICS
        public void Occupy()
        {
            Toilet.RemoveEmptyToiletItem(this);
        }
        public void Release()
        {
            Toilet.AddEmptyToiletItem(this);
        }
        #endregion
    }
}
