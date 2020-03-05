using System.Runtime.InteropServices;
using UnityEngine;

namespace UnhandledException
{
    class FastMath
    {
        #region distance resizing font/box/position - DistSizer
        public static void DistSizer(float distance, ref int FontSize, ref float deltaDistance, ref float devLabel)
        {
            if (distance >= 500f)
            {
                FontSize = 4;
                deltaDistance = 5f;
                devLabel = 5f;
            }
            else if (distance >= 400f)
            {
                FontSize = 6;
                deltaDistance = 10f;
                devLabel = 4f;
            }
            else if (distance >= 300f)
            {
                FontSize = 8;
                deltaDistance = 15f;
                devLabel = 3f;
            }
            else if (distance >= 200f)
            {
                FontSize = 10;
                deltaDistance = 20f;
                devLabel = 2f;
            }
            else if (distance >= 100f)
            {
                FontSize = 12;
                deltaDistance = 25f;
            }
            else
            {
                FontSize = 13;
                deltaDistance = 30f;
            }
        }
        #endregion
        #region FastDistance Vector3 - FD
        public static float FD(Vector3 c1, Vector3 c2) { float cx, cy, cz, n; cx = c2.x - c1.x; cy = c2.y - c1.y; cz = c2.z - c1.z; n = (cx * cx + cy * cy + cz * cz); return FSqrt(n); }
        #endregion
        #region FastDistance Vector2 - FDv2
        public static float FDv2(Vector2 c1, Vector2 c2) { float cx, cy, n; cx = c2.x - c1.x; cy = c2.y - c1.y; n = (cx * cx + cy * cy); return FSqrt(n); }
        #endregion
        #region Fast SQRT calculations - FSqrt
        [StructLayout(LayoutKind.Explicit)]
        private struct FloatIntUnion {[FieldOffset(0)] public float f;[FieldOffset(0)] public int tmp; }
        private static float FSqrt(float z)
        { if (z == 0) { return 0; } FloatIntUnion c; c.tmp = 0; c.f = z; c.tmp -= 1 << 23; c.tmp >>= 1; c.tmp += 1 << 29; return c.f; }
        #endregion
    }
}
