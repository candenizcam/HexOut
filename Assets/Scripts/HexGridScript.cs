using System;
using System.Collections.Generic;
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


        /** This function sets the size to a given field in world space
         * importantly, you give this the limits, and it fits the tiles accordingly
         */
        public void SetSize(Vector2 centre, float width, float height)
        {
            var baseWidth = HexTileScript.Width * _col;
            var baseHeight = HexTileScript.Height * .375f * (_row-1) + HexTileScript.SideLength*.5f;

            

            var widthScale = width / baseWidth;
            var heightScale = height / baseHeight;
            var smallScale = Math.Min(widthScale, heightScale);
            
            var v = new Vector3(smallScale, smallScale,1f);

            gameObject.transform.localScale = v;
            gameObject.transform.position = new Vector3(centre.x, centre.y, -1f);


        }
        

        public void InitializeGrid(int row, int col)
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

            for (int i = 0; i < _row; i++)
            {
                var valY = (i-(_row-1)*.5f) * diffY;
                var offset = (i % 2) * w *.5f;
                for (int j = 0; j < _col + i%2; j++)
                {
                    var valX = (j-(_col-1)*.5f) * diffX;

                    var q = Instantiate(res,this.gameObject.transform);
                    q.transform.position = new Vector3(-offset+valX, valY, 0f);
                    var hts = q.GetComponent<HexTileScript>();
                    _tileScripts.Add(hts);
                    hts.Paint(i%2==0 ? new Color(.3f,.2f,.2f) : new Color(.2f,.2f,.3f));
                    

                }
            }
            
            
            
        } 

        
        


        public static HexGridScript Instantiate()
        {
            var a = new GameObject("hex grid");
            var n = a.AddComponent<HexGridScript>();
            return n;



        }
    }
}