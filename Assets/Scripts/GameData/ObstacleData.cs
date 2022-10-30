namespace DefaultNamespace.GameData
{
    public class ObstacleData
    {
        public int Row;
        public int Col;
        public int Direction;
        
        public ObstacleData(int row, int col, int direction)
        {
            Row = row;
            Col = col;
            Direction = direction%6;
        }
        
        public (int OtherRow, int OtherCol) Opposition()
        {
            var a = Direction;
            var l = 1;
            
            switch (a)
            {
                case 0:
                    return (Row-1, Col + (l+(Row+1)%2) / 2);
                case 1:
                    return (Row, Col+1);
                case 2:
                    return (Row+1, Col + (1+(Row+1)%2) / 2);
                case 3:
                    return (Row+1, Col- (1+(Row)%2 )/2);
                case 4:
                    return (Row, Col-1);
                default:
                    return (Row-1, Col- (1+(Row)%2 )/2);
            }
        }
        
        
    }
}