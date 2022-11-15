using System;
using Punity;

namespace DefaultNamespace.GameData
{
    [Serializable]
    public class SerialHexOutData: SerialGameData
    {
        public int playerXp;

        public SerialHexOutData()
        {
            playerXp = 0;
        }
    }
}