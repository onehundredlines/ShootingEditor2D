using System;
using UnityEngine;

namespace ShootingEditor2D
{

    public class LevelEditor : MonoBehaviour
    {
        private enum OperateMode
        {
            Draw,
            Erase
        }
        private OperateMode mCurrentOperateMode;
        private readonly Lazy<GUIStyle> mModeLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 50,
            alignment = TextAnchor.MiddleCenter
        });
        private readonly Lazy<GUIStyle> mButtonLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 50
        });
        private void OnGUI()
        {
            var modeLabelRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, 35, 300, 50);
            GUI.Label(modeLabelRect, mCurrentOperateMode.ToString(), mModeLabelStyle.Value);

            var drawLabelRect = new Rect(10, 10, 160, 80);
            if (GUI.Button(drawLabelRect, "Draw", mButtonLabelStyle.Value)) mCurrentOperateMode = OperateMode.Draw;
            var eraseLabelRect = new Rect(10, 100, 160, 80);
            if (GUI.Button(eraseLabelRect, "Erase", mButtonLabelStyle.Value)) mCurrentOperateMode = OperateMode.Erase;
        }
        public SpriteRenderer EmptyHighlight;
        //当前高亮块坐标
        private float mCurrentHighlightPosX;
        private float mCurrentHighlightPosY;
        //是否可绘制
        private bool mCanDraw;
        private GameObject mCurrentObjectMouseOn;
        private void Update()
        {
            var mousePos = Input.mousePosition;
            if (!Camera.main) return;
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            //Mathf.Floor()取整，加上0.5f，意思是四舍五入
            mouseWorldPos.x = Mathf.Floor(mouseWorldPos.x + 0.5f);
            mouseWorldPos.y = Mathf.Floor(mouseWorldPos.y + 0.5f);
            mouseWorldPos.z = 0;
            if (GUIUtility.hotControl == 0) EmptyHighlight.gameObject.SetActive(true);
            else EmptyHighlight.gameObject.SetActive(false);
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
                var hit = Physics2D.Raycast(ray.origin, Vector2.zero, 20);
                //有碰撞说明有地块
                if (hit.collider)
                {
                    if (mCurrentOperateMode == OperateMode.Draw) EmptyHighlight.color = Color.red;
                    else if (mCurrentOperateMode == OperateMode.Erase) EmptyHighlight.color = new Color(1, 0.5f, 1);
                    mCanDraw = false;
                    mCurrentObjectMouseOn = hit.collider.gameObject;
                } else
                {
                    if (mCurrentOperateMode == OperateMode.Draw) EmptyHighlight.color = Color.white;
                    else if (mCurrentOperateMode == OperateMode.Erase) EmptyHighlight.color = Color.blue;
                    mCanDraw = true;
                    mCurrentObjectMouseOn = null;
                }
            }
            if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
            {
                if (mCanDraw && mCurrentOperateMode == OperateMode.Draw)
                {
                    var groundPrefab = Resources.Load<GameObject>("Ground");
                    var groundGameObject = Instantiate(groundPrefab, mouseWorldPos, Quaternion.identity, transform);
                    groundGameObject.name = "Ground";
                } else if (!mCanDraw && mCurrentObjectMouseOn && mCurrentOperateMode == OperateMode.Erase) Destroy(mCurrentObjectMouseOn);

            }
        }
    }
}