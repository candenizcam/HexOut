using System;
using Punity;

namespace DefaultNamespace.GameData
{
    [Serializable]
    public class SerialHexOutData: SerialGameData
    {
        public int playerXp; //in this level
        public int playerLevel;

        public SerialHexOutData()
        {
            playerXp = 0;
            playerLevel = 1;
        }
    }
}