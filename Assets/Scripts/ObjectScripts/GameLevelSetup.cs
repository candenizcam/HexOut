﻿using System;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DefaultNamespace
{
    public partial class GameLevelScript : WorldObject
    {
        public void SetGrid(Camera c, int row, int col, ObstacleData[] obstacles)
        {
            _row = row;
            _col = col;
            _grid = HexGridScript.Instantiate(gameObject.transform);
            _grid.InitializeGrid(_row,_col, obstacles);
            _obstacles = obstacles;
            
            SetSize(new Vector2(0f,0f), 
                c.orthographicSize*c.aspect*2f,
                c.orthographicSize*2f);
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
                var c = Constants.CapsuleColours.OrderBy(x => r.NextDouble()).First();
                SetCapsule(capsuleData,c);
            }
        }

        
        

        public void SetCapsule(CapsuleData capsuleData, Color? col = null)
        {
            var res = Resources.Load<GameObject>(capsuleData.Path());
            var q = Instantiate( res,(Transform) gameObject.transform);
            var cs = q.GetComponent<CapsuleScript>();
            var v3 = CapsulePosition(capsuleData);

            q.transform.position = new Vector3(v3.x, v3.y, -2f);
            q.transform.rotation = Quaternion.Euler(0f,0f,capsuleData.Degrees());
            q.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
            cs.Paint(col ??= Color.white);
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