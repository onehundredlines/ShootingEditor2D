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
        private enum BrushType
        {
            Ground,
            Player
        }
        private OperateMode mCurrentOperateMode;
        private BrushType mCurrentBrushType = BrushType.Ground;
        private readonly Lazy<GUIStyle> mModeLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 50,
            alignment = TextAnchor.MiddleCenter
        });
        private readonly Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 50
        });
        private readonly Lazy<GUIStyle> mRightStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 35
        });
        private void OnGUI()
        {
            var modeLabelRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, 35, 400, 50);
            //显示绘制模式
            if (mCurrentOperateMode == OperateMode.Draw) GUI.Label(modeLabelRect, $"{mCurrentOperateMode}: {mCurrentBrushType}", mModeLabelStyle.Value);
            else GUI.Label(modeLabelRect, mCurrentOperateMode.ToString(), mModeLabelStyle.Value);

            //绘制按钮
            var drawLabelRect = new Rect(10, 10, 160, 80);
            if (GUI.Button(drawLabelRect, "Draw", mButtonStyle.Value)) mCurrentOperateMode = OperateMode.Draw;
            //橡皮擦按钮
            var eraseLabelRect = new Rect(10, 100, 160, 80);
            if (GUI.Button(eraseLabelRect, "Erase", mButtonStyle.Value)) mCurrentOperateMode = OperateMode.Erase;
            if (mCurrentOperateMode == OperateMode.Draw)
            {
                //地块按钮
                var groundButtonRect = new Rect(190, 25, 150, 50);
                if (GUI.Button(groundButtonRect, "Ground", mRightStyle.Value)) mCurrentBrushType = BrushType.Ground;
                //玩家按钮
                var playerButtonRect = new Rect(360, 25, 150, 50);
                if (GUI.Button(playerButtonRect, "Player", mRightStyle.Value)) mCurrentBrushType = BrushType.Player;
            }

        }
        public SpriteRenderer EmptyHighlight;
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
            //当处于GUI的Button上关闭高亮块
            EmptyHighlight.gameObject.SetActive(GUIUtility.hotControl == 0);
            //与当前高亮块的x、y值
            if (Math.Abs(EmptyHighlight.transform.position.x - mouseWorldPos.x) < 0.4f && Math.Abs(EmptyHighlight.transform.position.y - mouseWorldPos.y) < 0.4f)
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
            ///按下鼠标左键和不在GUI的Label上时
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && GUIUtility.hotControl == 0)
            {
                if (mCanDraw && !mCurrentObjectMouseOn && mCurrentOperateMode == OperateMode.Draw)
                {
                    if (mCurrentBrushType == BrushType.Ground)
                    {
                        var groundPrefab = Resources.Load<GameObject>("Ground");
                        var groundGameObject = Instantiate(groundPrefab, mouseWorldPos, Quaternion.identity, transform);
                        groundGameObject.name = "Ground";
                    } else if (mCurrentBrushType == BrushType.Player)
                    {
                        var playerPrefab = Resources.Load<GameObject>("Player");
                        var playerGameObject = Instantiate(playerPrefab, mouseWorldPos, Quaternion.identity, transform);
                        playerGameObject.name = "Ground";
                    }
                    mCanDraw = false;
                } else if (!mCanDraw && mCurrentObjectMouseOn && mCurrentOperateMode == OperateMode.Erase)
                {
                    Destroy(mCurrentObjectMouseOn);
                    mCurrentObjectMouseOn = null;
                }
            }
        }
    }
}