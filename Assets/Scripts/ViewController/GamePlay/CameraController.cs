using UnityEngine;
namespace ShootingEditor2D
{
    public class CameraController : MonoBehaviour
    {
        private Transform mPlayerTrans;
        private void Update()
        {
            if (!mPlayerTrans)
            {
                var playerGameObj = GameObject.FindWithTag("Player").transform;
                if (playerGameObj)
                {
                    mPlayerTrans = playerGameObj.transform;
                } else
                {
                    return;
                }
            }
            var playerPos = mPlayerTrans.position;
            var cameraPos = transform.position;
            cameraPos.x = playerPos.x + 2;
            // cameraPos.y = playerPos.y + 2;
            transform.position = cameraPos;
        }
    }
}