using UnityEngine;
using ZestGames;

namespace ClubBusiness
{
    public class CameraRayShooter : MonoBehaviour
    {
        private Camera _camera;
        private Player _player;
        private Wall _transparentWall = null;
        private Wall _hitWall = null;

        private RaycastHit _hit;
        private Ray _ray;

        private void Awake()
        {
            _player = FindObjectOfType<Player>();
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            //_ray = _camera.ScreenPointToRay(_player.transform.position);
            //_ray = _camera.ScreenPointToRay(_player.transform.position);
            Vector3 dir = _player.transform.position - _camera.transform.position;

            if (Physics.Raycast(_camera.transform.position, dir, out _hit))
            {
                if (_hit.transform.TryGetComponent(out _hitWall))
                {
                    if (_transparentWall && _transparentWall != _hitWall)
                        _transparentWall.OnBecomeSolid?.Invoke();

                    _hitWall.OnBecomeTransparent?.Invoke();
                    _transparentWall = _hitWall;
                }
                else
                {
                    if (_transparentWall)
                        _transparentWall.OnBecomeSolid?.Invoke();

                    _transparentWall = _hitWall = null;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_camera && _player)
                Gizmos.DrawRay(_camera.transform.position, _player.transform.position - _camera.transform.position);
        }
    }
}
