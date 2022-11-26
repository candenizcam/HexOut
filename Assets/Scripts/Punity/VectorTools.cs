using System;
using UnityEngine;

namespace DefaultNamespace.Punity
{
    public static class VectorTools
    {

        public static Vector3 MutateVector(this Vector3 v, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ??= v.x, y ??= v.y, z ??= v.z);
        }

        public static Vector3 Add(this Vector3 v, Vector3 other)
        {
            return new Vector3(v.x + other.x, v.y + other.y, v.z + other.z);
        }
        
        public static Vector3 Add(this Vector3 v, float f)
        {
            return new Vector3(v.x + f, v.y + f, v.z + f);
        }
        
        public static Vector3 Subtract(this Vector3 v, Vector3 other)
        {
            return new Vector3(v.x - other.x, v.y - other.y, v.z - other.z);
        }
        
        public static Vector3 Subtract(this Vector3 v, float f)
        {
            return new Vector3(v.x - f, v.y - f, v.z - f);
        }

        public static Vector3 Multiply(this Vector3 v, Vector3 other)
        {
            return new Vector3(v.x * other.x, v.y * other.y, v.z * other.z);
        }
        
        public static Vector3 Multiply(this Vector3 v, float f)
        {
            return new Vector3(v.x * f, v.y * f, v.z * f);
        }
        
        public static Vector3 Divide(this Vector3 v, Vector3 other)
        {
            return new Vector3(v.x / other.x, v.y / other.y, v.z / other.z);
        }
        
        public static Vector3 Divide(this Vector3 v, float f)
        {
            return new Vector3(v.x / f, v.y / f, v.z / f);
        }

        public static Vector3 ToVector3(this Vector2 v, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x??=v.x, y??=v.y, z??=0f);
        }

        public static float Magnitude2D(this Vector3 v)
        {
            return ((Vector2) v).magnitude;
        }

        public static float Angle(this Vector2 v)
        {
            return (float)Math.Atan2(v.y, v.x)/6.282f*360f;
        }
        
        
        //public static Vector3 Multiply(this Vector3 v, Vector3 other)
        //{
        //    return new Vector3(v.x * other.x, v.y * other.y, v.z * other.z);
        //}
        
        //public static Vector3 Mutate(Vector3 v, ) 
    }
    
}