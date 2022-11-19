using System.Collections.Generic;

namespace DefaultNamespace.GameData
{
    public class ObstacleData
    {
        public int Row;
        public int Col;
        public int Direction;
        public int Length;
        private List<(int r, int c)> _oppositions;
        
        public ObstacleData(int row, int col, int direction, int len=1)
        {
            Row = row;
            Col = col;
            Direction = direction%6;
            Length = len;
            _oppositions = GenerateOppositions();
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

        private List<(int OtherRow, int OtherCol)> GenerateOppositions()
        {
            var l = 1;
            var output = new List<(int OtherRow, int OtherCol)>();
            for (int i = 0; i < Length; i++)
            {
                output.Add(Opposition(i));
            }

            return output;
        }
        
        public List<(int OtherRow, int OtherCol)> Oppositions()
        {            
            return _oppositions;
        }
        
        

        public bool Collides(ObstacleData other)
        {
            if (Row == other.Row && Col == other.Col)
            {
                for (int i = 0; i < Length; i++)
                {
                    for (int j = 0; j < other.Length; j++)
                    {
                        if ((i + Direction)%6 == (j + other.Direction)%6)
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

        public string InfoText()
        {
            return $"r: {Row},c: {Col},l: {Length},a: {Direction}";
        }

        
        
        
    }
}