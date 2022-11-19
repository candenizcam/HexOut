using System.Collections.Generic;

namespace DefaultNamespace.GameData
{
    public class LevelData
    {
        public string Name;
        public int Row;
        public int Col;
        public CapsuleData[] CapsuleDatas;
        public ObstacleData[] ObstacleDatas;
        public int Difficulty;
        
        
        public LevelData(string name, int row, int col, CapsuleData[] capsuleDatas, ObstacleData[] obstacleDatas, int difficulty=0)
        {
            Name = name;
            Row = row;
            Col = col;
            CapsuleDatas = capsuleDatas;
            ObstacleDatas = obstacleDatas;
            Difficulty = difficulty;
        }
    }
}