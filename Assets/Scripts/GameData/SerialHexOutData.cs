using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool SkinSelectionActive()
        {
            return activeSkins.Count>1;
        }

        public bool UnlockNewSkins(int newSkinNo)
        {
            if(newSkinNo<=0) return false;
            var allSkins = Enum.GetValues(typeof(SkinType)).Cast<SkinType>().ToList();

            var b = false;
            var counter = newSkinNo;
            for (int i = 0; i < allSkins.Count; i++)
            {
                if(activeSkins.Contains(allSkins[i])) continue;
                b = true;
                activeSkins.Add(allSkins[i]);
                counter -= 1;
                if (counter <= 0)
                {
                    break;
                }

            }

            return b;

        }

        
    }
}