using System;
using UnityEngine;

namespace Background
{
    
    public static class BackgroundHelper
    {
        public readonly static Vector2 HEXA_FACTOR = new Vector2(0.75f, 0.875f);

        public static Vector3 GetPos(uint row, uint col, float size=1f, float zindex=2f)
        {
            float yfactor = row * HEXA_FACTOR.y;
            float xfactor = col * HEXA_FACTOR.x;
            if (col % 2 == 1)
                yfactor += HEXA_FACTOR.y / 2f;

            float y = -yfactor;
            float x = xfactor;
            return new Vector3(x*size, y*size, zindex);
        }
    }
}
