using System.Xml;
using UnityEngine;
namespace ShootingEditor2D
{
    public class LevelPlayer : MonoBehaviour
    {
        public TextAsset LevelFile;
        private void Start()
        {
            if (!LevelFile) return;
            var xml = LevelFile.text;
            var document = new XmlDocument();
            document.LoadXml(xml);
            var levelRootNode = document.SelectSingleNode("Level");
            foreach(XmlElement levelItemNode in levelRootNode)
            {
                var name = levelItemNode.Attributes["name"].Value;
                var x = int.Parse(levelItemNode.Attributes["x"].Value);
                var y = int.Parse(levelItemNode.Attributes["y"].Value);

                var levelItemPrefabs = Resources.Load<GameObject>(name);
                var levelItemGameObj = Instantiate(levelItemPrefabs, transform);
                levelItemGameObj.transform.position = new Vector3(x, y, 0);
                Debug.Log($"name {name}, x {x}, y {y}");
            }



        }
    }
}