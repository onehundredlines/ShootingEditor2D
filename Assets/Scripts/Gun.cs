using UnityEngine;
namespace ShootingEditor2D
{
    public class Gun : MonoBehaviour
    {
        [SerializeField]
        private Bullet mBullet;
        [SerializeField]
        private Transform mBulletPoint;
        private GameObject bulletObj;
        private void Awake()
        {
            mBulletPoint = transform.Find("BulletPoint").transform;
            bulletObj = Resources.Load<GameObject>("Bullet");
        }
        public void Shoot()
        {
            var bulletGO = Instantiate(bulletObj);
            if (bulletGO == null) return;
            var trans = mBulletPoint.transform;
            bulletGO.transform.localPosition = trans.position;
            bulletGO.transform.localRotation = trans.rotation;
            bulletGO.gameObject.SetActive(true);
            Destroy(bulletGO, 1f);
        }
    }
}