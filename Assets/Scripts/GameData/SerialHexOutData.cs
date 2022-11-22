using System;
using System.Collections.Generic;
using Punity;

namespace DefaultNamespace.GameData
{
    [Serializable]
    public class SerialHexOutData: SerialGameData
    {
        public int playerXp; //in this level
        public int playerLevel;
        public List<string> playedLevels;
        public List<SkinType> activeSkins;
        public SkinType activeSkin;

        public SerialHexOutData()
        {
            playerXp = 0;
            playerLevel = 1;
            playedLevels = new List<string>();
            activeSkins = new List<SkinType>(){SkinType.Simple};
            activeSkin = SkinType.Simple;
        }

        
    }
}