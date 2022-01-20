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
            var labelRect = RectHelper.RectForAnchorCenter(Screen.width * oneHalf, Screen.height * oneHalf - 150, 1200, 400);
            GUI.Label(labelRect, "Shoot Editor 2D", mLabelStyle.Value);
            var buttonRect = RectHelper.RectForAnchorCenter(Screen.width * oneHalf, Screen.height * oneHalf, 300, 100);
            if (GUI.Button(buttonRect, "开始游戏", mButtonStyle.Value)) SceneManager.LoadScene("SampleScene");
        }
    }
}