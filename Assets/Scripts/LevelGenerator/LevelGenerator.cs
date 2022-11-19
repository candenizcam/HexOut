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

            List<CapsuleData> capsules;
            if (capsuleNumber > 0)
            {
                capsules = GenerateCapsuleDataOther(row, col, capsuleNumber,obstacles, capsuleProcedural).capsules;
                
            }
            else
            {
                capsules = new List<CapsuleData>();
            }
            
            
            
            return new LevelData($"level {obstacleSeed}-{capsuleSeed}", row, col, capsules.ToArray(),obstacles.ToArray());
        }

        public static (LevelData ld,float t1, float t2) GenerateFrameLevelWithNewObstacles(LevelData ld, int capsuleSeed, int capsuleNo)
        {
            var capsuleProcedural = new System.Random(capsuleSeed);
            var capsules = GenerateCapsuleDataOther(ld.Row, ld.Col, capsuleNo,ld.ObstacleDatas.ToList(), capsuleProcedural);
            return (new LevelData(ld.Name+$"{capsuleSeed}", ld.Row, ld.Col, capsules.capsules.ToArray(),ld.ObstacleDatas),capsules.t1,capsules.t2);
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
                //var r = procedural.Next(2, row - 2);
                //var c = procedural.Next(2, col - 2);
                //var a = procedural.Next(0, 6);
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
                        Debug.Log($"{i}");
                        break;
                    }
                }
            }

            return capsules;
        }
        
        private static (List<CapsuleData> capsules, float t1, float t2) GenerateCapsuleDataOther(int row, int col, int capsuleNumber,
            List<ObstacleData> obstacles, Random procedural)
        {
            var capsules = new List<CapsuleData>();

            var allCapsules = new List<CapsuleData>();
            var t1 = Time.realtimeSinceStartup;
            
            for (var r = 2; r <= row - 2; r++)
            {
                for (var c = 2; c <= col - 2; c++)
                {
                    for (var a = 0; a < 6; a++)
                    {
                        if((r==2 && (a is 0 or 5)) || (r==row-2 && (a is 2 or 3)) || (c==2 && a<3) ||(c==col-2 && a>=3)) continue;
                        
                        var cd = new CapsuleData(r, c, 2, a, false);
                        allCapsules.Add(cd);
                    }
                }
            }

            var t2 = Time.realtimeSinceStartup;

            
            
            //allCapsules = allCapsules.Where(x => !obstacles.Any(x.ObstaclesBy)).ToList();
            allCapsules = allCapsules.Where(x => !obstacles.Any(x.ObstaclesBy)).ToList();
            
            
            var t3 = Time.realtimeSinceStartup;

            var reordered = allCapsules.OrderBy(x=>procedural.Next()).ToList();

            
            for (var i = 0; i < reordered.Count(); i++)
            {
                if (capsules.Any(x => x.CollidesWith(reordered[i]))) continue;
                capsules.Add(reordered[i]);
                if (capsules.Count() >= capsuleNumber)
                {
                    break;
                }

            }

            
            if (!capsules.Any())
            {
                Debug.LogError($"nothing came from {row},{col},{procedural}");
            }
            return (capsules,t2-t1,t3-t2);
            
            /*
            var rList = Enumerable.Range(2, row-2).ToList();
            var cList = Enumerable.Range(2, col-2).ToList();
            var aList = Enumerable.Range(0, 6).ToList();
            var lList = Enumerable.Repeat(2, capsuleNumber).ToList(); // this will make sure procedural generator works with multiple length types,
            
            for(int i=0;i<1000;i++)
            {
                //var r = procedural.Next(2, row - 2);
                //var c = procedural.Next(2, col - 2);
                //var a = procedural.Next(0, 6);
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
                        Debug.Log($"{i}");
                        break;
                    }
                }
            }
            */
           
        }
        



        
        
        
    }
}