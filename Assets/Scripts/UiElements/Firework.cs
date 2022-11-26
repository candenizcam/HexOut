
using System;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    
    
    public class Firework: VisualElement
    {
        private Vector2 _origin;
        private Vector2 _explosionTarget; // not relative
        private VisualElement _bullet;
        private Color _colour;

        public Firework(Vector2 origin, Vector2 explosionTarget, UnityEngine.Color colour)
        {
            _origin = origin;
            _explosionTarget = explosionTarget;
            _colour = colour;

            

            var d = explosionTarget - origin;
            var a = (float)Math.Atan2(d.x,d.y)*2f*3.141f;
            
            _bullet = new VisualElement()
            {
                style=
                {
                    width = 60f,
                    height = 24f,
                    rotate = new StyleRotate(new Rotate(a)),
                    backgroundImage = QuickAccess.LoadSpriteBg("UI/Capsule"),
                    unityBackgroundImageTintColor = new StyleColor(_colour),
                    
                }
            };
            
        }

        public void Fly(float alpha)
        {
            var p = _explosionTarget * alpha + _origin * (1f - alpha);
            _bullet.style.top = p.x;
            _bullet.style.left = p.y;
        }

        public void Explode(float alpha)
        {
            
        }

    }
}