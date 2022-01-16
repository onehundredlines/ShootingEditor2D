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
            var bullet = Instantiate(mBullet);
            if (bullet == null) return;
            var trans = mBulletPoint.transform;
            bullet.transform.position = trans.position;
            bullet.Init(mBulletPoint);               
            bullet.gameObject.SetActive(true);
            Destroy(bullet, 2f);
        }
    }
}