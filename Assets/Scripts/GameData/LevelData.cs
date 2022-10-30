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
        
        
        public LevelData(string name, int row, int col, CapsuleData[] capsuleDatas, ObstacleData[] obstacleDatas)
        {
            Name = name;
            Row = row;
            Col = col;
            CapsuleDatas = capsuleDatas;
            ObstacleDatas = obstacleDatas;
        }
    }
}