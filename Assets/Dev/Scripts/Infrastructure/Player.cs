using UnityEngine;

namespace Client.Systems
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform bagTransform;

        [SerializeField] private Animator _animator;

        public Animator Animator => _animator;
        public Transform BagTransform => bagTransform;

        public int EntityId;
    }
}