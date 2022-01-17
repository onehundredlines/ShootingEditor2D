using UnityEngine;
namespace ShootingEditor2D
{
    public static class RectHelper
    {
        public static Rect RectForAnchorCenter(float x, float y, float width, float height)
        {
            var oneHalf = 0.5f;
            var finalX = x - width * oneHalf;
            var finalY = y - height * oneHalf;
            return new Rect(finalX, finalY, width, height);
        }
        public static Rect RectForAnchorCenter(Vector2 pos, Vector2 size)
        {
            var oneHalf = 0.5f;
            var finalX = pos.x - size.x * oneHalf;
            var finalY = pos.y - size.y * oneHalf;
            return new Rect(finalX, finalY, size.x, size.y);
        }
    }
}