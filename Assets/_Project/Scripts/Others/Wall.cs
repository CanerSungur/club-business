using System;
using UnityEngine;

namespace ClubBusiness
{
    public class Wall : MonoBehaviour
    {
        [Header("-- MATERIAL SETUP --")]
        [SerializeField] private Material solidMaterial;
        [SerializeField] private Material transparentMaterial;
        private MeshRenderer _meshRenderer;
        private bool _isTransparent;

        public Action OnBecomeSolid, OnBecomeTransparent;

        private void OnEnable()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _isTransparent = false;
            MakeItSolid();

            OnBecomeSolid += MakeItSolid;
            OnBecomeTransparent += MakeItTransparent;
        }

        private void OnDisable()
        {
            OnBecomeSolid -= MakeItSolid;
            OnBecomeTransparent -= MakeItTransparent;
        }

        private void MakeItSolid() => _meshRenderer.material = solidMaterial;
        private void MakeItTransparent() => _meshRenderer.material = transparentMaterial;
    }
}
