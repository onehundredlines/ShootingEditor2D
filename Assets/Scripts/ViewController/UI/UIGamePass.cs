using System;
using FrameworkDesign;
using UnityEngine;
namespace ShootingEditor2D
{
    public class UIGamePass : MonoBehaviour, IController
    {
        private readonly Lazy<GUIStyle> mLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 150,
            alignment = TextAnchor.MiddleCenter,
        });
        private readonly Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter,
        });
        private void OnGUI()
        {
            var labelWidth = 1000;
            var labelHeight = 200;
            var labelPos = new Vector2(Screen.width - labelWidth, Screen.height - labelHeight) * 0.5f;
            var labelSize = new Vector2(labelWidth, labelHeight);
            var labelRect = new Rect(labelPos, labelSize);
            GUI.Label(labelRect, "游戏通关", mLabelStyle.Value);
            var buttonWidth = 300;
            var buttonHeight = 100;
            var buttonPos = new Vector2(Screen.width, Screen.height) * 0.5f - new Vector2(buttonWidth * 0.5f, buttonHeight * 0.5f) + new Vector2(0, 150);
            var buttonSize = new Vector2(buttonWidth, buttonHeight);
            var buttonRect = new Rect(buttonPos, buttonSize);
            if (GUI.Button(buttonRect, "回到首页", mButtonStyle.Value))
            {
                Debug.Log("返回首页");
            }

        }
        public IArchitecture GetArchitecture() => ShootingEditor2D.Interface;
    }
}