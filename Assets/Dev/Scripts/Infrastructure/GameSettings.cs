using UnityEngine;

namespace Client.Systems
{
    [CreateAssetMenu(menuName = "StaticData/GameSettings", fileName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private float _cameraMoveSpeed = 10;
        [SerializeField] private float _playerMoveSpeed = 10;

        [SerializeField] private float objectStackFollowSpeed = 20f;
        [SerializeField] private AnimationCurve _objectStackFollowSpeedCurve;

        [SerializeField] private float _objectStackGenerationCooldown = 2;
        [SerializeField] private int _objectsStackGeneratedMaxAmount = 15;

        [SerializeField] private float _objectFlyDuration = 0.5f;

        [SerializeField] private int _stackWidth = 5;
        [SerializeField] private int stackLength = 5;

        public int StackWidth => _stackWidth;
        public int StackLength => stackLength;

        public float ObjectFlyDuration => _objectFlyDuration;
        public float ObjectStackGenerationCooldown => _objectStackGenerationCooldown;
        public int ObjectsStackGeneratedMaxAmount => _objectsStackGeneratedMaxAmount;
        public AnimationCurve ObjectStackFollowSpeedCurve => _objectStackFollowSpeedCurve;
        public float ObjectStackFollowSpeed => objectStackFollowSpeed;
        public float CameraMoveSpeed => _cameraMoveSpeed;
        public float PlayerMoveSpeed => _playerMoveSpeed;
    }
}