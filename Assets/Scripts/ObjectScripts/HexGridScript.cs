using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public class HexGridScript: WorldObject
    {
        private readonly string _hexPath = "prefabs/HexTile";
        private int _row = 1;
        private int _col = 1;
        
        private float _tileBaseSideLength;
        private List<HexTileScript> _tileScripts = new();

        protected override void AwakeFunction()
        {
            
            
        }




        /** Starts from top left, returns world coordinates
         * 
         */
        public Vector2 TwoCoordsToWorld(int r, int c, float smallScale)
        {
             //_tileScripts.fi


             var dw = (c - 1f + 0.5f*((r+1)%2)) * HexTileScript.Width *  smallScale;
             var dh = (r - 1) * HexTileScript.Height* .375f * smallScale;
             
             
             var topOffset = (_row-1)/2 *  HexTileScript.Height* .375f * smallScale;
             var leftOffset = -((_col - 1) / 2) * HexTileScript.Width * smallScale;

             //var  p =_tileScripts[c - 1 + (r - 1) * _row + r / 2];


             return new Vector2(leftOffset+dw,topOffset-dh);

             //return p.transform.position;
        }
        
        


        
        

        public void InitializeGrid(int row, int col, ObstacleData[] obstacles)
        {
            foreach (var hexTileScript in _tileScripts)
            {
                Destroy(hexTileScript);
            }
            _tileScripts.Clear();
            
            var res = Resources.Load<GameObject>(_hexPath);

            _tileBaseSideLength = HexTileScript.SideLength; 
            
            var h = HexTileScript.Height;
            var w = HexTileScript.Width;
            _row = row;
            _col = col;


            var diffY = h * .375f;
            var diffX = w;

            for (int i = _row-1; i >= 0; i--)
            {
                var valY = (i-(_row-1)*.5f) * diffY;
                var offset = (i % 2) * w *.5f;
                for (int j = 0; j < _col + i%2; j++)
                {
                    var valX = (j-(_col-1)*.5f) * diffX;

                    var q = Instantiate(res,this.gameObject.transform);
                    q.transform.position = new Vector3(-offset+valX, valY, 0f);
                    var hts = q.GetComponent<HexTileScript>();
                    hts.R = _row - (i );
                    hts.C = j + 1 - i%2;
                    hts.ActivateObstacles(obstacles
                        .Where(x => hts.R == x.Row && hts.C == x.Col)
                        .Select(x=> x.Direction)
                        .ToArray());

                    
                    _tileScripts.Add(hts);
                    //hts.Paint(i%2==0 ? new Color(.7f,.6f,.6f) : new Color(.6f,.6f,.7f));
                    

                }
            }
            
            
            
        } 

        
        


        public static HexGridScript Instantiate(Transform parent = null)
        {
            var a = new GameObject("hex grid");
            if (parent is not null)
            {
                a.transform.parent = (Transform)parent;
            }
            
            var n = a.AddComponent<HexGridScript>();
            return n;



        }
    }
}