using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GameData;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace
{
    public class LevelGenerator
    {

        public static LevelData GenerateFrameLevel(int capsuleSeed, int obstacleSeed, int row, int col, int capsuleNumber, int obstacleNumber)
        {
            var capsuleProcedural = new System.Random(capsuleSeed);
            var obstacleProcedural  = new System.Random(obstacleSeed);

            var allObstacles = GenerateObstacles(row, col,obstacleNumber, obstacleProcedural);

            

            var capsules = GenerateCapsuleData(row, col, capsuleNumber, allObstacles.raw, capsuleProcedural);
            
            
            return new LevelData($"level {capsuleProcedural}-{obstacleProcedural}", row, col, capsules.ToArray(),allObstacles.real.ToArray());
        }


        private static List<CapsuleData> GenerateCapsuleData(int row, int col, int capsuleNumber,
            List<ObstacleData> obstacles, Random procedural)
        {
            var capsules = new List<CapsuleData>();
            var rList = Enumerable.Range(2, row-2).ToList();
            var cList = Enumerable.Range(2, col-2).ToList();
            var aList = Enumerable.Range(0, 6).ToList();
            var lList = Enumerable.Repeat(2, capsuleNumber).ToList(); // this will make sure procedural generator works with multiple length types,
            
            for(int i=0;i<1000;i++)
            {
                var r = rList.OrderBy(a => procedural.Next()).First();
                var c = cList.OrderBy(a => procedural.Next()).First();
                var l = lList.OrderBy(a => procedural.Next()).First();
                var a = aList.OrderBy(a => procedural.Next()).First();
                var d = new CapsuleData(r,c,l,a);


                if (d.TwoIndexTiles().Any(t => t.row < 2 || t.row > row-1 || t.col < 2 || t.col > col-1))
                {
                    continue;
                }

                if (obstacles.Any(x => d.ObstaclesBy(x)))
                {
                    continue;
                }
                

                if (!capsules.Any(x => x.CollidesWith(d)))
                {
                    capsules.Add(d);
                    if (capsules.Count >= capsuleNumber)
                    {
                        Debug.Log($"done in {i}");
                        break;
                    }
                }
            }

            return capsules;
        }
        

        private static (List<ObstacleData> raw, List<ObstacleData> real ) GenerateObstacles(int row, int col, int obstacleNumber, System.Random procedural)
        {
            var rawObstacles = new List<ObstacleData>();

            for (int a = 0; a < 3; a++)
            {
                rawObstacles.Add(new ObstacleData(2, 2, a + 4));
                rawObstacles.Add(new ObstacleData(2, col-2, a + 5));
                rawObstacles.Add(new ObstacleData(row-1, col-2, a+1));
                rawObstacles.Add(new ObstacleData(row-1, 2, a + 2));
            }
            
            
            for (int r = 3; r < row - 1; r++)
            {
                for(int a=0;a<3;a++)
                {
                    rawObstacles.Add(new ObstacleData(r, 2- (r+1)%2, a + 3));
                    rawObstacles.Add(new ObstacleData(r, col-1,a));
                }
            }
            
            for (int c = 3; c < col - 2; c++)
            {
                for(int a=0;a<2;a++)
                {
                    rawObstacles.Add(new ObstacleData(2, c, a + 5));
                    rawObstacles.Add(new ObstacleData(row-1, c,a + 2));
                }
            }


            return (rawObstacles,rawObstacles.OrderBy(x => procedural.NextDouble())
                .ToList()
                .GetRange(0,obstacleNumber));
        }
        
        
        
        
    }
}