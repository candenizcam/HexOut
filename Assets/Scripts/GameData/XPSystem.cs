using System;
using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace.GameData
{
    public static class XPSystem
    {
        public static int BaseXp = 10;
        public static int expectedInt = 5;
        public static int expetedCapsule = 5; // buna bi bakalım sonra

        public static int GameXP(int difficulty, int capsule, int offset = 0)
        {
            return (difficulty * capsule + offset) * BaseXp;
        }

        public static int LevelXp(int playerLevel)
        {
            var draws = DiffDrawFromLevelNo(playerLevel).Select(x=> GameXP(x,expetedCapsule)).ToList();

            var drawMean = draws.Sum() / (float) draws.Count;

            return (int)(drawMean * Math.Clamp(playerLevel/5f,1f,expectedInt));


        }

        public static int CapsuleXp(int levelDiff)
        {
            return levelDiff * BaseXp;
        }
        

        public static List<int> DiffDrawFromLevelNo(int levelNo)
        {
            var draws = new List<int>();
            draws.Add(1);

            draws.AddRange(Enumerable.Repeat(1, Math.Min(10, levelNo)));
            if (levelNo < 10) return draws;
            
            draws.AddRange(Enumerable.Repeat(1, Math.Min(10, levelNo/5)));
            if (levelNo < 50) return draws;
            
            draws.AddRange(Enumerable.Repeat(1, Math.Min(10, levelNo/25)));
            return draws;
        }

        public static bool NewSkinInLevel(int levelNo)
        {
            return levelNo % 10 == 0;
        }

        public static (int newLevel, int newXp, int newSkin) AddXP(int oldLevel, int oldXp, int deltaXp, int newSkin=0)
        {
            var newXp = oldXp + deltaXp;
            var levelXp = LevelXp(oldLevel);
            return newXp > levelXp ? AddXP(oldLevel + 1, 0, newXp - levelXp, NewSkinInLevel(oldLevel+1) ? newSkin+1:newSkin) : (oldLevel, newXp, 0);
            
            
        }
        
    }
}