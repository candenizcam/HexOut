using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GameData;
using UnityEngine;

namespace DefaultNamespace
{
    public static class LevelTester
    {
        public static int TestLevel(LevelData ld)
        {
            return RecursiveTest(ld.CapsuleDatas, ld.ObstacleDatas, ld.Row, ld.Col,0,0);
            

        }

        
        private static bool PreTest(CapsuleData[] capsules, ObstacleData[] obstacleDatas, int row, int col)
        {
            var emptyList = new List<CapsuleData>();
            for (int i = 0; i < capsules.Count(); i++)
            {
                var thisGuy = capsules[i];
                var reverseGuy = thisGuy.Revert();
                var fw = GoForth(thisGuy, emptyList, obstacleDatas, row, col,0);
                var bw = GoForth(reverseGuy, emptyList, obstacleDatas, row, col,0);

                if (fw.res == 2 && bw.res ==2)
                {
                    return false;
                }   
            }

            return true;

        }



        private static int RecursiveTest(CapsuleData[] capsules, ObstacleData[] obstacleDatas, int row, int col, int turns, int diff )
        {
            
            if (turns > 100)
            {
                return -1;
            }
            var otherPossible = capsules.ToList();

            var changeCount = 0;
            for (int i = 0; i < capsules.Count(); i++)
            {
                var thisGuy = capsules[i];
                var reverseGuy = thisGuy.Revert();
                var others = otherPossible.Where(x => thisGuy!= x).ToList();

                var fw = GoForth(thisGuy, others, obstacleDatas, row, col,0);
                if (fw.res == 0)
                {
                    changeCount += 1;
                    otherPossible.Remove(thisGuy);
                    continue;
                }
                var bw = GoForth(reverseGuy, others, obstacleDatas, row, col,0);

                if (fw.res == 0 || bw.res == 0)
                {
                    changeCount += 1;
                    otherPossible.Remove(thisGuy);
                }else if (fw.res == 2 && bw.res ==2)
                {
                    return -2;
                }else if ((fw.res == 2 && bw.refStep == 1) || (bw.refStep == 1 && fw.refStep == 1) ||
                          (fw.refStep == 1 && bw.res == 2))
                {
                    return -4;
                }
            }

            if (changeCount==0)
            {
                return -3;
            }

            capsules = otherPossible.ToArray();//capsules.Where(x => !removes.Any(y=> y==x)).ToArray();
            if (capsules.Any())
            {
                return RecursiveTest(capsules, obstacleDatas, row, col,turns+1,diff+changeCount*(turns+1));
            }
            else
            {
                return diff+changeCount*(turns+1);
            }
            
        }
        
        
        


        private static (int res, CapsuleData finalData, int refStep) GoForth(CapsuleData l, List<CapsuleData> otherCapsules, ObstacleData[] obstacles, int row, int col,int step, int refStep=-1)
        {
            if (!l.WithinBounds(row, col)) return (0,l,refStep);

            if (otherCapsules.Any(x => x.CollidesWith(l))) return (1,l,refStep);

            var a = obstacles.Where(x => l.ObstaclesBy(x));
            if (a.Any())
            {
                if (a.Count() > 1)
                {
                    var a2 = a.Where(x => x.Row == l.FirstRow && x.Col == l.FirstCol);
                    if (a2.Count() > 1)
                    {
                        Debug.Log($"two obstacles at the same place");
                    }
                    a = a2;
                }

                var thatGuy = a.First();

                if (l.FirstRow == thatGuy.Row && l.FirstCol == thatGuy.Col && thatGuy.Length == 2)
                {
                    var newAngle = (l.Angle + (l.Angle == thatGuy.Direction ? 4:2))%6;
                    var newerData = new CapsuleData(l.FirstRow, l.FirstCol, l.Length, newAngle,false);
                    return GoForth(newerData, otherCapsules, obstacles, row, col,step+1, refStep==-1?step+1:refStep);
                }
                else
                {
                    return (2,l,refStep);
                }

                

            }

            var newl = l.DataFromFirst(1);
            return GoForth(newl, otherCapsules, obstacles, row, col,step+1);
        }
    }
}