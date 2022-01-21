using System;
using UnityEngine;

namespace ShootingEditor2D
{
    public class LevelEditor : MonoBehaviour
    {
        public SpriteRenderer EmptyHighlight;
        //当前高亮块坐标
        private float mCurrentHighlightPosX;
        private float mCurrentHighlightPosY;
        //是否可绘制
        private bool mCanDraw;
        private void Update()
        {
            var mousePos = Input.mousePosition;
            if (!Camera.main) return;
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            //Mathf.Floor()取整，加上0.5f，意思是四舍五入
            mouseWorldPos.x = Mathf.Floor(mouseWorldPos.x + 0.5f);
            mouseWorldPos.y = Mathf.Floor(mouseWorldPos.y + 0.5f);
            mouseWorldPos.z = 0;
            //todo 这点没想明白，先接着学看看，回来再解决。
            //与当前高亮块的x、y值
            if (Math.Abs(mCurrentHighlightPosX - mouseWorldPos.x) < 0.4f && Math.Abs(mCurrentHighlightPosY - mouseWorldPos.y) < 0.4f)
            {
                //不做任何事
            } else
            {
                //调整高亮块儿位置
                var highlightPos = mouseWorldPos;
                highlightPos.z = -6;
                EmptyHighlight.transform.position = highlightPos;
                //发出射线
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                var hit =  Physics2D.Raycast(ray.origin, Vector2.zero, 20);
                //有碰撞说明有地块
                if (hit.collider)
                {
                    EmptyHighlight.color=Color.red;
                    mCanDraw = false;
                } else
                {
                    EmptyHighlight.color=Color.white;
                    mCanDraw = true; 
                }
            }
            if (Input.GetMouseButtonDown(0) && mCanDraw)
            {
                var groundPrefab = Resources.Load<GameObject>("Ground");
                var groundGameObject = Instantiate(groundPrefab, mouseWorldPos, Quaternion.identity, transform);
                groundGameObject.name = "Ground";
            }
        }
    }
}