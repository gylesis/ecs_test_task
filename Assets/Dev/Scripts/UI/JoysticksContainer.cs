using UnityEngine;

namespace Dev.UI
{
    public class JoysticksContainer : MonoBehaviour
    {
        [SerializeField] private Joystick _movementJoystick;

        public Joystick MovementJoystick => _movementJoystick;
    }
}