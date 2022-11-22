using System;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace DefaultNamespace
{
    public partial class GameLevelScript : WorldObject
    {
        public GameFieldFrame FieldFrame;

        public int LevelDiff => ThisLevelData.Difficulty;

        public LevelData ThisLevelData;
        //public float UiFieldHeight;
        //public float UiFieldWidth;
        public void SetGameLevelInfo( LevelData ld)
        {
            ThisLevelData = ld;
            _levelCompleteData.LevelId = ld.Name;
        }
        
        public void SetGrid(Camera c, int row, int col, ObstacleData[] obstacles)
        {
            _row = row;
            _col = col;
            _grid = HexGridScript.Instantiate(gameObject.transform);
            _grid.InitializeGrid(_row,_col, obstacles);
            _obstacles = obstacles;


            //var frame = Constants.Frame;
            //var inner = Constants.FrameInner;

            var newY = 2f*c.orthographicSize/Constants.UiHeight*(Constants.UiHeight - Constants.GameFieldTop - Constants.GameFieldBottom);
            var newX = (2f * c.orthographicSize * c.aspect) / Constants.UiWidth * (Constants.UiWidth - Constants.GameFieldSide*2f);
            var centreY = (Constants.GameFieldBottom - Constants.GameFieldTop) * 0.5f;
            
            
            SetSize(new Vector2(0f,centreY), 
                newX,
                newY);
            
            
            
            var uiFieldWidth = (HexTileScript.Width * _col)*_smallScale/(2f * c.orthographicSize * c.aspect) *Constants.UiWidth;
            var uiFieldHeight = (HexTileScript.Height * .375f * (_row-1) + HexTileScript.SideLength*.5f)*_smallScale/(2f * c.orthographicSize ) *Constants.UiHeight;
            var uiFieldX = (Constants.UiWidth - uiFieldWidth) / 2f;
            var uiFieldY = (Constants.UiHeight - uiFieldHeight) / 2f- centreY;
            FieldFrame = new GameFieldFrame(uiFieldX, uiFieldY, uiFieldWidth, uiFieldHeight);

        }

        
        
        /** This function sets the size to a given field in world space
         * importantly, you give this the limits, and it fits the tiles accordingly
         */
        public void SetSize(Vector2 centre, float width, float height)
        {
            var baseWidth = HexTileScript.Width * _col;
            var baseHeight = HexTileScript.Height * .375f * (_row-1) + HexTileScript.SideLength*.5f;
            var widthScale = width / baseWidth;
            var heightScale = height / baseHeight;
            _smallScale = Math.Min(widthScale, heightScale);
            _gridCentre = centre;
            
            var v = new Vector3(_smallScale, _smallScale,1f);
            transform.localScale = v;
            transform.position = new Vector3(centre.x, centre.y, -1f);
        }
        
        

        public void SetCapsules(CapsuleData[] capsuleDataList)
        {
            var r = new System.Random();
            foreach (var capsuleData in capsuleDataList)
            {
                var colorIndex = r.Next(0, Constants.CapsuleColours.Length);
                
                SetCapsule(capsuleData,colorIndex);
            }
        }

        
        

        public void SetCapsule(CapsuleData capsuleData, int? col = null)
        {
            var res = Resources.Load<GameObject>(capsuleData.Path());
            var q = Instantiate( res,(Transform) gameObject.transform);
            var cs = q.GetComponent<CapsuleScript>();
            var v3 = CapsulePosition(capsuleData);

            q.transform.position = new Vector3(v3.x, v3.y, -2f);
            q.transform.rotation = Quaternion.Euler(0f,0f,capsuleData.Degrees());
            q.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
            if (col is null)
            {
                cs.Paint(Color.white,Color.gray);
            }
            else
            {
                cs.Paint(Constants.CapsuleColours[(int)col],Constants.CapsuleDarkColours[(int)col]);
            }
            cs.ThisCapsuleData = capsuleData;
            _capsules.Add(cs);
        }


        public void StartAnimation(float alpha)
        {
            
            var capsuleSize = Easing.OutBack(alpha);
            foreach (var capsuleScript in _capsules)
            {
                capsuleScript.transform.localScale = new Vector3(capsuleSize, capsuleSize, 1f);
            }
        }
    }
}