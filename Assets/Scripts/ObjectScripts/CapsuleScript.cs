using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public class CapsuleScript: WorldObject
    {
        public SpriteRenderer capsuleRenderer;
        public BoxCollider2D boxCollider;
        public SpriteRenderer trackShadow;
        public Vector2 UnitVector => ThisCapsuleData.UnitDirection();
        [NonSerialized]public CapsuleData ThisCapsuleData;
        [NonSerialized] public int MovementState; // 0: not, 1: forward, 2: ready for next
        private CapsuleData _targetData;
        private Vector2 _newPosition;
        private Vector2 _oldPosition;
        private float _moveAlpha;
        private bool _direction;
        public bool MovesForward => _direction;
        
        
        public void DeactivateTrackShadow()
        {
            var t = trackShadow.transform;
            t.localScale = new Vector3(1f,1f,1f);
            t.localPosition = new Vector3(0f, 0f, 0f);
            trackShadow.enabled = false;
        }
        
        public void ActivateTrackShadow(Vector2 position, Vector3 scale)
        {
            var t = trackShadow.transform;
            t.position = new Vector3(position.x, position.y, t.position.z);
            t.localScale = scale;
            trackShadow.enabled = true;

        }

        
        public void Paint(Color c)
        {
            capsuleRenderer.color = c;
            trackShadow.color = new Color(c.r, c.g, c.b, .1f);
        }

        public bool Touching(Vector2 p)
        {
            return boxCollider.OverlapPoint(p);
        }

        public CapsuleData NewData(int r, int c)
        {
            return new CapsuleData(r,c,ThisCapsuleData.Length,ThisCapsuleData.Angle);
        }

        public CapsuleData TranslateData(int d)
        {
            var p = ThisCapsuleData.PointFromFirst(d);
            return NewData(p.LastRow,p.LastCol);

        }


        public void SetToMovement(bool forward)
        {
            _direction = forward;
            MovementState = 2;


        }

        public void StopMovement()
        {
            MovementState = 0;
        }

        public List<(int row, int col)> Occupies()
        {
            var l = ThisCapsuleData.TwoIndexTiles();
            if (_targetData is not null)
            {
                l.AddRange(_targetData.TwoIndexTiles());
            }

            return l.Distinct().ToList();
        }
        
        
        public void NewTarget(CapsuleData newData, Vector2 newPosition)
        {
            _targetData = newData;
            _newPosition = newPosition;
            _oldPosition = transform.position;
            _moveAlpha = 0f;
            MovementState = 1;
        }

        public void DoMovement()
        {
            _moveAlpha += Time.deltaTime / Constants.SprayMovementPerTile;
            

            if (_moveAlpha > 1f)
            {
                _moveAlpha = 1f;
                ThisCapsuleData = _targetData;
                _targetData = null;
                MovementState = 2;
                
            }

            transform.position = new Vector3(_newPosition.x * _moveAlpha + _oldPosition.x * (1f - _moveAlpha),
                _newPosition.y * _moveAlpha + _oldPosition.y * (1f - _moveAlpha), transform.position.z);
            //var nd = (_newPosition - _oldPosition) / Constants.SprayMovementPerTile * Time.deltaTime;

            //transform.position 

        }

    }
}