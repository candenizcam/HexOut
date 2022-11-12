using System.Collections.Generic;

namespace DefaultNamespace.GameData
{
    public class ObstacleData
    {
        public int Row;
        public int Col;
        public int Direction;
        public int Length;
        
        public ObstacleData(int row, int col, int direction, int len=1)
        {
            Row = row;
            Col = col;
            Direction = direction%6;
            Length = len;
        }
        
        public (int OtherRow, int OtherCol) Opposition(int d)
        {
            var a = (Direction+d)%6;
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
        
        public List<(int OtherRow, int OtherCol)> Oppositions()
        {
            
            var l = 1;
            var output = new List<(int OtherRow, int OtherCol)>();
            for (int i = 0; i < Length; i++)
            {
                output.Add(Opposition(i));
            }

            return output;
        }
        
        

        public bool Collides(ObstacleData other)
        {
            if (Row == other.Row && Col == other.Col)
            {
                for (int i = 0; i < Length; i++)
                {
                    for (int j = 0; j < other.Length; j++)
                    {
                        if (i + Direction == j + other.Direction)
                        {
                            return true;
                        }
                    
                    }
                }

                return false;

            }
            else
            {
                return false;
            }

        }

        
        
        
    }
}