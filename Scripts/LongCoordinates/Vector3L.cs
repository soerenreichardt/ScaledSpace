using System;
using UnityEngine;

namespace LongCoordinates
{
    [Serializable]
    public class Vector3L
    {
        public long x;
        public long y;
        public long z;

        public Vector3L(long x, long y, long z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3L Divide(long divisor)
        {
            this.x /= divisor;
            this.y /= divisor;
            this.z /= divisor;

            return this;
        }

        public Vector3L Multiply(long factor)
        {
            this.x *= factor;
            this.y *= factor;
            this.z *= factor;

            return this;
        }

        public Vector3L Add(Vector3L other)
        {
            this.x += other.x;
            this.y += other.y;
            this.z += other.z;

            return this;
        }
    
        public Vector3L Add(Vector3 other)
        {
            this.x += (long) other.x;
            this.y += (long) other.y;
            this.z += (long) other.z;

            return this;
        }
    
        public Vector3 ToFloat()
        {
            return new Vector3(x, y, z);
        }
    
        public static Vector3L operator -(Vector3L lhs, Vector3L rhs)
        {
            return new Vector3L(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        public void Add(long x, long y, long z)
        {
            this.x += x;
            this.y += y;
            this.z += z;
        }
    }
}