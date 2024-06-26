using Client.Systems;
using Dev.Systems;
using UnityEngine;

namespace Client
{
    public class Factory : MonoBehaviour
    {
        [SerializeField] private DependenciesContainer _dependenciesContainer;
        
        public StackUIView CreateStackUI(Vector3 pos)
        {
            return Instantiate(_dependenciesContainer.StackUIViewPrefab, pos, Quaternion.identity);
        }
        
    }
}