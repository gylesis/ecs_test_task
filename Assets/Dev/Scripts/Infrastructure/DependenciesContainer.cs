using System.Collections.Generic;
using Dev.Systems;
using Dev.UI;
using UnityEngine;

namespace Client.Systems
{
    public class DependenciesContainer : MonoBehaviour
    {
        [SerializeField] private Player _spawnedPlayer;
        [SerializeField] private CameraContainer mainCameraContainerContainer;
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private JoysticksContainer _joysticksContainer;
        [SerializeField] private StackUIView _stackUIViewPrefab;
        
        [SerializeField] private ObjectStack _objectStackPrefab;

        [SerializeField] private List<ObjectStackSpawnZone> _objectsStackSpawnZones;
        [SerializeField] private List<ObjectStackDepositZone> _objectsDepositSpawnZones;
        [SerializeField] private Factory _factory;
        
        public List<ObjectStackSpawnZone> ObjectsStackSpawnZones => _objectsStackSpawnZones;
        public List<ObjectStackDepositZone> ObjectsStackDepositZones => _objectsDepositSpawnZones;

        
        public StackUIView StackUIViewPrefab => _stackUIViewPrefab;
        public ObjectStack ObjectStackPrefab => _objectStackPrefab;
        public Player SpawnedPlayer => _spawnedPlayer;
        public CameraContainer MainCameraContainer => mainCameraContainerContainer;
        public GameSettings GameSettings => _gameSettings;
        public JoysticksContainer JoysticksContainer => _joysticksContainer;
        public Factory Factory => _factory;

        public GameState GameState;
        
        
        private Vector3 _lastPos = Vector3.zero;
        private int _rowsCount;

        private void Awake()
        {
            GameState = new GameState();
        }
        
        public Vector3 GetBagStackPosition(Vector3 startPoint, int number)
        {
            float offset = 0.15f;
            
            startPoint += Vector3.up * (number * offset);
            return startPoint;
        }
        
        
        
    }
    
    
}