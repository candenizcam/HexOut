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

        public static (string produced, string res) TotalRandom(int amount, int procRow, int procCol,int singleObstacle, int doubleObstacle, int seed)
        {
            var r = new Random(seed);

            var res = "failed to produce";
            var minCap = (procRow - 2) * (procCol - 2) / 4;
            var maxCap = (procRow - 2) * (procCol - 2) / 2;
            var n = 0;
            var s = "";
            for (int i = 0; i < amount * 20; i++)
            {
                var capNo = r.Next(minCap, maxCap);
                var obsSeed = r.Next();
                var capSeed = r.Next();
                var generatedSeed = new LevelSeedData($"\"{procRow}_{procCol}_{seed}_{i}\"", procRow, procCol, capSeed,
                    obsSeed, capNo, singleObstacle, doubleObstacle, LevelSeedData.SeedType.FrameLevel,0);
                var generatedLevel = LevelGenerator.GenerateSeededLevel(generatedSeed);
                
                var testResult = LevelTester.TestLevel(generatedLevel);
                if (testResult >= 0)
                {
                    n += 1;
                    s += generatedSeed.RecordMe(difficulty: testResult);
                }

                if (n >= amount)
                {
                    res = $"Sucessfully generated {amount} levels in {i} attempts";
                    
                    break;
                }

            }
            return (s,res);


        }

        public static void ProceduralBatch(int procRow, int procCol)
        {
            var v1 = 0f;
            var v2 = 0f;
            var s = "";
            var r = new Random();

            for (int totalObs = 2; totalObs < 5; totalObs++)
            {
                var totalObstacles = totalObs * procCol;
                for (int doubleObs = 1; doubleObs <= totalObs - 2; doubleObs++)
                {

                    var doubleObstacles = doubleObs * procCol;
                    var singleObstacles = totalObstacles - doubleObstacles;
                    for (int os = 0; os < 6; os++)
                    {
                        var obstacleSeed = totalObs * 1000000 + doubleObs * 10000 + r.Next(0,9999);
                        var editorSeed = new LevelSeedData("testLevel", procRow, procCol, 0, obstacleSeed, 0,
                            singleObstacles, doubleObstacles, LevelSeedData.SeedType.FrameLevel, 1);
                        var obstacleOnly = LevelGenerator.GenerateSeededLevel(editorSeed); //0.00123

                        var possible = ((procRow - 2) * (procCol - 2)) / 2;

                        var pCount = 0;
                        for (int p = 0; p < possible / 2; p++)
                        {
                            var capsuleNo = possible - 2 * p;
                            if(capsuleNo< ((procRow - 2) * (procCol - 2)) / 5) break;

                            var n = 0;
                            for (int i2 = 0; i2 < 100; i2++)
                            {
                                var seed = r.Next();
                                //0.001
                                var t1 = Time.realtimeSinceStartup;
                                var d = LevelGenerator.GenerateFrameLevelWithNewObstacles(obstacleOnly, seed,
                                    capsuleNo); //0.000362
                                if (d.ld.CapsuleDatas.Count() < capsuleNo)
                                {
                                    continue;
                                }
                                
                                
                                
                                var b = LevelTester.TestLevel(d.ld); //0.00065
                                var t2 = Time.realtimeSinceStartup;
                                v1 += t2-t1;
                                v2 += 1f;
                                if (b >= 0)
                                {
                                    n += 1;
                                    s += editorSeed.RecordMe(name: $"\"{obstacleSeed}_{capsuleNo}_{seed}\"",
                                        capsuleSeed: seed, capsuleNumber: capsuleNo, difficulty: b);
                                }


                                if (n > 2)
                                {
                                    break;
                                }
                            }

                            if (n > 0)
                            {
                                pCount += 1;
                            }
                        }

                        if (pCount > 3)
                        {
                            break;
                        }
                    }
                }
            }
            
            
            Debug.Log(s);
            Debug.Log($"mean is: {v1/v2}");
        }
        
        public static LevelData GenerateSeededLevel(LevelSeedData lsd)
        {
            if (lsd.LevelType == LevelSeedData.SeedType.FrameLevel)
            {
                return GenerateFrameLevel(lsd.CapsuleSeed, lsd.ObstacleSeed, lsd.Row, lsd.Col, lsd.CapsuleNumber,
                    lsd.SingleObstacleNumber,lsd.DoubleObstacleNumber, lsd.LevelDifficulty);
            }
            else
            {
                throw new Exception("invalid level type");
            }
        }
        
        public static LevelData GenerateFrameLevel(int capsuleSeed, int obstacleSeed, int row, int col, 
            int capsuleNumber, int obstacleNumber, int doubleObstacleNumber, int diff)
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
            
            
            
            return new LevelData($"level {obstacleSeed}-{capsuleSeed}", row, col, capsules.ToArray(),obstacles.ToArray(),diff);
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