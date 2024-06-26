using System.Collections.Generic;

namespace Client.Systems
{
    public class GameState
    {
        public Dictionary<int, List<ObjectStack>> SpawnedStackObjectsInSpawnZones = new Dictionary<int, List<ObjectStack>>();
        public Dictionary<int, List<ObjectStack>> SpawnedStackObjectsInBag = new Dictionary<int, List<ObjectStack>>();

        public Dictionary<int, int> OwnersToBags = new Dictionary<int, int>(); // bag to owner dictionary;
    }
}   