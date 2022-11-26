using System.Linq;
using DefaultNamespace.GameData;
using UnityEngine;

namespace DefaultNamespace
{
    public partial class TestMainScript
    {
        private void LevelFromEditorData()
        {
            var editorSeed2 = new LevelSeedData("test level",testRow, testCol, testCapsuleSeed, testObstacleSeed, testCapsuleNumber,
                testObstacleNumber,testDoubleObstacleNumber,LevelSeedData.SeedType.FrameLevel,1);
            GameDataBase.SetSkinType(skinType);
            Debug.Log(editorSeed2.RecordMe());
            ActivateLevel(editorSeed2);
            levelId = editorSeed2.Name;
            levelDiff = editorSeed2.LevelDifficulty;
        }
        

        private void SortGameData()
        {
            var s = "";
            //LevelGenerator.ProceduralBatch(5, 9);
            GameDataBase.LevelSeedDatas
                .OrderBy(x => x.LevelDifficulty * x.CapsuleNumber)
                .Select(x => x.RecordMe()).ToList().ForEach(x => { s += x;});
                
            Debug.Log(s);
        }

        private void NewProcedural()
        {
            var obsArray = new (int Sin, int Doub)[]{(1,1),(1,2),(2,1),(2,2),(2,3),(3,2) };
            var r = 15;
            var c = 9;
            var seperates = 5;
            var res = "";
            var prods = "";
            for (int j = 0; j < obsArray.Length; j++)
            {
                for (int i = 0; i < seperates; i++)
                {
                    var q = LevelGenerator.TotalRandom(20,r,c,obsArray[j].Sin*c,obsArray[j].Doub*c,j*seperates+i+1);
                    res += q.res+"\n";
                    prods += q.produced;
                }
                
            }
            
            
            Debug.Log(prods);
            Debug.Log(res);
        }
    }
}