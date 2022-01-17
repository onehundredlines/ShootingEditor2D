using UnityEngine;
using Vector3 = UnityEngine.Vector3;
namespace ShootingEditor2D
{
    public class CameraController : MonoBehaviour
    {
        private Transform mPlayerTrans;

        private float xMin = -5;
        private float xMax = 5;
        private float yMin = -5;
        private float yMax = 5;
        private Vector3 mTargetPos;
        private void LateUpdate()
        {
            if (!mPlayerTrans)
            {
                var playerGameObj = GameObject.FindWithTag("Player").transform;
                if (playerGameObj) mPlayerTrans = playerGameObj.transform;
                else return;
            }
            var playerPos = mPlayerTrans.position;
            // var isRight = Mathf.Sign(mPlayerTrans.transform.localScale.x);
            // mTargetPos.x = playerPos.x + 3 * isRight;
            mTargetPos.x = playerPos.x;
            mTargetPos.y = playerPos.y + 1;
            mTargetPos.z = -10;
            var smoothSpeed = 5;
            //增加平滑处理
            var cameraPos = transform.position;
            cameraPos = Vector3.Lerp(cameraPos, new Vector3(mTargetPos.x, mTargetPos.y, cameraPos.z), smoothSpeed * Time.deltaTime);
            //锁定在一个固定区域
            transform.position = new Vector3(Mathf.Clamp(cameraPos.x, xMin, xMax), Mathf.Clamp(cameraPos.y, yMin, yMax), cameraPos.z);
        }
    }
}