using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GameData;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace
{
    public class LevelGenerator
    {

        public static LevelData GenerateSeededLevel(LevelSeedData lsd)
        {
            if (lsd.LevelType == LevelSeedData.SeedType.FrameLevel)
            {
                return GenerateFrameLevel(lsd.CapsuleSeed, lsd.ObstacleSeed, lsd.Row, lsd.Col, lsd.CapsuleNumber,
                    lsd.SingleObstacleNumber,lsd.DoubleObstacleNumber);
            }
            else
            {
                throw new Exception("invalid level type");
            }
        }
        
        public static LevelData GenerateFrameLevel(int capsuleSeed, int obstacleSeed, int row, int col, int capsuleNumber, int obstacleNumber, int doubleObstacleNumber)
        {
            var capsuleProcedural = new System.Random(capsuleSeed);
            var obstacleProcedural  = new System.Random(obstacleSeed);
            //var allObstacles = new List<ObstacleData>();
            var obstacles = ObstacleGenerator.GenerateMixedObstacles(row, col,obstacleNumber, doubleObstacleNumber,obstacleProcedural);

            var capsules = GenerateCapsuleData(row, col, capsuleNumber,obstacles, capsuleProcedural);
            
            
            return new LevelData($"level {capsuleProcedural}-{obstacleProcedural}", row, col, capsules.ToArray(),obstacles.ToArray());
        }

        public static LevelData GenerateRawFrame(LevelSeedData lsd, int l)
        {
            var o = l==1? ObstacleGenerator.SingleFrameObstacles(lsd.Row, lsd.Col) : ObstacleGenerator.DoubleFrameObstacles(lsd.Row,lsd.Col)  ;
            
            return new LevelData($"level dud", lsd.Row, lsd.Col, new CapsuleData[]{},o.ToArray());
            
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
                var d = new CapsuleData(r,c,l,a,false);


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
                        break;
                    }
                }
            }

            return capsules;
        }
        

        



        
        
        
    }
}