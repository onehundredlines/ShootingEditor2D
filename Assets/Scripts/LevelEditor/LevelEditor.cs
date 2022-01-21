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
            //Mathf.Floor()取整，加上0.5f，意思是四舍五入
            mouseWorldPos.x = Mathf.Floor(mouseWorldPos.x + 0.5f);
            mouseWorldPos.y = Mathf.Floor(mouseWorldPos.y + 0.5f);
            mouseWorldPos.z = 0;
            EmptyHighlight.transform.position = mouseWorldPos;
            if (Input.GetMouseButtonDown(0))
            {
                var groundPrefab = Resources.Load<GameObject>("Ground");
                var groundGameObject = Instantiate(groundPrefab, mouseWorldPos, Quaternion.identity, transform);
                groundGameObject.name = "Ground";
            }
        }
    }
}
