using UnityEngine.UIElements.Experimental;

namespace Punity
{
    public static class AnimationFunctions
    {
        public static float EaseIn(float alpha)
        {
            
            
            return Easing.InCubic(alpha);
        }
        
        public static float EaseOut(float alpha)
        {
            return Easing.OutCubic(alpha);
        }
        
        public static float EaseInOut(float alpha)
        {
            return Easing.InOutCubic(alpha);
        }

        //public static float Overshoot()
        //{
        //    Easing.
        //}
        
        
    }
}