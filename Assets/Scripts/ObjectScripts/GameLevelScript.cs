using System;
using System.Collections.Generic;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameLevelScript: WorldObject
    {
        private HexGridScript _grid;
        private List<CapsuleScript> _capsules = new();
        private float _smallScale = 1f;
        public float SmallScale => _smallScale;
        private Vector2 _gridCentre = new Vector2();
        private int _col = 5;
        private int _row = 5;
        
        public void SetGrid(Camera c)
        {
            _grid = HexGridScript.Instantiate(gameObject.transform);
            _grid.InitializeGrid(5,5);
            
            SetSize(new Vector2(0f,0f), 
                c.orthographicSize*c.aspect*2f,
                c.orthographicSize*2f);
            //transform.localScale = new Vector3(_grid.SmallScale, _grid.SmallScale,1f);
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
            //gameObject.transform.localScale = v;
            transform.position = new Vector3(centre.x, centre.y, -1f);


        }
        
        

        public void SetCapsules(CapsuleData[] capsuleDataList)
        {
            var r = new System.Random();
            
            
            foreach (var capsuleData in capsuleDataList)
            {
                var res = Resources.Load<GameObject>(capsuleData.Path());
                var q = Instantiate(res,gameObject.transform);

                var lp = capsuleData.LastPoint();
                var v1 = _grid.TwoCoordsToWorld(capsuleData.FirstRow,capsuleData.FirstCol,_smallScale);
                var v2 = _grid.TwoCoordsToWorld(lp.LastRow,lp.LastCol,_smallScale);

                var v3 = v1 * 0.5f + v2 * 0.5f;
                q.transform.position = new Vector3(v3.x, v3.y, -2f);
                q.transform.rotation = Quaternion.Euler(0f,0f,capsuleData.Degrees());
                var cs = q.GetComponent<CapsuleScript>();
                cs.Paint(new Color((float)r.NextDouble(),(float)r.NextDouble(),(float)r.NextDouble()));
                _capsules.Add(cs);
            }
            
            
            
        }
        
        
        
        
        
        public static GameLevelScript Instantiate()
        {
            var a = new GameObject("game level");
            var n = a.AddComponent<GameLevelScript>();
            return n;



        }
        
    }
}