using UnityEngine;

namespace Song.Editor.Core.Animation
{
    public class Line
    {
        public Vector2[] Straight(Vector2 origin, Vector2 target, int count)
        {
            if (count <= 0) return null;
            var points = new Vector2[count];
            var ox = origin.x;
            var oy = origin.y;
            var x = (target.x - origin.x) / count;
            var y = (target.y - origin.y) / count;
            for (int i = 0; i < count; i++)
            {
                ox += x;
                oy += y;
                points[i] = new Vector2(ox, oy);
            }
            return points;
        }
    }
}