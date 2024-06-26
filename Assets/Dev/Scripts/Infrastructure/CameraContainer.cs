using UnityEngine;

namespace Client.Systems
{
    public class CameraContainer : MonoBehaviour
    {
        [SerializeField] private Transform _cameraParent;
        [SerializeField] private Camera _camera;

        public Transform CameraParent => _cameraParent;

        public Camera Camera => _camera;
    }
}