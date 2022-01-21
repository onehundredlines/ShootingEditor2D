using System;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
namespace ShootingEditor2D
{
    public class LevelPlayer : MonoBehaviour
    {
        private enum State
        {
            Selection,
            Playing
        }
        private string mLevelFilesFolder;
        private State mCurrentState = State.Selection;
        private void Awake() { mLevelFilesFolder = $"{Application.persistentDataPath}/LevelFiles"; }
        private readonly Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 50
        });
        private void OnGUI()
        {
            if (mCurrentState == State.Selection)
            {
                var filesPath = Directory.GetFiles(mLevelFilesFolder);
                var y = 10;
                foreach(var file in filesPath.Where(file => file.EndsWith("xml")))
                {
                    var fileName = Path.GetFileName(file);
                    if (GUI.Button(new Rect(Screen.width * 0.5f - 500, y, 1000, 60), fileName, mButtonStyle.Value))
                    {
                        var xml = File.ReadAllText(file);
                        ParseAndRun(xml);
                        mCurrentState = State.Playing;
                    }
                    y += 70;
                }
            }
        }
        private void ParseAndRun(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);
            var levelRootNode = document?.SelectSingleNode("Level");
            if (levelRootNode == null) return;
            foreach(XmlElement levelItemNode in levelRootNode)
            {
                var name = levelItemNode.Attributes["name"].Value;
                float x = float.Parse(levelItemNode.Attributes["x"].Value);
                float y = float.Parse(levelItemNode.Attributes["y"].Value);

                var levelItemPrefabs = Resources.Load<GameObject>(name);
                var levelItemGameObj = Instantiate(levelItemPrefabs, transform);
                levelItemGameObj.transform.position = new Vector3(x, y, 0);
            }
        }
    }
}