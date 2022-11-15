using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GameData;
using UnityEngine;

namespace DefaultNamespace
{
    public static class ObstacleGenerator
    {
        
        public static (List<ObstacleData> raw, List<ObstacleData> real ) GenerateObstacles(int row, int col, int obstacleNumber, List<ObstacleData> others, System.Random procedural)
        {
            var rawObstacles = SingleFrameObstacles(row, col);


            return (rawObstacles,rawObstacles.OrderBy(x => procedural.NextDouble())
                .ToList()
                .GetRange(0,obstacleNumber));
        }
        
        public static (List<ObstacleData> raw, List<ObstacleData> real ) GenerateDoubleObstacles(int row, int col, int obstacleNumber, List<ObstacleData> others, System.Random procedural)
        {

            var rawObstacles = DoubleFrameObstacles(row, col);

            return (rawObstacles,rawObstacles.OrderBy(x => procedural.NextDouble())
                .ToList()
                .GetRange(0,obstacleNumber));
        }


        public static List<ObstacleData> GenerateMixedObstacles(int row, int col, int singleObstacleNumber, int doubleObstacleNumber, System.Random procedural)
        {
            var data = SingleFrameObstacles(row, col).OrderBy(x => procedural.NextDouble())
                .ToList()
                .GetRange(0,singleObstacleNumber);
            
            
            foreach (var obstacleData in DoubleFrameObstacles(row,col)
                         .OrderBy(x => procedural.NextDouble())
                         .ToList())
            {
                if(data.Any(x=> x.Collides(obstacleData))) continue;
                
                data.Add(obstacleData);
                if(data.Count>= singleObstacleNumber+doubleObstacleNumber) break;
            }

            string s = "";
            foreach (var obstacleData in data)
            {
                s += $"{obstacleData.Row}, {obstacleData.Col}, {obstacleData.Direction}, {obstacleData.Length}\n";
            }
            Debug.Log(s);
            

            return data;
        }
        
        public static List<ObstacleData> SingleFrameObstacles(int row, int col)
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

            return rawObstacles;
        }
        
        
        public static List<ObstacleData> DoubleFrameObstacles(int row, int col)
        {
            var rawObstacles = new List<ObstacleData>();

            for (int a = 0; a < 3; a++)
            {
                rawObstacles.Add(new ObstacleData(2, 2, a + 4,2));
                rawObstacles.Add(new ObstacleData(2, col-2, a + 5,2));
                rawObstacles.Add(new ObstacleData(row-1, col-2, a+1,2));
                rawObstacles.Add(new ObstacleData(row-1, 2, a + 2,2));
            }
            
            
            for (int r = 3; r < row - 1; r++)
            {
                for(int a=0;a<4;a++)
                {
                    rawObstacles.Add(new ObstacleData(r, 2- (r+1)%2, a + 2,2));
                    rawObstacles.Add(new ObstacleData(r, col-1,a+5,2));
                }
            }
            
            for (int c = 3; c < col - 2; c++)
            {
                for(int a=0;a<3;a++)
                {
                    rawObstacles.Add(new ObstacleData(2, c, a + 5,2));
                    rawObstacles.Add(new ObstacleData(row-1, c,a + 2,2));
                }
            }

            return rawObstacles;
        }
    }
}