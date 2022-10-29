using DefaultNamespace.Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public class CapsuleScript: WorldObject
    {
        public SpriteRenderer capsuleRenderer;
        
        
        public void Paint(Color c)
        {
            capsuleRenderer.color = c;
        }
    }
}