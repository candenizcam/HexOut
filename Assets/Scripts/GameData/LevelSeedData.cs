namespace DefaultNamespace.GameData
{
    public class LevelSeedData
    {
        public int Row;
        public int Col;
        public int CapsuleSeed;
        public int ObstacleSeed;
        public int CapsuleNumber;
        public int ObstacleNumber;
        public SeedType LevelType;
        
        public LevelSeedData(int row, int col, int capsuleSeed, int obstacleSeed, int capsuleNumber, int obstacleNumber, SeedType levelType)
        {
            Row = row;
            Col = col;
            CapsuleSeed = capsuleSeed;
            ObstacleSeed = obstacleSeed;
            CapsuleNumber = capsuleNumber;
            ObstacleNumber = obstacleNumber;
            LevelType = levelType;
        }



        public enum SeedType
        {
            FrameLevel
        }
    }
}