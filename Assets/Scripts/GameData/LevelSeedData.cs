namespace DefaultNamespace.GameData
{
    public class LevelSeedData
    {
        public int Row;
        public int Col;
        public int CapsuleSeed;
        public int ObstacleSeed;
        public int CapsuleNumber;
        public int SingleObstacleNumber;
        public int DoubleObstacleNumber;
        public SeedType LevelType;
        public int LevelDifficulty;
        
        public LevelSeedData(int row, int col, int capsuleSeed, int obstacleSeed, int capsuleNumber, int singleObstacleNumber, int doubleObstacleNumber, SeedType levelType, int levelDifficulty)
        {
            Row = row;
            Col = col;
            CapsuleSeed = capsuleSeed;
            ObstacleSeed = obstacleSeed;
            CapsuleNumber = capsuleNumber;
            SingleObstacleNumber = doubleObstacleNumber;
            DoubleObstacleNumber = singleObstacleNumber;
            LevelType = levelType;
            LevelDifficulty = levelDifficulty;
        }



        public enum SeedType
        {
            FrameLevel
        }
    }
}