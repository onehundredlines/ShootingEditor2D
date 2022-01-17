using System;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ShootingEditor2D
{
    public class UIGameStart : MonoBehaviour
    {
        private readonly Lazy<GUIStyle> mLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 150,
            alignment = TextAnchor.MiddleCenter
        });
        private readonly Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter
        });
        private const float oneHalf = 0.5f;
        private void OnGUI()
        {
            var labelWidth = 1200;
            var labelHeight = 400;
            var labelSize = new Vector2(labelWidth, labelHeight);
            var labelPos = new Vector2(Screen.width, Screen.height) * oneHalf - labelSize * oneHalf - Vector2.up * 150;
            var labelRect = new Rect(labelPos, labelSize);
            GUI.Label(labelRect, "Shoot Editor 2D", mLabelStyle.Value);
            var buttonWidth = 300;
            var buttonHeight = 100;
            var buttonSize = new Vector2(buttonWidth, buttonHeight);
            var buttonPos = new Vector2(Screen.width, Screen.height) * oneHalf - buttonSize * oneHalf + Vector2.up * 150;
            var buttonRect = new Rect(buttonPos, buttonSize);
            if (GUI.Button(buttonRect, "开始游戏", mButtonStyle.Value)) SceneManager.LoadScene("SampleScene");
        }
    }
}