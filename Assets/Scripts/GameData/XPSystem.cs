using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace.GameData
{
    public static class XPSystem
    {
        public static int BaseXp = 10;
        public static int NewSkinUnlockLevels = 5; //5,15,30,50 = 

        public static int GameXP(int difficulty, int capsule, int offset = 0)
        {
            return (difficulty * capsule + offset) * BaseXp;
        }

        public static int LevelXp(int playerLevel)
        {
            return 25 * playerLevel*BaseXp;
            //var draws = DiffDrawFromLevelNo(playerLevel).Select(x=> GameXP(x,expetedCapsule)).ToList();
            //var drawMean = draws.Sum() / (float) draws.Count;
            //return (int)(drawMean * Math.Clamp(playerLevel/5f,1f,expectedInt));
        }

        public static int CapsuleXp(int levelDiff)
        {
            return levelDiff * BaseXp;
        }
        

        public static (LevelSeedData data, int index) DrawGameLevelFromNo(int levelNo, List<string> playedLevels)
        {
            var p=GameDataBase.LevelSeedDatas
                .Where(x => x.LevelDifficulty < levelNo  + 5)
                .ToList();

            if (!p.Any())
            {
                var d = GameDataBase.LevelSeedDatas.Min(x => x.LevelDifficulty);
                p = GameDataBase.LevelSeedDatas.Where(x => x.LevelDifficulty == d).ToList();
            }


            var onlyNew = p.Where(x=>playedLevels.All(y => y != x.Name)).ToList();

            if (onlyNew.Any())
            {
                p = onlyNew;
            }

            var r = new Random();
            var ind = r.Next(0, p.Count());
            return (p[ind],ind);
        }

        public static bool NewSkinInLevel(int levelNo, int times = 1)
        {
            
            if (levelNo < NewSkinUnlockLevels)
            {
                return levelNo == 0;
            }

            return NewSkinInLevel(levelNo - NewSkinUnlockLevels*times, times+1);
        }

        public static (int newLevel, int newXp, int newSkin) AddXP(int oldLevel, int oldXp, int deltaXp, int newSkin=0)
        {
            var newXp = oldXp + deltaXp;
            var levelXp = LevelXp(oldLevel);
            return newXp >= levelXp ? AddXP(oldLevel + 1, 0, newXp - levelXp, NewSkinInLevel(oldLevel+1) ? newSkin+1:newSkin) : (oldLevel, newXp, newSkin);
            
            
        }
        
    }
}