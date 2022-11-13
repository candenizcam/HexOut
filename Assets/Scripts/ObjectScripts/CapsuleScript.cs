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

        private float _initSpriteX;
        private float _initSpriteY;

        protected override void AwakeFunction()
        {
            _initSpriteX = capsuleRenderer.size.x;
            _initSpriteY = capsuleRenderer.size.y;
        }


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
            return new CapsuleData(r,c,ThisCapsuleData.Length,ThisCapsuleData.Angle,false);
        }

        public CapsuleData TranslateData(int d)
        {
            var p = ThisCapsuleData.PointFromFirst(d);
            return NewData(p.LastRow,p.LastCol);
        }


        public void SetToMovement(bool forward)
        {
            if (!forward)
            {
                ThisCapsuleData = ThisCapsuleData.Revert();
            }
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


        public void DoThings()
        {
            if (_targetData.Collapsed && !ThisCapsuleData.Collapsed)
            {
                DoCollapse(true);
            }
            else if (!_targetData.Collapsed && ThisCapsuleData.Collapsed)
            {
                DoCollapse(false);
            }
            else if (_targetData.Collapsed && ThisCapsuleData.Collapsed)
            {
                DoCollapsedBounce();
            }
            else 
            {
                DoMovement();
            }
        }
        
        public void DoMovement()
        {
            _moveAlpha += Time.deltaTime / Constants.SprayMovementPerTile;
            

            if (_moveAlpha > 1f)
            {
                _moveAlpha = 1f;
                ThisCapsuleData = _targetData;
                gameObject.transform.rotation =  Quaternion.Euler(0f,0f,_targetData.Degrees());
                _targetData = null;
                MovementState = 2;
                
            }

            transform.position = new Vector3(_newPosition.x * _moveAlpha + _oldPosition.x * (1f - _moveAlpha),
                _newPosition.y * _moveAlpha + _oldPosition.y * (1f - _moveAlpha), transform.position.z);
        }

        public void DoCollapse(bool collapsing)
        {
            _moveAlpha += 2f*Time.deltaTime / Constants.SprayMovementPerTile;
            if (_moveAlpha > 1f)
            {
                _moveAlpha = 1f;
                ThisCapsuleData = _targetData;
                gameObject.transform.rotation =  Quaternion.Euler(0f,0f,_targetData.Degrees());
                _targetData = null;
                MovementState = 2;
            }
            

            transform.position = new Vector3(_newPosition.x * _moveAlpha + _oldPosition.x * (1f - _moveAlpha),
                _newPosition.y * _moveAlpha + _oldPosition.y * (1f - _moveAlpha), transform.position.z);

            var sizeX = collapsing
                ? 0.9f * _moveAlpha + _initSpriteX * (1f - _moveAlpha)
                : 0.9f * (1f - _moveAlpha) + _initSpriteX * (_moveAlpha); 
            
            capsuleRenderer.size = new Vector3(sizeX,_initSpriteY);
        }

        public void DoCollapsedBounce()
        {
            _moveAlpha += 2f*Time.deltaTime / Constants.SprayMovementPerTile;
            if (_moveAlpha > .5f)
            {
                gameObject.transform.rotation =  Quaternion.Euler(0f,0f,_targetData.Degrees());
            }
            else
            {
                gameObject.transform.rotation =  Quaternion.Euler(0f,0f,ThisCapsuleData.Degrees());
            }

            if (_moveAlpha > 1f)
            {
                _moveAlpha = 1f;
                ThisCapsuleData = _targetData;
                gameObject.transform.rotation =  Quaternion.Euler(0f,0f,_targetData.Degrees());
                _targetData = null;
                MovementState = 2;
            }

            var curveAlpha = 2f * _moveAlpha * (1f - _moveAlpha);
            var sizeX = curveAlpha* (_initSpriteY - 0.9f) + 0.9f;
            capsuleRenderer.size = new Vector3(sizeX,_initSpriteY+curveAlpha*0.2f);
        }

    }
}