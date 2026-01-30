// Copyright (c) 2023 NicoIer and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using MemoryPack;

namespace UnityToolkit.MathTypes
{
    [MemoryPackable]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Vector4 : IEquatable<Vector4>
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public static readonly Vector4 zero = new Vector4(0f, 0f, 0f, 0f);
        public static readonly Vector4 one = new Vector4(1f, 1f, 1f, 1f);

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        
        public static implicit operator Vector4((float x, float y, float z, float w) tuple)
        {
            return new Vector4(tuple.x, tuple.y, tuple.z, tuple.w);
        }
        
        
        
        
        

        public override string ToString()
        {
            return $"({x}, {y}, {z}, {w})";
        }


        public bool Equals(Vector4 other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector4 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                hashCode = (hashCode * 397) ^ w.GetHashCode();
                return hashCode;
            }
        }
    }
}