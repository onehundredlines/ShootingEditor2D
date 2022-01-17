using UnityEngine;
namespace ShootingEditor2D
{
    public class Gun : MonoBehaviour
    {
        [SerializeField]
        private Bullet mBullet;
        [SerializeField]
        private Transform mBulletPoint;
        private void Awake()
        {
            mBulletPoint = transform.Find("BulletPoint").transform;
            mBullet = Resources.Load<Bullet>("Bullet");
        }
        public void Shoot()
        {
            var bullet = Instantiate(mBullet,mBulletPoint.position,  Quaternion.identity);
            if (bullet == null) return;
            var trans = mBulletPoint.transform;
            var bulletTrans = bullet.transform;
            bulletTrans.position = trans.position;
            bulletTrans.rotation = trans.rotation;
            bullet.Init(mBulletPoint);               
            bullet.gameObject.SetActive(true);
        }
    }
}