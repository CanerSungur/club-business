using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class AiAppearanceHandler : MonoBehaviour
    {
        private Ai _ai;

        [Header("-- SETUP --")]
        [SerializeField] private Material[] materials;
        [SerializeField] private SkinnedMeshRenderer meshRenderer;

        public void Init(Ai ai)
        {
            if (_ai == null)
                _ai = ai;

            SetRandomMaterial();
        }

        private void SetRandomMaterial()
        {
            meshRenderer.material = materials[Random.Range(0, materials.Length)];
        }
    }
}
