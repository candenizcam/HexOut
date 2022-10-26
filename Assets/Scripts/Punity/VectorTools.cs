﻿using UnityEngine;

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
        
        
        
        
        
        //public static Vector3 Multiply(this Vector3 v, Vector3 other)
        //{
        //    return new Vector3(v.x * other.x, v.y * other.y, v.z * other.z);
        //}
        
        //public static Vector3 Mutate(Vector3 v, ) 
    }
    
}