using UnityEngine;

namespace ShootingEditor2D
{
    public class LevelEditor : MonoBehaviour
    {
        public SpriteRenderer EmptyHighlight;
        private void Update()
        {
            var mousePos = Input.mousePosition;
            if (!Camera.main) return;
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            mouseWorldPos.z = 0;
            EmptyHighlight.transform.position = mouseWorldPos;
        }
    }
}
