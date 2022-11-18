using JetBrains.Annotations;

namespace DefaultNamespace.GameData
{
    public class LevelSeedData
    {
        public string Name;
        public int Row;
        public int Col;
        public int CapsuleSeed;
        public int ObstacleSeed;
        public int CapsuleNumber;
        public int SingleObstacleNumber;
        public int DoubleObstacleNumber;
        public SeedType LevelType;
        public int LevelDifficulty;
        
        public LevelSeedData(string name, int row, int col, int capsuleSeed, int obstacleSeed, int capsuleNumber, int singleObstacleNumber, int doubleObstacleNumber, SeedType levelType, int levelDifficulty)
        {
            Name = name;
            Row = row;
            Col = col;
            CapsuleSeed = capsuleSeed;
            ObstacleSeed = obstacleSeed;
            CapsuleNumber = capsuleNumber;
            SingleObstacleNumber = singleObstacleNumber;
            DoubleObstacleNumber = doubleObstacleNumber;
            LevelType = levelType;
            LevelDifficulty = levelDifficulty;
        }

        public string RecordMe(string name=null,int? capsuleNumber=null, int? capsuleSeed=null, int? difficulty =null)
        {
            return $"new LevelSeedData({name??=Name},{Row},{Col},{capsuleSeed??=CapsuleSeed},{ObstacleSeed},{capsuleNumber??=CapsuleNumber},{SingleObstacleNumber},{DoubleObstacleNumber},LevelSeedData.SeedType.FrameLevel,{difficulty??=LevelDifficulty}),\n";
        }


        public enum SeedType
        {
            FrameLevel
        }
    }
}